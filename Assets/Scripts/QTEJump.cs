using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class QTEJump : QTE
{
	private Button buttonJump;
	private float timeLeft;
	
	void Awake()
	{
		TimeLimit = 1.0f;
		buttonJump = PanelUI.transform.FindChild("ButtonJump").GetComponent<Button>();
	}
	
	void Update()
	{
		if(NoChoice && GameManager.Instance.state == GameState.Run)
			responseTime += Time.deltaTime;
		
		timeLeft = TimeLimit - responseTime;
	}
	
	void OnClickButtonJump()
	{
		responseTime = Time.time - startTime;
		player.GoForward();
		NoChoice = false;
	}
}