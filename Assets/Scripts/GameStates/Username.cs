using UnityEngine;
using System.Collections;

public class Username : MonoBehaviour {

	public string username;
	public UnityEngine.UI.InputField inputFiled;
	public UnityEngine.UI.Text nameDisplay;


	public void NameInput ()
	{
		username = inputFiled.text;
		nameDisplay.text = username;
		inputFiled.enabled = false;
	}

	public void ResetInputField ()
	{
		username = "";
		nameDisplay.text = username;
		inputFiled.text = username;
		inputFiled.enabled = true;
	}
}
