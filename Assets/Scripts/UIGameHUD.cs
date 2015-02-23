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

    public NewMenu CurrentMenu;
    public NewMenu MainMenu;
    public NewMenu HUD;
    public NewMenu PauseMenu;
    public NewMenu EmptyMenu;
    public NewMenu GameOverMenu;
    public NewMenu WonMenu;
    public NewMenu SettingsMenu;
    public NewMenu ScoresMenu;
    public NewMenu LevelsMenu;

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

    void ShowMenu(NewMenu menu)
    {
        if (CurrentMenu != null)
            CurrentMenu.IsOpen = false;

        CurrentMenu = menu;
        CurrentMenu.IsOpen = true;
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
	}

	public void OnClickBackButton()
	{
		GameManager.Instance.ChangeGameState (GameState.Menu);
	}

	public void OnClickLevelsButton()
	{
		GameManager.Instance.ChangeGameState (GameState.Levels);
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
        Application.Quit();
	}

	public void OnClickLvlButton(int level)
	{
		GameManager.Instance.level = level - 2;
		OnClickPlayNextButton ();
	}
	#endregion
    
    #region UIGameState
    public void UIStartState()
    {
        ShowMenu(EmptyMenu);
        SetGameLevel(GameManager.Instance.level + 1);   
    }

    public void UIRunState()
    {
        ShowMenu(HUD);
        SetPlayerName(GameManager.Instance.PlayerName);
    }

    public void UIPauseState()
    {
        ShowMenu(PauseMenu);
    }

    public void UIMenuState()
    {
        ShowMenu(MainMenu);
    }

    public void UIEndLostState()
    {
        ShowMenu(GameOverMenu);
    }

    public void UIEndWonState()
    {
        ShowMenu(WonMenu);
        ShowEndTime(GameManager.Instance.gameTimer);
        SetGameLevel(GameManager.Instance.level + 1);
    }

    public void UIMapState()
    {
        ShowMenu(EmptyMenu);
    }

    public void UILevelsState()
    {
        ShowMenu(LevelsMenu);
    }

    public void UISettingsState()
    {
        ShowMenu(SettingsMenu);
    }

    public void UIScoresState()
    {
        ShowMenu(ScoresMenu);
    }
    #endregion
}
