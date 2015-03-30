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
	// Singleton with GameObject instance
	public static GameManager Instance { 
		get; 
		private set;
	}

	public GameState state;

	public UIGameHUD UI;
	public PlayerMecanimController player;
	public PlayerCamera playerCamera;
	public MapCamera mapCamera;
	public Labyrinth lab;
	public Animator cloudsAnimator;


	public string PlayerName = "defaultPlayer";

	public float gameTimer;
	public int level;
    public int maxLevel = 5;
    public int score;

	float gameStartupTimer;
	float gameTimeElapsed;
	float MapViewTime = 3;


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
		//SetReferences ();

		ChangeGameState(GameState.Menu);
        score = 0;
        level = 0;

		Debug.Log ("GameManager.Start()");
	}

	void OnLevelWasLoaded(int lvl) 
	{
		SetReferences();
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

        if(Input.GetKey(KeyCode.Space))
        {
            lab.ClearMaze();
        }
	}

	public void SetReferences()
	{
		UI =  GameObject.FindGameObjectWithTag ("UI").GetComponent<UIGameHUD> ();
		if(cloudsAnimator == null) cloudsAnimator =  GameObject.FindGameObjectWithTag ("Clouds").GetComponent<Animator> ();
		if(playerCamera == null) playerCamera = GameObject.FindObjectOfType<PlayerCamera>();
		if(mapCamera == null) mapCamera = GameObject.FindObjectOfType<MapCamera>();
		if(player == null) player = GameObject.FindWithTag("Player").GetComponent<PlayerMecanimController>();
		Debug.Log ("Set References");
	}

	void AdjustCamerasAtStart()
	{
		// set cameras at start position
		float x = (((float)(lab.Width)/2-1)*4)+2;
		float z = (((float)(lab.Height)/2-1)*4)-2;
		playerCamera.SetStartUpPosition(x,x*4,z);
		playerCamera.SetPosition(x,x*4,z);
		mapCamera.SetPosition (x, 5, z);
		mapCamera.SetCameraSize (((lab.Width + lab.Height)/2)*4);
		Debug.Log ("SetCamerasAtStart: " + lab.Width + lab.Height);
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
        AdjustCamerasAtStart();
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
		Application.LoadLevel(Application.loadedLevelName); // <-- realoding scene?
	}

    public void RunGame()
    {
        ChangeGameState(GameState.Run);
        player.StartPlayer();
    }

    public void ToggleCameras()
    {
        playerCamera.camera.enabled = !playerCamera.camera.enabled;
        mapCamera.camera.enabled = !mapCamera.camera.enabled;
        Debug.Log("ToogleCameras()");
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
        int var;

	    switch (level)
	    {
	        case 1:		// LVL 1
                var = Random.Range(0, 2);
                if(var == 0)
	                RebuildLabyrinth("level_tut_01-1.maze");
                if(var == 1)
                    RebuildLabyrinth("level_tut_01-2.maze");
	            break;
	        case 2:		// LVL 2
                var = Random.Range(0, 2);
                if(var == 0)
				    RebuildLabyrinth("level_tut_02-1.maze");
                if (var == 1)
                    RebuildLabyrinth("level_tut_02-2.maze");
	            break;
	        case 3:		// LVL 3
                var = Random.Range(0, 2);
                if(var == 0)
				    RebuildLabyrinth("level_tut_03-1.maze");
                if (var == 1)
                    RebuildLabyrinth("level_tut_03-2.maze");
				break;
			case 4:		// LVL 4
				RebuildLabyrinth("level_test_04.maze");
				break;
			case 5:		// LVL 5
				RebuildLabyrinth("level_test_05.maze");
				break;
			case 6:		// LVL 6
				RebuildLabyrinth("level_test_06.maze");
				break;
			case 7:		// LVL 7
				RebuildLabyrinth("level_test_07.maze");
				break;
			default:
				Debug.Log ("Invalid level: " + level);
				break;
	    }
		Debug.Log ("CheckLvlAndRebuild: " + level);
	}
	#endregion

	private void GameStateRun()	// same as RunGame
	{
		ChangeGameState(GameState.Run);
	}
	
	private void PlayerCameraStart() 
	{ 
		playerCamera.StartCamera (); 
	}
	
	private void PlayerUnpause()
	{ 
		player.UnpauseAnimations (); 
	}
	
	// game state manager
	public void ChangeGameState(GameState gameState)
	{
		state = gameState;

		switch(state)
		{
		case GameState.Start:
			CheckLvlAndRebuild();
			AdjustCamerasAtStart();

			UI.UIStartState();
			cloudsAnimator.SetTrigger("Start");

			player.ResetPlayer (); 
			player.SetStartupRotation(lab.GetStartCellRotation());
			player.ResetAnimations();

			playerCamera.RestartCamera ();
			playerCamera.ResetCameraTransform();

			Debug.Log("Player has been reseted");


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
            RebuildLabyrinth("level_tut_01-1.maze");
			level = 0;
			player.ResetPlayer ();
			player.ResetAnimations();

			playerCamera.RestartCamera ();
			playerCamera.ResetCameraTransform();
			break;

		case GameState.EndLost:
            UI.UIEndLostState();
			break;

		case GameState.EndWon:
            SetScoreAtEnd();
            UI.UIEndWonState();
			playerCamera.LevelEndCameraAnimation();

			break;

		case GameState.Map:
			if(player.mapsUses > 0)
			{
                UI.UIMapState();
				player.decreaseMaps();
				ToggleCameras ();
                Debug.Log("tooglecamera");
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
