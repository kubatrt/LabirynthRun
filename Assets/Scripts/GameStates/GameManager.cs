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
	public int previousLevel;
    int maxLevel = 5;
    public int score;

	public int MazeWidth;
	public int MazeHeight;
	public MazeGenerator Maze;
	public Labyrinth lab;

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
        score = 0;
        level = previousLevel = 0;

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
		Debug.Log ("RebuildLabyrinth");
		// change dimensions and positions of cameras
		MazeWidth = Maze.Width = width;
		MazeHeight = Maze.Height = height;
		lab.SetCamerasAtStart();
		// clear
		lab.ClearMaze();
		// build
		lab.CreateMaze();
		lab.BuildWalls();
		lab.CreateGameObjects();
		lab.CreateGround();
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

    public void AddScore(int number)
    {
        score += number;
    }

    void SetScoreAtEnd()
    {
        score += (int)(1000 / gameTimer);
    }

	public void AddLevel()
	{
        previousLevel = level;
		level++;
        if(level < maxLevel)
        {
            ChangeGameState(GameState.Start);
        }
        else
        {
            ChangeGameState(GameState.Menu);
        }
	}

	void CheckLvlAndRebuild()
	{
        if (previousLevel != level)
        {
            switch (level)
            {
                case 1:		// LVL 2 7x7
                    RebuildLabyrinth(7, 7);
                    break;
                case 2:		// LVL 3 8x8
                    RebuildLabyrinth(8, 8);
                    break;
                case 3:		// LVL 4 9x9
                    RebuildLabyrinth(9, 9);
                    break;
                case 4:		// LVL 5 10x10
                    RebuildLabyrinth(10, 10);
                    break;
            }
        }
	}
	#endregion

	// game state manager
	public void ChangeGameState(GameState gameState)
	{
		state = gameState;

		switch(state)
		{
		case GameState.Start:
			CheckLvlAndRebuild();

            UI.UIStartState();
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
            score = 0;
			Invoke ("PlayerCameraStart", 3);
			Invoke ("PlayerStart", 6);
			Invoke ("GameStateRun", 6); 
			break; 

		case GameState.Run:
            UI.UIRunState();
			player.UnpauseAnimations ();
			break;

		case GameState.Pause:
            UI.UIPauseState();
			player.PauseAnimations ();
			break;

		case GameState.Menu:
            UI.UIMenuState();
			RebuildLabyrinth(6,6);
			level = 0;
			player.ResetPlayer ();
			player.ResetAnimations();
			PlayerCamera.Instance.ResetCamera();
			break;

		case GameState.EndLost:
            UI.UIEndLostState();
			break;

		case GameState.EndWon:
            SetScoreAtEnd();
            UI.UIEndWonState();
            PlayerCamera.Instance.LevelEndCameraAnimation();

			break;

		case GameState.Map:
			if(player.maps > 0)
			{
                UI.UIMapState();
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
            UI.UILevelsState();
			break;

		case GameState.Settings:
            UI.UISettingsState();
			break;

		case GameState.Scores:
            UI.UIScoresState();
			break;
		}
	}
}
