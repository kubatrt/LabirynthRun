using UnityEngine;
using System.Collections;

public class UIGameHUD : MonoBehaviour 
{
	public UnityEngine.UI.Text timeText;


	PlayerMecanimController player;


	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindWithTag("Player").GetComponent<PlayerMecanimController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(player == null)
			return;

		timeText.text = player.gameTimer.ToString();
	}
}
