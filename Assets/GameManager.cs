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
	public string playerName;

	float gameStartupTimer;
	float gameTimeElapsed;



	private static GameManager instance;
	public static GameManager Instance { 
		get; 
		private set;
	}

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
	}

	
}
