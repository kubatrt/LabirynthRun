using UnityEngine;
using System.Collections;

public enum TrapType : int
{
	None = 0,
	Spikes,
	Saw,
	Hole
}

public enum TrapOrientation
{
	NorthSouth,
	EastWest
}

public class Trap : MonoBehaviour 
{
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			other.GetComponent<PlayerMecanimController>().Kill();
		}
	}
}