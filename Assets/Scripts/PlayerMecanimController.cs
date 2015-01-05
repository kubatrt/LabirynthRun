using UnityEngine;
using System.Collections;



public class PlayerMecanimController : MonoBehaviour 
{
	// player states
	public bool isMoving = false;
	public bool isAlive = false;
	
	public float minSpeed = 0.5f;
	public float maxSpeed = 5;
	public float speed;
	public float angle;
	public int failures;
	
	//  timers
	[SerializeField] float rotationTime;
	public float gameTimer; // TEMP
	float coroutineTimer;

	static readonly float rotationLeft = -90f;
	static readonly float rotationRight = 90f;

	Vector3 	startupPosition;
	Quaternion 	startupRotation;
	Animator 	animator;
	
	public QuickTimeEvent qte;

	void Awake()
	{
		animator = GetComponent<Animator> ();
		qte = GameObject.FindWithTag("QTE").GetComponent<QuickTimeEvent>();
		qte.player = this;
	}
	
	void Start () 
	{
		minSpeed = 0.5f;
		maxSpeed = 5;
		rotationTime  = 0.25f;
		
		startupPosition = transform.position;
		startupRotation = transform.rotation;
		animator.SetBool ("Run", false);
		animator.SetBool ("Ded", false);

		qte.gameObject.SetActive(false);
		Invoke ("StartPlayer", 1f);
	}
	
	void Update () 
	{
		if(isAlive) {
			Move();
			gameTimer += Time.deltaTime;
		}
	}
	
	
	void OnCollisionEnter(Collision col)
	{
		if (col.transform.tag == "Wall")
		{
			ToggleMoving();
			transform.Translate(new Vector3(0,0,-0.25f));
			SetDedAnim();
			isAlive = false;
			Invoke("ResetPlayer", 3f);	// animation.Lenght
		}
	}

	
	void StartPlayer()
	{
		speed = maxSpeed;
		ToggleMoving ();
		SetMovingAnim ();
		failures = 0;
		isAlive = true;
		gameTimer = 0;
	}
	
	void ResetPlayer()
	{
		transform.position = startupPosition;
		transform.rotation = startupRotation;
		ResetAnimations();
		Invoke("StartPlayer", 1f);
	}
	
	void ResetAnimations()
	{
		animator.SetBool("Run", false);
		animator.SetBool("Ded", false);
		animator.SetBool("SlowDown", false);
	}

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

				float timeForDecision = 2f;
				SlowDownMovement();
				
				qte.directions = directions;
				qte.gameObject.SetActive(true);
				

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
			if(qte.noChoice)
				failures++;
			qte.gameObject.SetActive(false);
		}
	}
	
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




	
	void SetMovingAnim() 
	{ 
		ResetAnimations();
		animator.SetBool("Run", true);
	}
	
	void SetSlowDownAnim(bool choice)
	{
		animator.SetBool ("SlowDown", choice);
	}
	
	void SetDedAnim()
	{
		ResetAnimations();
		animator.SetBool ("Ded", true);
	}
	
	public void SetCelebrateAnim()
	{
		ResetAnimations();
		animator.SetBool ("Celebrate", true);
	}
	
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
		StartCoroutine( LerpSpeed(speed, maxSpeed, 0.3f));
		SetSlowDownAnim(false);
	}
	
	public void BreakSlowAndGo()
	{
		StopCoroutine("LerpSpeed");
		StartCoroutine( LerpSpeed(speed, maxSpeed, coroutineTimer));
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
		//coroutineTimer = Time.time - startTime;
		speed = B.x;
	}

	#endregion
}
