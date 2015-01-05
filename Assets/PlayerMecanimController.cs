using UnityEngine;
using System.Collections;

public class PlayerMecanimController : MonoBehaviour 
{
	// player states
	public bool isMoving = false;
	public bool chanceToChoice = false;
	public bool isDead = false;
	
	public float minSpeed = 0.5f;
	public float maxSpeed = 5;
	public float speed;
	public float angle;
	
	//  timers
	public float coroutineTimer;
	public float rotationTime = 0.25f;
	
	//[SerializeField] float startupTimer = 0;
	
	// QTE crossing possibility of movement
	public bool leftArrow, rightArrow, upArrow;

	Vector3 	startupPosition;
	Quaternion 	startupRotation;
	
	Animator animator;
	
	
	void Awake()
	{
		animator = GetComponent<Animator> ();
	}
	
	void Start () 
	{
		minSpeed = 0.5f;
		maxSpeed = 5;
		rotationTime  = 0.25f;
		
		startupPosition = transform.position;
		startupRotation = transform.rotation;
		Invoke ("PlayerStart", 1f);
		
		animator.SetBool ("Run", false);
		animator.SetBool ("Ded", false);
	}
	
	void Update () 
	{
		Move();
	}
	
	
	void OnCollisionEnter(Collision col)
	{
		if (col.transform.tag == "Wall")
		{
			ToggleMoving();
			transform.Translate(new Vector3(0,0,-0.25f));
			SetDedAnim();
			RestartGame();
		}
	}
	
	
	void RestartGame()
	{
		Debug.Log ("### RestartGame ");
		Invoke("ResetPlayer", 3f);
		Invoke("ToggleMoving", 5f);
		Invoke("SetMovingAnim", 5f);
	}
	
	void StartPlayer()
	{
		speed = maxSpeed;
		ToggleMoving ();
		SetMovingAnim ();
		Debug.Log ("### StartPlayer ");
	}
	
	void ResetPlayer()
	{
		transform.position = startupPosition;
		transform.rotation = startupRotation;
		ResetAnimations ();
		ResetDirections ();
	}
	
	void ResetAnimations()
	{
		animator.SetBool("Run", false);
		animator.SetBool("Ded", false);
		animator.SetBool("SlowDown", false);
	}
	
	public void MoveOverCrossroad(Vector3 triggerPos)
	{
		ToggleMoving();
		transform.position = triggerPos;
		if(angle != 0)
		{					
			Rotate(rotationTime); // turn and go
			Invoke ("ToggleMoving", (rotationTime + 0.05f)); // TODO: MagicNumber
		}
		else
		{
			ToggleMoving(); // just go
		}
	}
	
	void Move()
	{
		if(isMoving == true)
			transform.Translate(0, 0, speed * Time.deltaTime);
	}
	
	public void ToggleMoving() 
	{ 
		isMoving = !isMoving; 
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
	
	public void SlowDownPlayer()
	{
		StartCoroutine ( LerpSpeed(speed, minSpeed, 0.3f));
		SetSlowDownAnim (true);
	}
	
	public void AcceleratePlayer()
	{
		StartCoroutine (LerpSpeed (speed, maxSpeed, 0.3f));
		SetSlowDownAnim (false);
	}
	
	public void BreakSlowAndGo()
	{
		StopCoroutine("LerpSpeed");
		StartCoroutine (LerpSpeed (speed, maxSpeed, coroutineTimer));
		SetSlowDownAnim (false);
	}
	
	public void ResetDirections()
	{
		upArrow = leftArrow = rightArrow = false;
	}
}
