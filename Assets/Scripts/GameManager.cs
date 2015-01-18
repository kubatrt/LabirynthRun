using UnityEngine;
using System.Collections;

public enum GameState
{
	Menu,
	Start,
	Run,
	End,
	Pause,
	Map
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

	float gameStartupTimer;
	float gameTimeElapsed;
	public float gameTimer;

	public NewMenu MainMenu;
	public NewMenu HUD;
	public NewMenu PauseMenu;
	public NewMenu EmptyMenu;
	public NewMenu GameOverMenu;

	public Camera playerCamera;
	public Camera mapCamera;


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
	#endregion

	public void ChangeGameState(GameState gameState)
	{
		state = gameState;

		switch(state)
		{
		case GameState.Start:
			MenuManager.Instance.ShowMenu (EmptyMenu);
			player.ResetPlayer ();
			PlayerCamera.Instance.ResetCamera();
			gameTimer = 0;
			Invoke ("PlayerCameraStart", 3);
			Invoke ("PlayerStart", 6);
			Invoke ("GameStateRun", 6); 
			break; 

		case GameState.Run:
			MenuManager.Instance.ShowMenu (HUD);
			player.UnpauseAnimations ();
			break;

		case GameState.Pause:
			MenuManager.Instance.ShowMenu (PauseMenu);
			player.PauseAnimations ();
			break;

		case GameState.Menu:
			MenuManager.Instance.ShowMenu (MainMenu);
			break;

		case GameState.End:
			MenuManager.Instance.ShowMenu(GameOverMenu);
			break;

		case GameState.Map:
			if(player.maps > 0)
			{
				player.decreaseMaps();
				MenuManager.Instance.ShowMenu (EmptyMenu);
				ToggleCameras ();
				player.PauseAnimations ();
				Invoke ("PlayerUnpause",3f);
				Invoke ("ToggleCameras", 3f);
				Invoke ("GameStateRun", 3f);
			}
			else
				ChangeGameState(GameState.Run);
			break; 
		}
	}
}
