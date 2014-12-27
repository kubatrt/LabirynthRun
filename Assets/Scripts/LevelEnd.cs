using UnityEngine;
using System.Collections;

public class LevelEnd : MonoBehaviour {

	void OnTriggerEnter(Collider targetColl)
	{
		
		if (targetColl.transform == PlayerController.Instance.gameObject.transform) 
		{
			Application.LoadLevel(0);
		}

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
