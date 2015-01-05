using System;
using UnityEngine;
using System.Collections;

public enum TriggerCrossing
{
	OneWay,
	MoreWays
};

public class TriggerCrossroad : MonoBehaviour {
	
	public TriggerCrossing crossingType;
	
	float collisionTolerance = 0.1f;
	bool isLocked;
	//float playerSpeed;
	
	
	void Awake()
	{
		isLocked = false;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag != "Player")
			return;
		
		// change trigger's rotation to player's rotation (same directions)
		PlayerMecanimController player = other.gameObject.GetComponent<PlayerMecanimController>();
		Quaternion playerRotation = player.gameObject.transform.rotation; 
		transform.rotation = playerRotation;player.angle = 0f;
		
		CheckPlayerPossibleDirectionsForPlayer(player);
		
		switch (crossingType) 
		{
		case TriggerCrossing.OneWay:
			if(player.rightArrow)
				player.angle = 90;
			else if(player.leftArrow)
				player.angle = -90;
			break;
		case TriggerCrossing.MoreWays:
			Debug.Log ("ENTER chanceToChoice");
			// signal player is on crossing
			player.chanceToChoice = true;
			// slow the player
			player.SlowDownPlayer ();
			break;
		}
	}
	
	
	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag != "Player")
			return;
		
		// inside of crossing change direction and move 
		PlayerMecanimController player = other.gameObject.GetComponent<PlayerMecanimController>();
		Vector3 playerPos = player.gameObject.transform.position; playerPos.y = 0;
		Vector3 triggerPos = transform.position; triggerPos.y = 0;
		
		float distance = Vector3.Distance(triggerPos, playerPos);
		distance = Mathf.Abs(distance);
		
		// when player is on middle of crossing
		if(!isLocked && distance >= 0 && distance < collisionTolerance)
		{
			isLocked = true;
			Debug.Log ("#### trigger ### ");
			switch(crossingType)
			{
			case TriggerCrossing.OneWay:
				player.MoveOverCrossroad(triggerPos);
				break;
			case TriggerCrossing.MoreWays:
				// if player didnt choose clear chance to choice
				player.chanceToChoice = false;
				// go exacly on the middle and set way to move (rotate and push)
				player.MoveOverCrossroad(triggerPos);
				player.AcceleratePlayer();
				
				Debug.Log ("# " + gameObject.name + " dist: " + distance);
				Debug.Log ("# Trigger: " + triggerPos + " Player: " + playerPos);
				
				break;
			}
		}
		
		
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag != "Player")
			return;
		
		PlayerMecanimController player = other.gameObject.GetComponent<PlayerMecanimController>();
		player.ResetDirections();
		isLocked = false;
	}
	
	void CheckPlayerPossibleDirectionsForPlayer(PlayerMecanimController player)
	{
		Ray rayLeft = new Ray(transform.position, 	-transform.right); 
		Ray rayRight = new Ray(transform.position, 	transform.right);    
		Ray rayForward = new Ray(transform.position, transform.forward);   
		//Ray rayDown = new Ray(transform.position,-transform.forward); 
		
		if (!Physics.Raycast (rayLeft, 2)) 
		{
			player.leftArrow = true;
		}
		if (!Physics.Raycast (rayRight, 2)) 
		{
			player.rightArrow = true;
		}
		if (!Physics.Raycast (rayForward, 2)) 
		{
			player.upArrow = true;
		} 
		Debug.Log (string.Format("# Possible directions: L:{0} R:{1} UP:{2} ", 
		                         player.leftArrow, player.rightArrow, player.upArrow));
	}
}
