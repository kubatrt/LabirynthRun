using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIGameHUD : MonoBehaviour 
{
	public Text timeText;
	public Text failuresText; 
	public Text playerLivesText;
	public Text endTimeText;
	public Text enterPlayerNameText;
	public Text playerNameText;
	public Text gameLevelText;

	PlayerMecanimController player;

	void Start () 
	{
		player = GameObject.FindWithTag("Player").GetComponent<PlayerMecanimController>();
	}
	
	void Update () 
	{
		if(player == null)
			return;

		failuresText.text = player.failures.ToString();
		playerLivesText.text = player.lives.ToString();
		timeText.text = string.Format(" {0:F2} ", GameManager.Instance.gameTimer);
	}

	public void ShowEndTime(float time)
	{
		endTimeText.text = string.Format(" {0:F2} ", time);
	}

	public void SetPlayerName(string PlayerName)
	{
		PlayerName = enterPlayerNameText.text;
		playerNameText.text = PlayerName;
	}

	public void SetGameLevel(int level)
	{
		gameLevelText.text = level.ToString ();
	}

	#region UI controls
	public void OnClickPlayButton()
	{
		GameManager.Instance.ChangeGameState (GameState.Start);
	}

	public void OnClickMapButton()
	{
		GameManager.Instance.ChangeGameState (GameState.Map);
	}

	public void OnClickPauseButton()
	{
		GameManager.Instance.ChangeGameState (GameState.Pause);
	}

	public void OnClickResumeButton()
	{
		GameManager.Instance.ChangeGameState (GameState.Run);
	}

	public void OnClickPlayAgainButton()
	{
		GameManager.Instance.ChangeGameState (GameState.Start);
	}

	public void OnClickPlayNextButton()
	{
		GameManager.Instance.AddLevel ();
		GameManager.Instance.ChangeGameState (GameState.Start);
	}

	public void OnClickBackButton()
	{
		GameManager.Instance.ChangeGameState (GameState.Menu);
	}

	public void OnClickScoresButton()
	{
		GameManager.Instance.ChangeGameState (GameState.Scores);
	}

	public void OnClickSettingsButton()
	{
		GameManager.Instance.ChangeGameState (GameState.Settings);
	}

	public void OnClickQuitButton()
	{
		Application.LoadLevel (Application.loadedLevel);
	}
	#endregion
}
