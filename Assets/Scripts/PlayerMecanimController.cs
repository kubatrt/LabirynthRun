using UnityEngine;
using System.Collections;



public class PlayerMecanimController : MonoBehaviour 
{
	// player states
	public bool isMoving = false;
	public bool isAlive = false;
	public bool isPaused = false;
	
	public float minSpeed = 0.5f;
	public float normalSpeed = 5f;
	public float runSpeed = 7f;
	public float speed;
	public float angle;
	public int failures;

	//  timers
	[SerializeField] float rotationTime;
	public float gameTimer; // TEMP
	float coroutineTimer;
	float startTime = 3f;
	float cameraCoroutineTime = 2.9f;

	static readonly float rotationLeft = -90f;
	static readonly float rotationRight = 90f;

	Vector3 	startupPosition;
	Quaternion 	startupRotation;
	Animator 	animator;
	public PlayerCamera	playerCamera;
	
	public QuickTimeEvent qte;

	void Awake()
	{
		animator = GetComponent<Animator> ();
		playerCamera = transform.GetComponentInChildren<PlayerCamera>();
		playerCamera.player = this;
		qte = GameObject.FindWithTag("QTE").GetComponent<QuickTimeEvent>();
		qte.player = this;
	}
	
	void Start () 
	{
		minSpeed = 0.5f;
		normalSpeed = 5;
		rotationTime  = 0.25f;
		
		startupPosition = transform.position;
		startupRotation = transform.rotation;
		animator.SetBool ("Run", false);
		animator.SetBool ("Ded", false);

		qte.gameObject.SetActive(false);
		playerCamera.transform.eulerAngles = 
			new Vector3 (90,playerCamera.transform.eulerAngles.y,playerCamera.transform.eulerAngles.z);
	}
	
	void Update () 
	{
		if(isAlive && GameManager.Instance.state == GameState.Run) 
		{
			Move();
			gameTimer += Time.deltaTime;
			//playerCamera.AdjustFovToPlayerSpeed();
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.transform.tag == "Wall")
		{
			ToggleMoving();
			isAlive = false;
			transform.Translate(new Vector3(0,0,-0.25f));
			SetDedAnim();
		}
	}

	public void StartPlayerGame()
	{
		Invoke ("StartPlayerCamera",startTime);
		Invoke ("StartPlayer", cameraCoroutineTime+startTime);
	}

	void StartPlayerCamera()
	{
		if(transform.eulerAngles.y == 0)
		{
			StartCoroutine( LerpCameraPosition(playerCamera.transform.position,new Vector3 (0,2.5f,-1.75f), cameraCoroutineTime));
			StartCoroutine (LerpCameraRotation (playerCamera.transform.eulerAngles,new Vector3(30,0,0),cameraCoroutineTime));
		}
		else if(transform.eulerAngles.y > 89 && transform.eulerAngles.y < 91)
		{
			StartCoroutine( LerpCameraPosition(playerCamera.transform.position,new Vector3 (-1.75f,2.5f,0), cameraCoroutineTime));
			StartCoroutine (LerpCameraRotation (playerCamera.transform.eulerAngles,new Vector3(30,90,0),cameraCoroutineTime));
		}
	}

	void StartPlayer()
	{
		speed = normalSpeed;
		ToggleMoving ();
		SetMovingAnim ();

		failures = 0;
		isAlive = true;
		gameTimer = 0;
	}

	void StopPlayer()
	{
		ToggleMoving ();
		transform.position = startupPosition;
		transform.rotation = startupRotation;
		ResetAnimations();
	}

	void ResetPlayer()
	{
		transform.position = startupPosition;
		transform.rotation = startupRotation;
		ResetAnimations();
		Invoke("StartPlayer", 1f);
	}

	public void TogglePlayerPause()
	{
		isPaused = !isPaused;
	}

	#region Crossroads
	public void EnterCrossroad(MoveDirections directions, TriggerCrossing crossingType)
	{
		angle = 0f;

		switch (crossingType) 
		{
			case TriggerCrossing.OneWay:
				Debug.Log ("# EnterCrossroad:OneWay");
				if(directions.Right)
					angle = rotationRight;
				else if(directions.Left)
					angle = rotationLeft;
				break;
				
			case TriggerCrossing.MoreWays:
				Debug.Log ("# EnterCrossroad:MoreWays");

				//float timeForDecision = 2f;
				SlowDownMovement();
				StartQTE(directions);

				break;
		}
	}
	
