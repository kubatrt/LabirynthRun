using UnityEngine;
using System.Collections;

public class LevelEnd : MonoBehaviour {

	void OnTriggerEnter(Collider targetColl)
	{
		
		if (targetColl.transform == PlayerController.Instance.gameObject.transform) 
		{
			PlayerController.Instance.changeMoving();
			PlayerController.Instance.SetCelebrateAnim();
			Invoke("RestartLevel", 5);
		}

	}

	void RestartLevel()
	{
		Application.LoadLevel(0);
	}
}
