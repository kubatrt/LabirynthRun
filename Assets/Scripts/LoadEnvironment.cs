using UnityEngine;
using System.Collections;

public class LoadEnvironment : MonoBehaviour 
{
	public string envname;

	void Awake()
	{
		Application.LoadLevelAdditive (envname);
	}
}
