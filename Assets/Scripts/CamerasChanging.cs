using UnityEngine;
using System.Collections;

public class CamerasChanging : MonoBehaviour {

	public static CamerasChanging Instance;
	public GUISkin countdown;
	public float mapTime;
	bool isCounting;

	// Use this for initialization
	void Start () {
		camera.enabled = false;
		mapTime = 3;
	}
	
	// Update is called once per frame
	void Update () {

		if(camera.enabled)
		{
			if(!isCounting) 
			{
				mapTime= 3;
				isCounting =true;
			}
			mapTime -= Time.deltaTime;
		}
		else isCounting = false;
	}

	void OnGUI()
	{
		// GAME START COUNTDOWN
		if(camera.enabled && mapTime > 0)
		{
			GUI.skin = countdown;
			GUI.Box(new Rect(Screen.width/2-50 ,Screen.height/2-60,100,100),"" + ((int)mapTime + 1));
			
		}
	}
}
