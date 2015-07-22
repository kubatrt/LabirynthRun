using UnityEngine;
using System.Collections;

public class TriggerLevelEnd : MonoBehaviour 
{

	void OnTriggerEnter(Collider other)
	{	
		if (other.gameObject.tag == "Player") 
		{
			PlayerMecanimController player = other.GetComponent<PlayerMecanimController>();
			gameObject.renderer.enabled = false;
			player.ToggleMoving();
			player.SetCelebrateAnim();
			GameManager.Instance.ChangeGameState(GameState.EndWon);
		}

	}
}
