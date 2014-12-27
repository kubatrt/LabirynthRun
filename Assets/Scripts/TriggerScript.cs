using UnityEngine;
using System.Collections;

public enum Crossing
{
	oneWay,
	moreWays
};

public class TriggerScript : MonoBehaviour {

	public Crossing crossingType;
	
	float distance;
	float playerSpeed;
	bool isLocked;

	void Awake()
	{
		isLocked = false;
	}

	void OnTriggerEnter()
	{
		// change trigger's rotation to player's rotation (same directions)
		Quaternion playerRot;
		playerRot = PlayerController.Instance.gameObject.transform.rotation; 
		transform.rotation = playerRot;

		// check a choice of way
		checkPlayerDirections();

		switch (crossingType) 
		{
			case Crossing.oneWay:
			if(PlayerController.Instance.rightArrow)
				PlayerController.Instance.angle = 90;
			else if(PlayerController.Instance.leftArrow)
				PlayerController.Instance.angle = -90;
			break;
			case Crossing.moreWays:
				// signal player is on crossing
				PlayerController.Instance.chanceToChoice = true;
				// slow the player
				PlayerController.Instance.slowDownPlayer ();

			break;
		}
	}
	
	void OnTriggerStay(Collider targetColl)
	{
		// inside of crossing change direction and move 
		if(targetColl.transform == PlayerController.Instance.gameObject.transform)
		{
			Vector3 playerPos;
			playerPos = PlayerController.Instance.gameObject.transform.position;
			distance = Vector3.Distance(transform.position, playerPos);
			distance = Mathf.Abs(distance);
			// when player is on middle of crossing
			if(!isLocked && distance >= 0 && distance < 0.1f)
			{
				isLocked = true;
				switch(crossingType)
				{
				case Crossing.oneWay:
					PlayerController.Instance.DoSomethingFun(transform.position);
					break;
				case Crossing.moreWays:
					// if player didnt choose clear chance to choice
					PlayerController.Instance.chanceToChoice = false;
					// go exacly on the middle and set way to move (rotate and push)
					PlayerController.Instance.DoSomethingFun(transform.position);
					PlayerController.Instance.acceleratePlayer();
					break;
				}
			}
		}
	}
	
	void OnTriggerExit()
	{
		isLocked = false;

		// reset player directions and angle before next crossing
		PlayerController.Instance.resetDirections();
		PlayerController.Instance.resetAngle ();
	}

	private void checkPlayerDirections()
	{
		Ray rayLeft, rayRight, rayUp, rayDown;
		rayLeft = new Ray(transform.position,-transform.right);    // leftArrow 
		rayRight = new Ray(transform.position,transform.right);    // rightArrow
		rayUp = new Ray(transform.position,transform.forward);    // upArrow
		rayDown = new Ray(transform.position,-transform.forward);    // downArrow
		
		if (!Physics.Raycast (rayLeft, 2)) 
		{
			PlayerController.Instance.leftArrow = true;
		}
		if (!Physics.Raycast (rayRight, 2)) 
		{
			PlayerController.Instance.rightArrow = true;
		}
		if (!Physics.Raycast (rayUp, 2)) 
		{
			PlayerController.Instance.upArrow = true;
		} 
	}
}
