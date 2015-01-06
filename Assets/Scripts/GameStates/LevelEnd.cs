using UnityEngine;
using System.Collections;

public class LevelEnd : MonoBehaviour 
{
	PlayerMecanimController player;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMecanimController> ();
	}

	void OnTriggerEnter(Collider targetColl)
	{
		
		if (targetColl.transform == player.transform) 
		{
			Destroy(gameObject);
			player.ToggleMoving();
			player.SetCelebrateAnim();
			Invoke("RestartLevel", 5);
		}

	}

	void RestartLevel()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}
