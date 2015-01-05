using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMainMenu : MonoBehaviour 
{
	public string username;

	public InputField nameInputField;
	public Text nameText;


	public void StartGame()
	{
		Application.LoadLevel ("Random Maze (Krystian)");
	}
	
	public void ExitGame()
	{
		Application.Quit ();
	}
	
	public void EnterInputName ()
	{
		username = nameInputField.text;
		nameText.text = username;
		nameInputField.enabled = false;
	}
	
	public void ClearInputName ()
	{
		username = "";
		nameText.text = username;
		nameInputField.text = username;
		nameInputField.enabled = true;
	}

}
