using UnityEngine;
using System.Collections;



public class PlayerMecanimController : MonoBehaviour 
{
	// player states
	public bool isMoving = false;
	public bool isAlive = false;
	
	public float minSpeed = 0.5f;
	public float normalSpeed = 5f;
	public float runSpeed = 8f;
	public float speed;
	public float angle;
	public int failures;
	public int lives;
	public int maps;

	//  timers
	[SerializeField] float rotationTime;
	float coroutineTimer;

	static readonly float rotationLeft = -90f;
	static readonly float rotationRight = 90f;

	Vector3 	startupPosition;
	Quaternion 	startupRotation;
	Animator 	animator;
	bool IsGoodRotated;

	float AnimNormSpeed;
	
	public QuickTimeEvent qte;

	void Awake()
	{
		animator = GetComponent<Animator> ();
		qte = GameObject.FindWithTag("QTE").GetComponent<QuickTimeEvent>();
		qte.player = this;
		Debug.Log ("Player.Awake()");
	}

	void Start () 
	{
		PlayerCamera.Instance.player = this;
		minSpeed = 0.5f;
		normalSpeed = 5;
		rotationTime  = 0.25f;
		lives = 3;
		maps = 3;
		
		startupPosition = transform.position;
		startupRotation = transform.rotation;
		ResetAnimations ();

		qte.gameObject.SetActive(false);
		Debug.Log ("Player.Start()");
	}

    void Update()
    {
        if (isAlive && GameManager.Instance.state == GameState.Run)
        {
            Move();
            PlayerCamera.Instance.SmoothFollow();
            AlignPlayer();
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
			if(lives>1)
				lives--;
			else 
				GameManager.Instance.ChangeGameState(GameState.EndLost);
		}
	}

	public void StartPlayer()
	{
		speed = normalSpeed;
		ToggleMoving ();
		SetMovingAnim ();

		failures = 0;
		isAlive = true;

		RunPlayer ();
	}

	public void SetRotation()
	{
		//Debug.Log (" Player Rot set0");
		while(IsGoodRotated == false)
		{
			//Debug.Log (" Player Rot set1");
			Ray ray;
			ray = new Ray(transform.position, transform.forward);
			if(Physics.Raycast(ray,2f))
			{
				//Debug.Log (" Player Rot set2");
				transform.Rotate(new Vector3(0,90f,0));
				IsGoodRotated = false;
			}
			else
			{
				//Debug.Log (" Player Rot set3");
				IsGoodRotated = true;
			}
		}
		IsGoodRotated = false;
		//Debug.Log (" Player Rot set4");
	}

	public void AlignPlayer()
	{
		if(transform.eulerAngles.y > -0.5f && transform.eulerAngles.y < 0.5f)
			transform.eulerAngles = new Vector3(0,0,0);
		if(transform.eulerAngles.y > 89.5f && transform.eulerAngles.y < 90.5f)
			transform.eulerAngles = new Vector3(0,90,0);
		if(transform.eulerAngles.y > 179.5f && transform.eulerAngles.y < 180.5f)
			transform.eulerAngles = new Vector3(0,180,0);
		if(transform.eulerAngles.y > 269.5f && transform.eulerAngles.y < 270.5f)
			transform.eulerAngles = new Vector3(0,270,0);
	}

	public void ResetPlayer() // on game over
	{
		isMoving = false;
		transform.position = startupPosition;
		transform.rotation = startupRotation;
		SetRotation ();
		PlayerCamera.Instance.RestartCamera ();
		ResetAnimations();
		lives = 3;
		maps = 3;
	}

	public void decreaseMaps()
	{
		maps--;
	}

	public void RestartPlayer() // every single ded
	{
		isMoving = false;
		transform.position = startupPosition;
		transform.rotation = startupRotation;
		SetRotation ();
		PlayerCamera.Instance.RestartCamera ();
		ResetAnimations();
		maps = 3;
		Invoke("StartPlayer", 1f);
	}

	#region Crossroads
	public void EnterCrossroad(MoveDirections directions, TriggerCrossing crossingType)
	{
		AlignPlayer ();

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
		if(isMoving == true) 
		{
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
        animator.SetBool("SpeedUp", false);
	}

	public void AnimEvent_DeadEnd()
	{
		if(GameManager.Instance.state == GameState.Run)
			Invoke("RestartPlayer", 2f);
	}

	public void PauseAnimations()
	{
		animator.enabled = false;
	}

	public void UnpauseAnimations()
	{
		animator.enabled = true;
	}

    void SetSpeedUpAnimation(bool choice)
    {
        animator.SetBool("SpeedUp", choice);
    }
	#endregion

	#region Player Actions
	public void Rotate(float exTime)
	{
		Vector3 currentRotation = transform.eulerAngles;
		StartCoroutine(LerpRotation(currentRotation, new Vector3(currentRotation.x, angle, currentRotation.z), exTime));
	}
	
	public void SlowDownMovement()
	{
		StartCoroutine( LerpSpeed(speed, minSpeed, 0.3f));
		SetSlowDownAnim(true);
		PlayerCamera.Instance.SlowDownFov ();
	}
	
	public void AccelerateMovement()
	{
		StartCoroutine( LerpSpeed(speed, normalSpeed, 0.3f));
		SetSlowDownAnim(false);
		PlayerCamera.Instance.NormalizeFov ();
	}
	
	public void BreakSlowAndGo()
	{
		StopCoroutine("LerpSpeed");
		StartCoroutine( LerpSpeed(speed, normalSpeed, coroutineTimer));
		SetSlowDownAnim(false);
		PlayerCamera.Instance.NormalizeFov ();
	}

	public void BackToNormalSpeed()
	{
        StopCoroutine("LerpSpeed");
		StartCoroutine( LerpSpeed(speed, normalSpeed, 0.3f));
		PlayerCamera.Instance.NormalizeFov ();
	}

    public void SpeedUpOn()
    {
        //speed = normalSpeed*2;
        StartCoroutine(LerpSpeed(speed, normalSpeed * 2, 0.1f));
        SetSpeedUpAnimation(true);
    }

    public void SpeedUpOff()
    {
       // speed = normalSpeed;
        StopCoroutine("LerpSpeed");
        StartCoroutine(LerpSpeed(speed, normalSpeed, 0.1f));
        SetSpeedUpAnimation(false);
    }

	public void RunPlayer()
	{
		const float farDistance = 3f;
		float road = 0;

		RaycastHit hit;
		Ray ray = new Ray(transform.position, transform.forward);
		if (Physics.Raycast(ray, out hit))
		{
			road = hit.distance - farDistance;
		}
		Debug.Log ("RunPlayer() Distance:" + hit.distance + "Road:" + road);


		const float roadThreshold = 2f;
		if(hit.distance > roadThreshold)
		{
			Debug.Log ("im accelerating....!!!!");
			StartCoroutine(LerpSpeed(speed, runSpeed, 0.3f));
			PlayerCamera.Instance.SpeedUpFov();
			Invoke ("BackToNormalSpeed", road/runSpeed);
			Debug.Log ("Time:" + road/runSpeed);
		}

        /*
		const float roadThreshold = 2f;
		if(road > roadThreshold)
		{
			Debug.Log ("im accelerating....!!!!");
			StartCoroutine(LerpSpeed(speed, runSpeed, 0.3f));
			PlayerCamera.Instance.SpeedUpFov();
			Invoke ("BackToNormalSpeed", road/runSpeed);
			Debug.Log ("Time:" + road/runSpeed);
		}*/
	}
	#endregion

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
	#endregion
}
