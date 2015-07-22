using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour 
{
	public int failures;
	public int lives;
	public int mapsUses;
	public int score;

	void Start () 
	{
		lives = 3;
		mapsUses = 3;
		failures = 0;
		score = 0;
	}
}
