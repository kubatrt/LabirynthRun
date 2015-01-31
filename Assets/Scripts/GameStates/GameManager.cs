using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum GameState
{
	Menu,
	Settings,
	Scores,
	Start,
	Run,
	Pause,
	Map,
	EndLost,
	EndWon
}


public class GameManager : MonoBehaviour 
{

	public GameState state = GameState.Menu;
	// Singleton with GameObject instance
	static GameManager instance;
	public static GameManager Instance { 
		get; 
		private set;
	}

	PlayerMecanimController player;
	string playerName = "";
	public string PlayerName {
		get { return playerName.Equals("") ? "Player" : playerName; }
		set { playerName = value; }
	}

	public UIGameHUD UI;

	float gameStartupTimer;
	float gameTimeElapsed;
	public float gameTimer;

	public int level;
	int previousLevel;

	public NewMenu CurrentMenu;
	public NewMenu MainMenu;
	public NewMenu HUD;
	public NewMenu PauseMenu;
	public NewMenu EmptyMenu;
	public NewMenu GameOverMenu;
	public NewMenu WonMenu;
	public NewMenu SettingsMenu;
	public NewMenu ScoresMenu;

	public Camera playerCamera;
	public Camera mapCamera;

	public Animator cloudsAnimator;


	void Awake() 
	{
		if(Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		Instance = this;
		//DontDestroyOnLoad(gameObject);
		Debug.Log ("GameManager.Awake()");
	}

	void Start () 
	{
		Debug.Log ("GameManager.Start()");
		player = GameObject.FindWithTag("Player").GetComponent<PlayerMecanimController>();
		ChangeGameState (GameState.Menu);
		level = 1;
	}

	void Update()
	{
		if(state == GameState.Run && player.isAlive == true)
		{
			gameTimer += Time.deltaTime;
		}
	}

	#region GamePlay Functions
	private void PlayerStart() { player.StartPlayer (); }
	private void PlayerCameraStart() { PlayerCamera.Instance.StartCamera (); }
	private void GameStateRun(){ ChangeGameState (GameState.Run); }
	private void PlayerUnpause(){ player.UnpauseAnimations (); }
	
	public void ToggleCameras()
	{
		playerCamera.enabled = !playerCamera.enabled;
		mapCamera.enabled = !mapCamera.enabled;
	}

	public void AddLevel() { level++; }
	#endregion

	void ShowMenu(NewMenu menu)
	{
		if(CurrentMenu != null)
			CurrentMenu.IsOpen = false;
		
		CurrentMenu = menu;
		CurrentMenu.IsOpen = true;
	}

	// game state manager
	public void ChangeGameState(GameState gameState)
	{
		state = gameState;

		switch(state)
		{
		case GameState.Start:
			/* creating level */
			if(level != previousLevel)
			{
				// generate new maze
				switch(level)
				{
				case 1: 
					// maze dimensions 6x6 
					break;
				case 2:
					// maze dimensions 7x7 
					break;
				case 3: 
					break;
					// maze dimensions 7x8 
				case 4:
					break;
					// maze dimensions 8x7 
				case 5: 
					break;
					// maze dimensions 8x8 
				}
			}
			previousLevel = level;

			ShowMenu (EmptyMenu);
			UI.SetGameLevel(level);
			cloudsAnimator.SetTrigger("Start");
			player.ResetPlayer ();
			player.ResetAnimations();
			PlayerCamera.Instance.ResetCamera();
			gameTimer = 0;
			Invoke ("PlayerCameraStart", 3);
			Invoke ("PlayerStart", 6);
			Invoke ("GameStateRun", 6); 
			break; 

		case GameState.Run:
			ShowMenu (HUD);
			UI.SetPlayerName(PlayerName);
			Debug.Log("Set Player Name");
			player.UnpauseAnimations ();
			break;

		case GameState.Pause:
			ShowMenu (PauseMenu);
			player.PauseAnimations ();
			break;

		case GameState.Menu:
			ShowMenu (MainMenu);
			player.ResetPlayer ();
			player.ResetAnimations();
			PlayerCamera.Instance.ResetCamera();
			break;

		case GameState.EndLost:
			ShowMenu(GameOverMenu);
			break;

		case GameState.EndWon:
			ShowMenu(WonMenu);
			UI.ShowEndTime(gameTimer);
			break;

		case GameState.Map:
			if(player.maps > 0)
			{
				ShowMenu (EmptyMenu);
				player.decreaseMaps();
				ToggleCameras ();
				player.PauseAnimations ();
				Invoke ("PlayerUnpause",3f);
				Invoke ("ToggleCameras", 3f);
				Invoke ("GameStateRun", 3f);
			}
			else
				ChangeGameState(GameState.Run);
			break; 

		case GameState.Settings:
			ShowMenu(SettingsMenu);
			break;

		case GameState.Scores:
			ShowMenu(ScoresMenu);
			break;
		}
	}
}
