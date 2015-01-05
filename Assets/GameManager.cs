using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
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
