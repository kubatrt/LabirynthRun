using UnityEngine;
using System.Collections;

public class LoadEnvironment : MonoBehaviour 
{
	public string environment;

	void Awake()
	{
		if(environment != "")
			Application.LoadLevelAdditive (environment);
	}
}
