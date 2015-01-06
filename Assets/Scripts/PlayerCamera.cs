using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour 
{ 
	// TODO:
	public float slowFov = 50f;
	public float normalFov = 60f;
	public float runFov = 70f;

	public PlayerMecanimController player;

	public void Update()
	{

	}

	public void AdjustFovToPlayerSpeed()
	{
		if(player == null) 
			return;

		float s = player.normalSpeed / player.speed;
		float fov = normalFov * s;
		camera.fieldOfView = Mathf.Clamp(fov, slowFov, runFov);
	}
}
