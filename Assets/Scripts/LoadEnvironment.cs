using UnityEngine;
using System.Collections;

public class LoadEnvironment : MonoBehaviour {

	void Awake()
	{
		Application.LoadLevelAdditive ("Enviro_Islands");
	}
}
