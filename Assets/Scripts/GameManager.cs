using UnityEngine;
using System.Collections;

public enum GameState
{
	Menu,
	Start,
	Run,
	End,
	Pause
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

	string playerName = "";
	public string PlayerName {
		get { return playerName.Equals("") ? "Player" : playerName; }
		set { playerName = value; }
	}

	float gameStartupTimer;
	float gameTimeElapsed;


	void Awake() 
	{
		if(Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
		Debug.Log ("GameManager.Awake()");
	}

	void Start () 
	{
		Debug.Log ("GameManager.Start()");
		ChangeGameState (GameState.Menu);
	}

	public void ChangeGameState(GameState gameState)
	{
		state = gameState;
	}
}
