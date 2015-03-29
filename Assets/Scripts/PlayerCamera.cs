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
	public float slowFov = 55f;
	public float normalFov = 65f;
	public float runFov = 75f;

	public PlayerMecanimController player;
	Transform playerTransform;

	public float cameraCoroutineTime = 3f;

	public float dampTime;
	private Vector3 velocity = Vector3.zero;
	private float yVelocity = 0.0F;

	void Awake()
	{
		Instance = this;

	}

	public void Start()
	{
		startupPosition = transform.position;
		startupRotation = transform.rotation;
		
		dampTime = 0.3f;
	}

	public void SmoothFollow()
	{
		Transform target = player.transform;
		if(target)
		{
			Vector3 targetPos = target.position + (target.forward * -2) +  (target.up * 2.5f);
			Vector3 newPosition =   Vector3.SmoothDamp(transform.position, targetPos, ref velocity, dampTime);
			transform.position = newPosition;

			float yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref yVelocity, dampTime);
			Vector3 newRotation = new Vector3(transform.eulerAngles.x, yAngle, transform.eulerAngles.z);
			transform.eulerAngles = newRotation;
		}
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
		StartCoroutine (LerpFov(camera.fieldOfView, slowFov,1f));
	}

	public void SpeedUpFov()
	{
		StartCoroutine (LerpFov(camera.fieldOfView, runFov,1f));
	}

	public void NormalizeFov()
	{
        StopCoroutine("LerpFov");
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

	public void ResetCameraTransform()
	{
		transform.position = startupPosition;
		transform.rotation = startupRotation;
		Debug.Log ("ResetCameraTransform()");
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
		Debug.Log ("RestartCamera()");
	}

	public void StartCamera()
	{
		playerTransform = player.transform;
		Vector3 runGamePosition = playerTransform.position 
			+ (playerTransform.forward * -2)
			+ (playerTransform.up * 2.5f);
		Debug.Log ("StartCamera() runGamePosition: " + runGamePosition);
		StartCoroutine (LerpPosition(transform.position, runGamePosition, cameraCoroutineTime));

		Vector3 runGameRotation = 
			new Vector3 (playerTransform.eulerAngles.x + 30,
			             playerTransform.eulerAngles.y,
			             playerTransform.eulerAngles.z);
		StartCoroutine (LerpRotation(transform.eulerAngles, runGameRotation, cameraCoroutineTime));
	}

    public void LevelEndCameraAnimation()
    {
        playerTransform = player.transform;
        Vector3 runGamePosition = playerTransform.position
            + (playerTransform.forward * +2.5f)
            + (playerTransform.up * 2.5f);
        StartCoroutine(LerpPosition(transform.position, runGamePosition, 2f));

        Vector3 runGameRotation =
            new Vector3(playerTransform.eulerAngles.x + 40,
                         playerTransform.eulerAngles.y + 180,
                         playerTransform.eulerAngles.z);
        StartCoroutine(LerpRotation(transform.eulerAngles, runGameRotation, 2f));
    }

	#region Coroutines
	IEnumerator LerpPosition(Vector3 from, Vector3 to, float exTime)
	{
		float startTime = Time.time;
		float dTime = 0;
		float n = 1 / exTime;
		
		while (dTime < exTime) 
		{
            if(GameManager.Instance.state != GameState.Pause)
            {
                transform.position = Vector3.Lerp(from, to, dTime * n);
                yield return null;
                dTime = Time.time - startTime;
            }
            else
            {
                yield return null;
                startTime = Time.time - dTime;
            }
		}
		Debug.Log ("In corutine: to = " + to + " position = " + transform.position);
		transform.position = to;
        GameManager.Instance.RunGame();
	}
	
	IEnumerator LerpRotation(Vector3 from, Vector3 to, float exTime)
	{
		float startTime = Time.time;
		float dTime = 0;
		float n = 1 / exTime;
		
		while (dTime <= exTime) 
		{
            if (GameManager.Instance.state != GameState.Pause)
            {
                transform.eulerAngles = Vector3.Lerp(from, to, (Time.time - startTime) * n);
                yield return null;
                dTime = Time.time - startTime;
            }
            else
            {
                yield return null;
                startTime = Time.time - dTime;
            }
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
	}
	#endregion
}
