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
	static GameManager instance = null;
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
    float MapViewTime = 3;

	public int level;
	public int previousLevel;
    int maxLevel = 5;
    public int score;

	public MazeGenerator Maze;
	public Labyrinth lab;

	public Camera playerCamera;
	public Camera mapCamera;

	public Animator cloudsAnimator;

	void Awake() 
	{
		if(Instance != null && Instance != this) // && Instance != this
		{
			Debug.Log ("Destroy duplicate!");
			Destroy(gameObject);

		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
		Debug.Log ("GameManager.Awake()");
	}

	void Start () 
	{
		SetReferences ();

		ChangeGameState(GameState.Menu);
        score = 0;
        level = previousLevel = 0;

		Debug.Log ("GameManager.Start()");
	}

	void OnLevelWasLoaded(int lvl) 
	{
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
		UI =  GameObject.FindGameObjectWithTag ("UI").GetComponent<UIGameHUD> ();
		cloudsAnimator =  GameObject.FindGameObjectWithTag ("Clouds").GetComponent<Animator> ();
		playerCamera = Camera.main.camera;
		mapCamera = GameObject.FindGameObjectWithTag ("MapCamera").camera;
		player = GameObject.FindWithTag("Player").GetComponent<PlayerMecanimController>();
		Debug.Log ("Set References");
	}

	void RebuildLabyrinth(string name)
	{
		Debug.Log ("GameManager.RebuildLabyrinth");
		// change dimensions and positions of cameras

		// clear
		lab.ClearMaze();
		// build
		lab.CreateMaze(name);
		//lab.CreatePlayer();
		lab.BuildWalls();
		lab.CreateGameObjects();
		lab.CreateGround();

		lab.SetCamerasAtStart();
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

    public void RunGame()
    {
        ChangeGameState(GameState.Run);
        player.StartPlayer();
    }

    private void GameStateRun()
    {
        ChangeGameState(GameState.Run);
    }

	private void PlayerCameraStart() 
	{ 
		PlayerCamera.Instance.StartCamera (); 
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
                    RebuildLabyrinth("level_test.maze");
                    break;
                case 2:		// LVL 3 8x8
                    //RebuildLabyrinth(8, 8);
                    break;
                case 3:		// LVL 4 9x9
                    //RebuildLabyrinth(9, 9);
                    break;
                case 4:		// LVL 5 10x10
                    //RebuildLabyrinth(10, 10);
                    break;
				case 5:		// LVL 5 10x10
					//RebuildLabyrinth(10, 10);
					break;
				case 6:		// LVL 5 10x10
					//RebuildLabyrinth(10, 10);
					break;
				case 7:		// LVL 5 10x10
					//RebuildLabyrinth(10, 10);
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
			Invoke ("PlayerCameraStart", MapViewTime);
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
			//RebuildLabyrinth("level_88_01.maze"); nie działa
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
			if(player.mapsUses > 0)
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
