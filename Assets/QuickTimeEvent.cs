using UnityEngine;
using System.Collections;



public class QuickTimeEvent : MonoBehaviour 
{
	public float startTime;
	public float reactionTime;

	public MoveDirections directions;

	PlayerMecanimController player;
	public bool noChoice = true;

	void Awake() 
	{
		player = GetComponent <PlayerMecanimController>();
	}

	void Start() 
	{

	}
	
	void Update () 
	{
		reactionTime += Time.deltaTime;
	}

	void UIArrowLeft()
	{
		player.GoLeft();
		noChoice = false;
	}
	
	void UIArrowRight()
	{
		player.GoRight();
		noChoice = false;
	}
	
	void UIArrowForward()
	{
		player.GoForward();
		noChoice = false;
	}
}
