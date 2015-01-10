using UnityEngine;
using System.Collections;

public class TriggerLevelEnd : MonoBehaviour 
{
	PlayerMecanimController player;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMecanimController> ();
	}

	void OnTriggerEnter(Collider targetColl)
	{
		
		if (targetColl.gameObject.tag == "Player") 
		{

			Destroy(gameObject);
			player.ToggleMoving();
			player.SetCelebrateAnim();
			Invoke("RestartLevel", 5);
		}

	}

	void RestartLevel()
	{
		Debug.Log("# Restart level ");
		Application.LoadLevel(Application.loadedLevel);
	}
}
