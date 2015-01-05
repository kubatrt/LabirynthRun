using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
	public NewMenu CurrentMenu;

	public void Start()
	{
		ShowMenu (CurrentMenu);
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
}
