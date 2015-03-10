using UnityEngine;
using System.Collections;

public class TriggerGroundTrap : MonoBehaviour 
{
	PlayerMecanimController player;
	
	void Start()
	{
		player = GameObject.FindObjectOfType<PlayerMecanimController>();
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag != "Player")
			return;
		
		//Quaternion playerRotation = player.gameObject.transform.rotation; 
		//transform.rotation = playerRotation;

	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag != "Player")
			return;
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag != "Player")
			return;

		player.RunPlayer ();
	}

}
