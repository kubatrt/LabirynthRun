using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
	static MenuManager instance;
	public static MenuManager Instance { 
		get; 
		private set;
	}

	public NewMenu CurrentMenu;

	public string username;

	public void Awake()
	{
		Instance = this;
	}
	public void Start()
	{
		//ShowMenu (CurrentMenu);
	}

	public void ShowMenu(NewMenu menu)
	{
		if(CurrentMenu != null)
			CurrentMenu.IsOpen = false;

		CurrentMenu = menu;
		CurrentMenu.IsOpen = true;
	}

	public void StartGame(int scene)
	{
		Application.LoadLevel (scene);
	}

	public void ExitGame()
	{
		Application.Quit ();
	}

	public void GetUsername(UnityEngine.UI.InputField inputFiled)
	{
		username = inputFiled.text;
	}

	public void ShowUsername(UnityEngine.UI.Text text)
	{
		text.text = username;
	}

	public void SetUsername()
	{
		GameManager.Instance.PlayerName = username;
	}
}
