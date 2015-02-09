using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum GameState
{
	Menu,
	Settings,
	Scores,
	Levels,
	Start,
	Run,
	Pause,
	Map,
	EndLost,
	EndWon
}


public class GameManager : MonoBehaviour 
{

	public GameState state;
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

	public int MazeWidth;
	public int MazeHeight;
	public MazeGenerator Maze;
	public Labyrinth lab;

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

	public Camera playerCamera;
	public Camera mapCamera;

	public Animator cloudsAnimator;

	void Awake() 
	{
		if(Instance != null && Instance != this) // && Instance != this
		{
			Destroy(gameObject);
			Debug.Log ("Destroy duplicate!");
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
		gameObject.name = gameObject.name + "Instance";

		Debug.Log ("GameManager.Awake()");
		SetReferences ();
	}

	void Start () 
	{
		player = GameObject.FindWithTag("Player").GetComponent<PlayerMecanimController>();
		if(cloudsAnimator == null) GameObject.Find ("Clouds").GetComponent<Animator>();

		ChangeGameState(GameState.Menu);

		Debug.Log ("GameManager.Start()");
	}

	void OnLevelWasLoaded(int lvl) {


		Debug.Log ("OnSceneWasLoaded:" + lvl);
	}

	void Update()
	{
		if(state == GameState.Run && player.isAlive == true)
		{
			gameTimer += Time.deltaTime;
		}

		if(Input.GetKey(KeyCode.Escape)) {

			AddLevel();
			RestartGame();
		}
	}

	public void SetReferences()
	{
		// menu references
		CurrentMenu = GameObject.FindGameObjectWithTag ("MainMenu").GetComponent<NewMenu> ();
		MainMenu = GameObject.FindGameObjectWithTag ("MainMenu").GetComponent<NewMenu> ();
		HUD = GameObject.FindGameObjectWithTag ("HUD").GetComponent<NewMenu> ();
		PauseMenu = GameObject.FindGameObjectWithTag ("PauseMenu").GetComponent<NewMenu> ();
		EmptyMenu = GameObject.FindGameObjectWithTag ("EmptyMenu").GetComponent<NewMenu> ();
		GameOverMenu = GameObject.FindGameObjectWithTag ("GameOverMenu").GetComponent<NewMenu> ();
		WonMenu = GameObject.FindGameObjectWithTag ("WonMenu").GetComponent<NewMenu> ();
		SettingsMenu = GameObject.FindGameObjectWithTag ("SettingsMenu").GetComponent<NewMenu> ();
		ScoresMenu = GameObject.FindGameObjectWithTag ("ScoresMenu").GetComponent<NewMenu> ();
		LevelsMenu = GameObject.FindGameObjectWithTag ("LevelsMenu").GetComponent<NewMenu> ();
		
		// ui reference
		UI =  GameObject.FindGameObjectWithTag ("UI").GetComponent<UIGameHUD> ();
		
		// clouds animator reference
		cloudsAnimator =  GameObject.FindGameObjectWithTag ("Clouds").GetComponent<Animator> ();
		
		// camera's references
		playerCamera = Camera.main.camera;
		mapCamera = GameObject.FindGameObjectWithTag ("MapCamera").camera;

		// player
		player = GameObject.FindWithTag("Player").GetComponent<PlayerMecanimController>();

		Debug.Log ("Set References");
	}

	void RebuildLabyrinth(int width, int height)
	{
		// change dimensions and positions of cameras
		MazeWidth = Maze.Width = width;
		MazeHeight = Maze.Height = height;
		PlayerCamera.Instance.UnPinCamereaFromPlayer ();
		lab.SetCamerasAtStart();
		// clear
		GameObject walls = GameObject.Find("_Walls"); DestroyImmediate(walls);
		GameObject objects = GameObject.Find ("_Objects"); DestroyImmediate(objects);
		GameObject triggers = GameObject.Find("_Triggers"); DestroyImmediate(triggers);
		// build
		lab.CreateMaze();
		lab.BuildWalls();
		lab.CreateGameObjects();
	}

	#region GamePlay Functions

	private void NewGame() 
	{

	}

	private void RestartGame() 
	{
		Debug.Log ("##### RESTART GAME #####");
		ChangeGameState(GameState.Start);
		Application.LoadLevel(Application.loadedLevelName);
	}

	private void PlayerStart() 
	{ 
		player.StartPlayer (); 
	}

	private void PlayerCameraStart() 
	{ 
		PlayerCamera.Instance.StartCamera (); 
	}

	private void GameStateRun()
	{ 
		ChangeGameState (GameState.Run); 
	}

	private void PlayerUnpause()
	{ 
		player.UnpauseAnimations (); 
	}
	
	public void ToggleCameras()
	{
		playerCamera.enabled = !playerCamera.enabled;
		mapCamera.enabled = !mapCamera.enabled;
	}

	public void AddLevel()
	{
		level++;
	}

	void CheckLvlAndRebuild()
	{
		switch (level) 
		{
		case 1:		// LVL 2 7x7
			RebuildLabyrinth(7,7);
			break;
		case 2:		// LVL 3 8x8
			RebuildLabyrinth(8,8);
			break;
		case 3:		// LVL 4 9x9
			RebuildLabyrinth(9,9);
			break;
		case 4:		// LVL 5 10x10
			RebuildLabyrinth(10,10);
			break;
		case 5:
			ChangeGameState(GameState.Menu);
			break;
		}
	}

	/*public void PlayNextLevel()
	{
		level++;
		if(level > 1 && level <=5)
		{
			MazeWidth++;
			MazeHeight++;
			PlayerCamera.Instance.UnPinCamereaFromPlayer();
			Application.LoadLevel(Application.loadedLevel);
			SetReferences();
			player.ResetPlayer();
			player.ResetAnimations();
			ChangeGameState(GameState.Start);
		}
		SetReferences ();
	}*/

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
			CheckLvlAndRebuild();

			ShowMenu (EmptyMenu);
			UI.SetGameLevel(level + 1);
			cloudsAnimator.SetTrigger("Start");
			if(player != null)
			{
				player.ResetPlayer ();
				player.ResetAnimations();
				player.SetRotation();
				Debug.Log("Player has been reseted");
			}
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
			RebuildLabyrinth(6,6);
			level = 0;
			player.ResetPlayer ();
			player.ResetAnimations();
			PlayerCamera.Instance.ResetCamera();
			break;

		case GameState.EndLost:
			ShowMenu(GameOverMenu);
			break;

		case GameState.EndWon:
			PlayerCamera.Instance.UnPinCamereaFromPlayer();
			ShowMenu(WonMenu);
			UI.ShowEndTime(gameTimer);
			UI.SetGameLevel(level + 1);
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

		case GameState.Levels:
			ShowMenu(LevelsMenu);
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
