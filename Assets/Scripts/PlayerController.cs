using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private static PlayerController instance;
	public static PlayerController Instance { get { return instance; } }

	// player states
	bool isStarted = false;
	public bool isMoving = false;
	public bool chanceToChoice = false;
	public bool isDead = false;
	Vector3 startPosition;
	Quaternion startRotation;

	// possibility of movement
	public bool leftArrow, rightArrow, upArrow;

	// player stats
	public float minSpeed;
	public float maxSpeed;
	public float speed;
	public float Speed
	{
		get { return speed; }
		set { speed = value; }
	}

	public int angle;
	public int Angle
	{
		get { return angle; }
		set { angle = value; }
	}

	//  timers
	public float corTimer;
	public float gameTime;  
	public float rotateTime;

	// animations
	Animator animator;
	
	// Use this for initialization
	void Awake()
	{
		instance = this;
		animator = GetComponent<Animator> ();
	}
	void Start () 
	{
		startPosition = transform.position;
		startRotation = transform.rotation;
		minSpeed = 0.5f;
		maxSpeed = 5;
		rotateTime = 0.25f;
		gameTime = 3;
		//Invoke ("PlayerStart", gameTime + 2);
		animator.SetBool ("Run", false);
		animator.SetBool ("Ded", false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		gameTime -= Time.deltaTime;

		if (!isStarted && gameTime < -2) 
		{
			StartPlayer();
		}

		Move();
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.transform.tag == "Wall")
		{
			changeMoving();
			transform.Translate(new Vector3(0,0,-0.25f));
			SetDedAnim();
			Invoke("ResetPlayer",3);
			Invoke("changeMoving",5);
			Invoke("SetMovingAnim",5);

		}
	}

	void StartPlayer()
	{
		speed = maxSpeed;
		changeMoving ();
		SetMovingAnim ();
		isStarted = true;
	}

	void ResetPlayer()
	{
		transform.position = startPosition;
		transform.rotation = startRotation;
		ResetPlayerAnim ();
		resetDirections ();
	}

	void ResetPlayerAnim()
	{
		// all animations set at false
		animator.SetBool("Run", false);
		animator.SetBool("Ded", false);
		animator.SetBool ("SlowDown", false);
	}

	void Move()
	{
		if(isMoving == true)
			transform.Translate(0,0,speed * Time.deltaTime);
	}

	public void changeMoving() { isMoving = !isMoving; }

	void SetMovingAnim() 
	{ 
		ResetPlayerAnim();
		animator.SetBool("Run", true);
	}

	void SetSlowDownAnim(bool choice)
	{
		animator.SetBool ("SlowDown", choice);
	}

	void SetDedAnim()
	{
		ResetPlayerAnim();
		animator.SetBool ("Ded", true);
	}

	public void SetCelebrateAnim()
	{
		ResetPlayerAnim();
		animator.SetBool ("Celebrate", true);
	}

	public void DoSomethingFun(Vector3 triggerPos)
	{
		changeMoving ();
		transform.position = triggerPos;
		if(angle != 0)
		{					// turn and go
			Rotate(rotateTime);
			Invoke ("changeMoving", (rotateTime + 0.05f));
		}
		else
		{
			changeMoving(); // just go
		}
	}

	IEnumerator LerpRotation(Vector3 from, Vector3 to, float exTime)
	{
		float startTime = Time.time;
		float n = 1 / exTime;
		to = new Vector3 (to.x, to.y + from.y, to.z);
		while (Time.time - startTime <= exTime) {
			transform.eulerAngles = Vector3.Lerp(from, to, (Time.time - startTime)*n);
			yield return null;
		}
		transform.eulerAngles = to;
	}

	public void Rotate(float exTime)
	{
		Vector3 currentRot = transform.eulerAngles;
		StartCoroutine(LerpRotation(currentRot, new Vector3(currentRot.x, angle, currentRot.z), exTime));
	}

	IEnumerator LerpSpeed(float a, float b, float exTime)
	{
		float startTime = Time.time;
		Vector3 A = Vector3.zero;
		A.x = a;
		Vector3 B = Vector3.zero;
		B.x = b;
		float n = 1/exTime;
		while(Time.time - startTime <= exTime){
			speed = Vector3.Lerp(A, B, (Time.time - startTime)*n).x;
			yield return null;
		}
		corTimer = Time.time - startTime;
		speed = B.x;
	}

	public void slowDownPlayer()
	{
		StartCoroutine (LerpSpeed (speed, minSpeed, 0.3f));
		// slow down animation on
		SetSlowDownAnim (true);
	}

	public void acceleratePlayer()
	{
		StartCoroutine (LerpSpeed (speed, maxSpeed, 0.3f));
		// slow down animation off
		SetSlowDownAnim (false);
	}

	public void breakSlowAndGo()
	{
		StopCoroutine("LerpSpeed");
		StartCoroutine (LerpSpeed (speed, maxSpeed, corTimer));
		// slow down animation off
		SetSlowDownAnim (false);
	}

	public void resetAngle()
	{
		angle = 0;
	}
	
	public void resetDirections()
	{
		upArrow = leftArrow = rightArrow = false;
	}
}
