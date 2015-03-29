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
    public Text gameLevelTextEnd;
    public Text gameScoreTextEnd;

    public NewMenu CurrentMenu;
    NewMenu PreviousMenu;
    public NewMenu MainMenu;
    public NewMenu HUD;
    public NewMenu PauseMenu;
    public NewMenu EmptyMenu;
    public NewMenu GameOverMenu;
    public NewMenu WonMenu;
    public NewMenu SettingsMenu;
    public NewMenu ScoresMenu;
    public NewMenu LevelsMenu;

    bool SpeedUpIsPushed;
    float timer;

	PlayerMecanimController player;
	PlayerCamera playerCamera;

	void Start () 
	{
		player = GameObject.FindWithTag("Player").GetComponent<PlayerMecanimController>();
		playerCamera = GameObject.FindObjectOfType<PlayerCamera>();

        SpeedUpIsPushed = false;
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
        {
            CurrentMenu.IsOpen = false;
            PreviousMenu = CurrentMenu;
        }

        CurrentMenu = menu;
        CurrentMenu.IsOpen = true;
    }

    void ShowPreviousMenu()
    {
        if(CurrentMenu != null)
        {
            CurrentMenu.IsOpen = false;
        }
        CurrentMenu = PreviousMenu;
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

	public void SetTextNumber(Text text, int number)
	{
		//gameLevelText.text = level.ToString ();
        text.text = number.ToString();
	}

	#region UI controls
	public void OnClickPlayButton()
	{
		GameManager.Instance.level = 1;
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

	public void OnClickPlayNextButton()
	{
		GameManager.Instance.AddLevel ();
	}

	public void OnClickBackButton()
	{
        ShowPreviousMenu();
	}

    public void OnClickBackToMenuButton()
    {
        GameManager.Instance.ChangeGameState(GameState.Menu);
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
		GameManager.Instance.level = level;
		//OnClickPlayNextButton ();
        GameManager.Instance.previousLevel = GameManager.Instance.level;
		GameManager.Instance.ChangeGameState (GameState.Start);
	}

	public void OnClickJumpButton()
	{
		player.SetJumpAnim ();
	}

	public void OnClickRollButton()
	{
		player.SetRollAnim();
	}

    public void PointerDownSpeedButton()
    {
        SpeedUpIsPushed = true;
        StartCoroutine(DelaySpeedUp(0.25f));
		playerCamera.SpeedUpFov();
    }

    public void PointerUpSpeedButton()
    {
        SpeedUpIsPushed = false;
        timer = 0;
        player.SpeedUpOff();
		playerCamera.NormalizeFov();
    }
	#endregion
    
    #region UIGameState
    public void UIStartState()
    {
        ShowMenu(EmptyMenu);
        SetTextNumber(gameLevelText, GameManager.Instance.level + 1);   
    }

    public void UIRunState()
    {
        ShowMenu(HUD);
        SetPlayerName(GameManager.Instance.player.name);
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
        SetTextNumber(gameLevelTextEnd, GameManager.Instance.level);
        SetTextNumber(gameScoreTextEnd, GameManager.Instance.score);
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

    IEnumerator DelaySpeedUp(float time)
    {
        while (SpeedUpIsPushed)
        {
            timer += Time.deltaTime;
            if(timer > time)
            {
                player.SpeedUpOn();
            }
            yield return null;
        }
    }
}
