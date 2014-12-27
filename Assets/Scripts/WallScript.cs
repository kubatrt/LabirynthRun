using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {
	
	void OnTriggerEnter()
	{
		PlayerController.Instance.isDead = true;
		PlayerController.Instance.isMoving = false;
		Invoke("loadLevel",1);
	}

	void loadLevel()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}
