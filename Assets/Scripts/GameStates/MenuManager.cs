﻿using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
	public NewMenu CurrentMenu;
	public string username;

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
		GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ().playerName = username;
	}
}
