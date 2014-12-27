using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	int buttonHeight, buttonWidth;
	int screenCenterWidth, screenCenterHeight;
	int startX,startY,exitX,exitY;

	// Use this for initialization
	void Start () 
	{
		buttonHeight = Screen.height / 5;
		buttonWidth = Screen.width / 2;
		screenCenterWidth = Screen.width / 2;
		screenCenterHeight = Screen.height / 2;
		startX = screenCenterWidth - buttonWidth / 2;
		startY = screenCenterHeight - buttonHeight / 2 ;
		exitX = screenCenterWidth - buttonWidth / 2;
		exitY = screenCenterHeight - buttonHeight / 2 + Screen.height/5;
	}
	
	// Update is called once per frame
	void Update () 
	{
		buttonHeight = Screen.height / 5;
		buttonWidth = Screen.width / 2;
		screenCenterWidth = Screen.width / 2;
		screenCenterHeight = Screen.height / 2;
		startX = screenCenterWidth - buttonWidth / 2;
		startY = screenCenterHeight - buttonHeight / 2;
		exitX = screenCenterWidth - buttonWidth / 2;
		exitY = screenCenterHeight - buttonHeight / 2 + Screen.height/5;
	}

	void OnGUI()
	{
		if (GUI.Button (new Rect (startX, startY, buttonWidth,buttonHeight), "PLAY")) 
		{
			Application.LoadLevel(1);
		}
		if (GUI.Button (new Rect (exitX, exitY, buttonWidth,buttonHeight), "EXIT")) 
		{
			Application.Quit();
		}

	}
}
