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
		player.StartPlayerGame ();
		GameManager.Instance.ChangeGameState (GameState.Start);
		Invoke ("Run", 6);
	}

	private void Run(){ GameManager.Instance.ChangeGameState (GameState.Run); }

	public void OnClickResumeButton()
	{
		GameManager.Instance.ChangeGameState (GameState.Run);
		player.SetMovingAnim ();
	}

	public void OnClickQuitButton()
	{
	//	player.StopPlayer ();
		Application.LoadLevel (Application.loadedLevel);
	}

	public void OnClickMapButton()
	{
		GameManager.Instance.ChangeGameState (GameState.Pause);
	}

	public void OnClickRestartButton()
	{
		GameManager.Instance.ChangeGameState (GameState.Menu);
		Application.LoadLevel (Application.loadedLevel);
	}

	public void OnClickPauseButton()
	{
		GameManager.Instance.ChangeGameState (GameState.Pause);
		player.ResetAnimations ();
	}

	#endregion
}
