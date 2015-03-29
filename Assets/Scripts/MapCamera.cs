using UnityEngine;
using System.Collections;

public class MapCamera : MonoBehaviour 
{
	void Start ()
	{
		camera.enabled = false;
	}

	public void SetPosition(float x, float y, float z)
	{
		transform.position = new Vector3 (x, y, z);
	}
	
	public void SetRotation(float x, float y, float z)
	{
		transform.eulerAngles = new Vector3 (x, y, z);
	}

	public void SetCameraSize(float size)
	{
		camera.orthographicSize = size;
	}
}