	public void MoveOverCrossroad(Vector3 triggerPos, TriggerCrossing crossingType)
	{
		transform.position = triggerPos;

		if(angle != 0)
		{					
			ToggleMoving();
			Rotate(rotationTime); // turn and go
			Invoke ("ToggleMoving", (rotationTime + 0.05f));
		}

		if(crossingType == TriggerCrossing.MoreWays) {
			AccelerateMovement();
			EndQTE();
		}
	}
	#endregion

	void StartQTE(MoveDirections dirs)
	{
		qte.directions = dirs;
		qte.gameObject.SetActive(true);
	}

	void EndQTE()
	{
		if(qte.noChoice)
			failures++;
		qte.gameObject.SetActive(false);
	}

	#region Player Movements
	void Move()
	{
		if(isMoving == true) {
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}
	}

	public void ToggleMoving() 
	{ 
		isMoving = !isMoving; 
	}

	public void GoForward()
	{
		angle = 0;
		BreakSlowAndGo();
	}

	public void GoLeft()
	{
		angle = rotationLeft;
		BreakSlowAndGo();
	}

	public void GoRight()
	{
		angle = rotationRight;
		BreakSlowAndGo();
	}
	#endregion

	#region Animations
	public void SetMovingAnim() 
	{ 
		ResetAnimations();
		animator.SetBool("Run", true);
	}
	
	void SetSlowDownAnim(bool choice)
	{
		animator.SetBool ("SlowDown", choice);
	}
	
	float SetDedAnim()
	{
		ResetAnimations();
		animator.SetBool ("Ded", true);
		return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;	// just cheking
	}
	
	public void SetCelebrateAnim()
	{
		ResetAnimations();
		animator.SetBool ("Celebrate", true);
	}

	public void ResetAnimations()
	{
		animator.SetBool("Run", false);
		animator.SetBool("Ded", false);
		animator.SetBool("SlowDown", false);
		animator.SetBool ("Celebrate", false);
	}

	public void AnimEvent_DeadEnd()
	{
		Invoke("ResetPlayer", 2f);
	}
	#endregion

	public void Rotate(float exTime)
	{
		Vector3 currentRotation = transform.eulerAngles;
		StartCoroutine(LerpRotation(currentRotation, new Vector3(currentRotation.x, angle, currentRotation.z), exTime));
	}
	
	public void SlowDownMovement()
	{
		StartCoroutine( LerpSpeed(speed, minSpeed, 0.3f));
		SetSlowDownAnim(true);
	}
	
	public void AccelerateMovement()
	{
		StartCoroutine( LerpSpeed(speed, normalSpeed, 0.3f));
		SetSlowDownAnim(false);
	}
	
	public void BreakSlowAndGo()
	{
		StopCoroutine("LerpSpeed");
		StartCoroutine( LerpSpeed(speed, normalSpeed, coroutineTimer));
		SetSlowDownAnim(false);
	}

	#region Coroutines

	IEnumerator LerpRotation(Vector3 from, Vector3 to, float exTime)
	{
		float startTime = Time.time;
		float n = 1 / exTime;
		to = new Vector3 (to.x, to.y + from.y, to.z);
		
		while (Time.time - startTime <= exTime) 
		{
			transform.eulerAngles = Vector3.Lerp(from, to, (Time.time - startTime)*n);
			yield return null;
		}
		transform.eulerAngles = to;
	}
	
	IEnumerator LerpSpeed(float a, float b, float exTime)
	{
		float startTime = Time.time;
		Vector3 A = Vector3.zero; A.x = a;
		Vector3 B = Vector3.zero; B.x = b;
		float n = 1 / exTime;
		
		while(Time.time - startTime <= exTime)
		{
			speed = Vector3.Lerp(A, B, (Time.time - startTime)*n).x;
			yield return null;
		}
		coroutineTimer = Time.time - startTime;
		speed = B.x;
	}

	IEnumerator LerpCameraPosition(Vector3 from, Vector3 to, float exTime)
	{
		float startTime = Time.time;
		float n = 1 / exTime;
		
		while (Time.time - startTime <= exTime) 
		{
			playerCamera.transform.position = Vector3.Lerp(from, to, (Time.time - startTime)*n);
			yield return null;
		}
		playerCamera.transform.position = to;
	}

	IEnumerator LerpCameraRotation(Vector3 from, Vector3 to, float exTime)
	{
		float startTime = Time.time;
		float n = 1 / exTime;
		
		while (Time.time - startTime <= exTime) 
		{
			playerCamera.transform.eulerAngles = Vector3.Lerp(from, to, (Time.time - startTime)*n);
			yield return null;
		}
		playerCamera.transform.eulerAngles = to;
	}

	#endregion
}
