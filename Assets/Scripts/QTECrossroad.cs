using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class QTECrossroad : QTE
{
	public MoveDirections directions = new MoveDirections();

	// UI
	private Button buttonLeft, buttonRight, buttonUp;
	private Text responseText; 
	private Slider sliderTimeLeft;
	private Image circleFillTimeLeft;

	void Awake() 
	{
		TimeLimit = 2.0f;
		buttonLeft = PanelUI.transform.FindChild("ButtonLeft").GetComponent<Button>();
		buttonRight = PanelUI.transform.FindChild("ButtonRight").GetComponent<Button>();
		buttonUp = PanelUI.transform.FindChild("ButtonUp").GetComponent<Button>();

		//sliderTimeLeft = PanelUI.transform.FindChild("SliderTimeLeft").GetComponent<Slider>();
		//responseText = panel.transform.FindChild("TextResponse").GetComponent<Text>();
		circleFillTimeLeft = PanelUI.transform.FindChild("FillCircle").GetComponent<Image>();

		buttonLeft.onClick.AddListener(OnClickButtonLeft);
		buttonRight.onClick.AddListener(OnClickButtonRight);
		buttonUp.onClick.AddListener(OnClickButtonUp);

	}

	public override void OnEnable()
	{
		base.OnEnable();

		SetupButtons();
		//sliderTimeLeft.maxValue = TimeLimit;
		//sliderTimeLeft.minValue = 0f;
		//sliderTimeLeft.value = TimeLimit;
		circleFillTimeLeft.fillAmount = 1f;
	}

	
	void Update () 
	{
		// TODO: checking GameState shouldnt be here
		if(WasNoChoice && GameManager.Instance.state == GameState.Run)
			responseTime += Time.deltaTime;

		//responseText.text = string.Format ("{0:F2}", responseTime);
		//sliderTimeLeft.value = TimeLimit - responseTime;
		float value = (TimeLimit - responseTime) * (1f / TimeLimit);
		circleFillTimeLeft.fillAmount = value;
	}

	void SetupButtons()
	{
		//Debug.Log(string.Format("# SetupButtons L: {0} R: {1} F: {2}", directions.Left, directions.Right, directions.Forward));
		buttonLeft.enabled = false;
		buttonLeft.targetGraphic.enabled = false;
		buttonRight.enabled = false;
		buttonRight.targetGraphic.enabled = false;
		buttonUp.enabled = false;
		buttonUp.targetGraphic.enabled = false;
		
		if(directions.Left) {
			buttonLeft.enabled = true;
			buttonLeft.targetGraphic.enabled = true;
		}
		if(directions.Right) {
			buttonRight.enabled = true;
			buttonRight.targetGraphic.enabled = true;
		}
		if(directions.Forward) {
			buttonUp.enabled = true;
			buttonUp.targetGraphic.enabled = true;
		}
	}


	#region UI controls

	void OnClickButtonLeft()
	{
		PlayerResponse();
		player.GoLeft();
	}
	
	void OnClickButtonRight()
	{
		PlayerResponse();
		player.GoRight();
	}
	
	void OnClickButtonUp()
	{
		PlayerResponse();
		player.GoForward();
	}

	#endregion
}
