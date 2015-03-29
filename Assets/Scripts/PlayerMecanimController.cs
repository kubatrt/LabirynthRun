using UnityEngine;
using System.Collections;


public class PlayerMecanimController : MonoBehaviour 
{
	public PlayerCamera playerCamera;

	// player states
	public bool isMoving = false;
	public bool isAlive = false;
	
	public float minSpeed = 0.5f;
	public float normalSpeed = 5f;
	public float runSpeed = 9f;
	public float speed;
	public float angle;

	// TODO: Move to separate component: PlayerStats
	public int failures;
	public int lives;
	public int mapsUses;
	public int score;

	//  timers
	[SerializeField] float rotationTime;
	float coroutineTimer;

	Vector3 	startupPosition;
	Quaternion 	startupRotation;
	Animator 	animator;
	bool IsGoodRotated;
	float AnimNormSpeed;
	
	private QTECrossroad qte;

	void Awake()
	{
		animator = GetComponent<Animator> ();
		qte = GameObject.FindWithTag("QTE").GetComponent<QTECrossroad>();

		Debug.Log ("Player.Awake()");
	}

	void Start () 
	{
		minSpeed = 0.5f;
		normalSpeed = 5;
        runSpeed = 9f;
		rotationTime  = 0.25f;
		lives = 3;
		mapsUses = 3;
		
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
			//if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
           	Move();
            
			playerCamera.SmoothFollow();
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
            StopAllCoroutines();
			if(lives>1)
				lives--;
			else 
				GameManager.Instance.ChangeGameState(GameState.EndLost);
		}
	}

	public void StartPlayer()
	{
		Debug.Log("StartPlayer()");
		speed = normalSpeed;
		ToggleMoving ();
		SetMovingAnim ();

		failures = 0;
		isAlive = true;

		RunPlayer ();
	}

	Vector3 startupPlayerRotation = Vector3.zero;
	public void SetStartupRotation(Vector3 rotation)
	{
		startupPlayerRotation = rotation;
		transform.Rotate(startupPlayerRotation);
	}

	/*public void SetStartupRotation()
	{

		Debug.Log (" Player Rot set0");
		while(IsGoodRotated == false) // INFINITE LOOP KURWAAAAAAAAAAAAAAAAAAAAAAA!!!!!!!!!!!!!!
		{
			Debug.Log (" Player Rot set1");
			Ray ray = new Ray(transform.position, transform.forward);
			if(Physics.Raycast(ray,2f))
			{
				Debug.Log (" Player Rot set2");
				transform.Rotate(new Vector3(0,90f,0));
				IsGoodRotated = false;
			}
			else
			{
				Debug.Log (" Player Rot set3");
				IsGoodRotated = true;
			}
		}
		IsGoodRotated = false;
		Debug.Log (" Player Rot set4");
	}*/

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

		// RestartCamera() REMOVED
		ResetAnimations();
		lives = 3;
		mapsUses = 3;
	}

	public void decreaseMaps()
	{
		mapsUses--;
	}

	public void RestartPlayer() // every single ded
	{
		isMoving = false;
		transform.position = startupPosition;
		transform.rotation = startupRotation;
		transform.Rotate(startupPlayerRotation);
		playerCamera.RestartCamera();
		ResetAnimations();
		mapsUses = 3;
		Invoke("StartPlayer", 1f);
	}

	void StartQTE(MoveDirections dirs)
	{
		qte.directions = dirs;
		qte.gameObject.SetActive(true);
	}
	
	void EndQTE()
	{
		if(qte.NoChoice)
			failures++;
		qte.gameObject.SetActive(false);
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
					angle = 90f;
				else if(directions.Left)
					angle = -90f;
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

		// leaving... it was more than 1 way crossroad?
		if(crossingType == TriggerCrossing.MoreWays) {
			AccelerateMovement();
			EndQTE();
		}
	}
	#endregion

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
		angle = -90f;
		BreakSlowAndGo();
	}

	public void GoRight()
	{
		angle = 90f;
		BreakSlowAndGo();
	}
	#endregion

	#region Animations
	public void SetJumpAnim()
	{
		animator.SetTrigger ("Jump");
	}

	public void SetRollAnim()
	{
		animator.SetTrigger("Roll");
	}

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
        SpeedUpOff();
		StartCoroutine( LerpSpeed(speed, minSpeed, 0.3f));
		SetSlowDownAnim(true);
		playerCamera.SlowDownFov ();
	}
	
	public void AccelerateMovement()
	{
		StartCoroutine( LerpSpeed(speed, normalSpeed, 0.3f));
		SetSlowDownAnim(false);
		playerCamera.NormalizeFov ();
	}
	
	public void BreakSlowAndGo()
	{
		StopCoroutine("LerpSpeed");
		StartCoroutine( LerpSpeed(speed, normalSpeed, coroutineTimer));
		SetSlowDownAnim(false);
		playerCamera.NormalizeFov ();
	}

	public void BackToNormalSpeed()
	{
        StopCoroutine("LerpSpeed");
		StartCoroutine( LerpSpeed(speed, normalSpeed, 0.3f));
		playerCamera.NormalizeFov ();
	}

    public void SpeedUpOn()
    {
        speed = runSpeed;
        SetSpeedUpAnimation(true);
    }

    public void SpeedUpOff()
    {
        speed = normalSpeed;
        SetSpeedUpAnimation(false);
    }

	public void RunPlayer()
	{
		float road = 0;

		RaycastHit hit;
		int wallsLayerMask = 1 << 9; 
		Ray ray = new Ray(transform.position, transform.forward);
		if (Physics.Raycast(ray, out hit, 100f, wallsLayerMask))
		{
			road = hit.distance - 3f;
			Debug.Log ("RunPlayer() (9) WALL:" + hit.distance + "Road:" + road + " hit: " + hit.collider.gameObject.name);
		}

		int triggersLayerMask = 1 << 10;
		ray = new Ray(transform.position, transform.forward);
		if (Physics.Raycast(ray, out hit, 100f, triggersLayerMask ))
		{
			Debug.Log ("RunPlayer() (10) TRIGGER:" + hit.distance);
		}

		int trapsLayerMask = 1 << 11; 
		ray = new Ray(transform.position, transform.forward);
		if (Physics.Raycast(ray, out hit, 100f, trapsLayerMask ))
		{
			Debug.Log ("RunPlayer() (11) TRAP:" + hit.distance);
		}


        /*
		const float roadDistanceThreshold = 2f;
		if(road > roadThreshold)
		{
			Debug.Log ("im accelerating....!!!!");
			StartCoroutine(LerpSpeed(speed, runSpeed, 0.3f));
			playerCamera.SpeedUpFov();
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
