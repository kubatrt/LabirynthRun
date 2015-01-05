﻿using System;
using UnityEngine;
using System.Collections;

public enum TriggerCrossing
{
	OneWay,
	MoreWays
};

public class MoveDirections
{
	public bool Left {get; set; }
	public bool Right {get; set; }
	public bool Forward { get; set; }
	public bool Back { get; set; }
	
	public MoveDirections()
	{
		Left = Right = Forward = Back = false;
	}
};

public class TriggerCrossroad : MonoBehaviour {
	
	public TriggerCrossing crossingType;
	
	float collisionTolerance = 0.1f;
	bool isLocked;

	PlayerMecanimController player;
	
	void Start()
	{
		isLocked = false;
		player = GameObject.FindObjectOfType<PlayerMecanimController>();
	}


	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag != "Player")
			return;

		Quaternion playerRotation = player.gameObject.transform.rotation; 
		transform.rotation = playerRotation;

		MoveDirections directions = CheckPossibleDirections();
		player.EnterCrossroad(directions, crossingType );
	}
	
	
	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag != "Player")
			return;
		
		// inside of crossing change direction and move
		Vector3 playerPos = player.gameObject.transform.position; playerPos.y = 0;
		Vector3 triggerPos = transform.position; triggerPos.y = 0;

		float distance = Vector3.Distance(triggerPos, playerPos);
		distance = Mathf.Abs(distance);
		
		// when player is on middle of crossing, go out
		if(!isLocked && distance >= 0 && distance < collisionTolerance)
		{
			isLocked = true;
			player.MoveOverCrossroad(triggerPos, crossingType);

			Debug.Log ("# Leaving trigger # ");
		}
		
		//Debug.Log ("# " + gameObject.name + " Dist: " + distance + " # Pos: " + triggerPos + " PlayerPos: " + playerPos);
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag != "Player")
			return;

		// TODO: move to better place
		QuickTimeEvent qte = player.GetComponent<QuickTimeEvent>();
		if(qte != null && qte.noChoice)
		{
			Debug.Log ("### No choice");
			player.failures++;
		}
		Destroy(player.GetComponent<QuickTimeEvent>());
		isLocked = false;
	}
	
	MoveDirections CheckPossibleDirections()
	{
		MoveDirections availableDirs = new MoveDirections();
		float rayDistance = 2f;
		Ray rayLeft = new Ray(transform.position, 	-transform.right); 
		Ray rayRight = new Ray(transform.position, 	transform.right);    
		Ray rayForward = new Ray(transform.position, transform.forward);   
		//Ray rayDown = new Ray(transform.position,-transform.forward); 
		
		if (!Physics.Raycast (rayLeft, rayDistance)) 
		{
			availableDirs.Left = true;
		}
		if (!Physics.Raycast (rayRight, rayDistance)) 
		{
			availableDirs.Right = true;
		}
		if (!Physics.Raycast (rayForward, rayDistance)) 
		{
			availableDirs.Forward = true;
		} 
		return availableDirs;
	}
}
