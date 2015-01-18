using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIGameHUD : MonoBehaviour 
{
	public Text playerText;
	public Text timeText;
	public Text failuresText; 
	public Text playerLives;

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
		playerLives.text = player.lives.ToString();
		timeText.text = string.Format(" {0:F2} ", GameManager.Instance.gameTimer);
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

	public void OnClickRestartButton()
	{
		Application.LoadLevel (Application.loadedLevel);
	}

	public void OnClickQuitButton()
	{
		Application.LoadLevel (Application.loadedLevel);
	}
	#endregion
}
