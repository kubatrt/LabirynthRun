using UnityEngine;
using System.Collections;

public class MenuButtons : MonoBehaviour {

	public void StartGame()
	{
		Application.LoadLevel (3);
	}

	public void ExitGame()
	{
		Application.Quit ();
	}


}
