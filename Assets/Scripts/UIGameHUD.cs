using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIGameHUD : MonoBehaviour 
{
	public Text playerText;
	public Text timeText;
	public Text failuresText; 

	PlayerMecanimController player;


	void Start () 
	{
		player = GameObject.FindWithTag("Player").GetComponent<PlayerMecanimController>();
		playerText.text = GameManager.Instance.PlayerName;
	}
	
	void Update () 
	{
		if(player == null)
			return;

		failuresText.text = player.failures.ToString();
		timeText.text = string.Format(" {0:F2} ", player.gameTimer);
	}

	#region UI controls

	public void OnClickPlayButton()
	{
		player.StartPlayer ();
	}

	public void OnClickQuitButton()
	{
	//	player.StopPlayer ();
		Application.LoadLevel (Application.loadedLevel);
	}

	public void OnClickMapButton()
	{

	}

	public void OnClickRestartButton()
	{

	}

	public void OnClickPauseButton()
	{

	}

	#endregion
}
