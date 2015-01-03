using UnityEngine;
using System.Collections;

public class RotateAroundY : MonoBehaviour 
{
	public float speed = 1f;
	

	void Update () 
	{
		transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * speed);
	}
}
