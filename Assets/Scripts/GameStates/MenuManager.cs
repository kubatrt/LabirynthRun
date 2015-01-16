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
	public NewMenu MainMenu;
	public NewMenu HUD;
	public NewMenu PauseMenu;
	public NewMenu EmptyMenu;

	public string username;

	public void Start()
	{
		ShowMenu (CurrentMenu);
	}

	void Update()
	{
		switch(GameManager.Instance.state)
		{
		case GameState.Start:
			ShowMenu (EmptyMenu);
			break;
		case GameState.Run:
			ShowMenu (HUD);
			break;
		case GameState.Pause:
			ShowMenu (PauseMenu);
			break;
		case GameState.Menu:
			ShowMenu (MainMenu);
			break;
		case GameState.End:
			break;
		case GameState.Map:
			ShowMenu (EmptyMenu);
			break;
		}
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
