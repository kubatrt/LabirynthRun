using UnityEngine;
using System.Collections;

public class TriggerLevelEnd : MonoBehaviour 
{
    [SerializeField]
	PlayerMecanimController player;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMecanimController>();
        Debug.Log("Gdzie jest player?");
	}


	void Update()
	{
		if(GameManager.Instance.state != GameState.Run && GameManager.Instance.state != GameState.Start
		   && GameManager.Instance.state != GameState.Map)
		{
			gameObject.renderer.enabled = false;	
		}
		else
			gameObject.renderer.enabled = true;
	}

	void OnTriggerEnter(Collider targetColl)
	{
		
		if (targetColl.gameObject.tag == "Player") 
		{
			gameObject.renderer.enabled = false;
			player.ToggleMoving();
			player.SetCelebrateAnim();
			GameManager.Instance.ChangeGameState(GameState.EndWon);
		}

	}
}
