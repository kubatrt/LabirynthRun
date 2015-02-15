using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour 
{ 
	static PlayerCamera instance;
	public static PlayerCamera Instance { 
		get; 
		private set;
	}

	Vector3 startupPosition;
	Quaternion startupRotation;

	// TODO:
	public float slowFov = 60f;
	public float normalFov = 65f;
	public float runFov = 70f;

	public PlayerMecanimController player;
	Transform playerTransform;

	public float cameraCoroutineTime = 3f;

	void Awake()
	{
		Instance = this;
	}

	public void Start()
	{
		//startupPosition = transform.position;
		startupRotation = transform.rotation;
	}

	public void FollowThePlayer()
	{
		playerTransform = player.transform;
		Vector3 runGamePosition = playerTransform.position 
			+ (playerTransform.forward * -2)
				+ (playerTransform.up * 2.5f);
		transform.position = Vector3.Lerp (transform.position, runGamePosition, 0.25f);
		Vector3 runGameRotation = 
			new Vector3 (playerTransform.eulerAngles.x + 30,
			             playerTransform.eulerAngles.y,
			             playerTransform.eulerAngles.z);
	
		transform.eulerAngles = Vector3.Lerp (transform.eulerAngles, runGameRotation, 0.25f);
	}

	public void AdjustFovToPlayerSpeed()
	{
		if(player == null) 
			return;

		float s = player.normalSpeed / player.speed;
		float fov = normalFov * s;
		camera.fieldOfView = Mathf.Clamp(fov, slowFov, runFov);
	}

	public void SlowDownFov()
	{
		StartCoroutine (LerpFov(camera.fieldOfView, slowFov,0.5f));
	}

	public void SpeedUpFov()
	{
		StartCoroutine (LerpFov(camera.fieldOfView, runFov,0.5f));
	}

	public void NormalizeFov()
	{
		StartCoroutine (LerpFov(camera.fieldOfView, normalFov,0.5f));
	}

	public void SetStartUpPosition(float x, float y, float z)
	{
		startupPosition = new Vector3 (x, y, z);
	}

	public void SetPosition(float x, float y, float z)
	{
		transform.position = new Vector3 (x, y, z);
	}

	public void SetRotation(float x, float y, float z)
	{
		transform.eulerAngles = new Vector3 (x, y, z);
	}

	public void ResetCamera()
	{
		transform.position = startupPosition;
		transform.rotation = startupRotation;
	}

	public void RestartCamera() 		// on player ded
	{
		playerTransform = player.transform;
		Vector3 runGamePosition = playerTransform.position 
			+ (playerTransform.forward * -2)
				+ (playerTransform.up * 2.5f);
		Vector3 runGameRotation = 
			new Vector3 (playerTransform.eulerAngles.x + 30,
			             playerTransform.eulerAngles.y,
			             playerTransform.eulerAngles.z);
		transform.position = runGamePosition;
		transform.eulerAngles = runGameRotation;
	}

	public void StartCamera()
	{
		playerTransform = player.transform;
		Vector3 runGamePosition = playerTransform.position 
			+ (playerTransform.forward * -2)
			+ (playerTransform.up * 2.5f);
		Debug.Log ("runGamePosition: " + runGamePosition);
		StartCoroutine (LerpPosition(transform.position, runGamePosition, cameraCoroutineTime));

		Vector3 runGameRotation = 
			new Vector3 (playerTransform.eulerAngles.x + 30,
			             playerTransform.eulerAngles.y,
			             playerTransform.eulerAngles.z);
		StartCoroutine (LerpRotation(transform.eulerAngles, runGameRotation, cameraCoroutineTime));
		//Invoke ("PinCameraToPlayer", cameraCoroutineTime);
	}

	void PinCameraToPlayer()
	{
		transform.parent = playerTransform;
	}

	public void UnPinCamereaFromPlayer()
	{
		transform.parent = null;
		Debug.Log ("Un pin camera!");
	}

	#region Coroutines
	IEnumerator LerpPosition(Vector3 from, Vector3 to, float exTime)
	{
		float startTime = Time.time;
		float dTime = 0;
		float n = 1 / exTime;
		
		while (dTime < exTime) 
		{
			transform.position = Vector3.Lerp(from, to, dTime*n);
			yield return null;
			dTime = Time.time - startTime;
		}
		Debug.Log ("In corutine: to = " + to + " position = " + transform.position);
		transform.position = to;
	}
	
	IEnumerator LerpRotation(Vector3 from, Vector3 to, float exTime)
	{
		float startTime = Time.time;
		float dTime = 0;
		float n = 1 / exTime;
		
		while (dTime <= exTime) 
		{
			transform.eulerAngles = Vector3.Lerp(from, to, (Time.time - startTime)*n);
			yield return null;
			dTime = Time.time - startTime;
		}
		transform.eulerAngles = to;
	}

	IEnumerator LerpFov(float a, float b, float exTime)
	{
		float startTime = Time.time;
		Vector3 A = Vector3.zero; A.x = a;
		Vector3 B = Vector3.zero; B.x = b;
		float n = 1 / exTime;
		
		while(Time.time - startTime <= exTime)
		{
			camera.fieldOfView = Vector3.Lerp(A, B, (Time.time - startTime)*n).x;
			yield return null;
		}
		camera.fieldOfView = B.x;
	}
	#endregion
}
