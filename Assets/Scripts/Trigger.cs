using UnityEngine;
using System.Collections;

public enum TriggerCrossing
{
	OneWay,
	MoreWays
};

public sealed class MoveDirections
{
	public bool Left 	{get; set; }
	public bool Right 	{get; set; }
	public bool Forward {get; set; }
	public bool Back 	{get; set; }
	
	public MoveDirections()
	{
		Left = false;
		Right = false;
		Forward = false;
		Back = false;
	}
};