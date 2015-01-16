using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIGameHUD : MonoBehaviour 
{
	public Camera playerCamera;
	public Camera mapCamera;

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
	#region GamePlay Functions
	private void PlayerStart() { player.StartPlayer (); }
	private void PlayerCameraStart() { PlayerCamera.Instance.StartCamera (); }
	private void GameStateRun(){ GameManager.Instance.ChangeGameState (GameState.Run); }

	public void ToggleCameras()
	{
		playerCamera.enabled = !playerCamera.enabled;
		mapCamera.enabled = !mapCamera.enabled;
	}
	#endregion

	#region UI controls

	public void OnClickPlayButton()
	{
		GameManager.Instance.ChangeGameState (GameState.Start);
		Invoke ("PlayerCameraStart", 3);
		Invoke ("PlayerStart", 6);
		Invoke ("GameStateRun", 6);
	}

	public void OnClickMapButton()
	{
		ToggleCameras ();
		GameManager.Instance.ChangeGameState (GameState.Map);
		player.PauseAnimations ();
		Invoke ("OnClickResumeButton",3f);
		Invoke ("ToggleCameras", 3f);
	}

	public void OnClickPauseButton()
	{
		GameManager.Instance.ChangeGameState (GameState.Pause);
		player.PauseAnimations ();
	}

	public void OnClickResumeButton()
	{
		GameManager.Instance.ChangeGameState (GameState.Run);
		player.UnpauseAnimations ();
	}

	public void OnClickRestartButton()
	{
		GameManager.Instance.ChangeGameState (GameState.Menu);
		Application.LoadLevel (Application.loadedLevel);
	}

	public void OnClickQuitButton()
	{
		Application.LoadLevel (Application.loadedLevel);
	}





	#endregion
}
