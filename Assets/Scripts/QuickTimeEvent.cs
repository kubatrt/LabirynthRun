using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class QuickTimeEvent : MonoBehaviour 
{
	public GameObject	panel;

	public float startTime;
	public float responseTime;
	public float timeLimit = 2.0f;
	public bool noChoice;

	public MoveDirections directions = new MoveDirections();

	public PlayerMecanimController player;

	Button buttonLeft, buttonRight, buttonUp;
	Text responseText; 
	Slider sliderTimeLeft;

	void Awake() 
	{
		// find controls in "Panel" childs, could be public
		buttonLeft = panel.transform.FindChild("ButtonLeft").GetComponent<Button>();
		buttonRight = panel.transform.FindChild("ButtonRight").GetComponent<Button>();
		buttonUp = panel.transform.FindChild("ButtonUp").GetComponent<Button>();
		responseText = panel.transform.FindChild("TextResponse").GetComponent<Text>();
		sliderTimeLeft = panel.transform.FindChild("SliderTimeLeft").GetComponent<Slider>();

		buttonLeft.onClick.AddListener(OnClickButtonLeft);
		buttonRight.onClick.AddListener(OnClickButtonRight);
		buttonUp.onClick.AddListener(OnClickButtonUp);
	}

	void Start() 
	{
		if(player == null)
			player = GameObject.FindObjectOfType<PlayerMecanimController>();
	}

	void OnEnable()
	{
		noChoice = true;
		startTime = Time.time;
		responseTime = 0;
		panel.SetActive(true);
		//panel.
		SetupButtons();
		sliderTimeLeft.maxValue = timeLimit;
		sliderTimeLeft.minValue = 0f;
		sliderTimeLeft.value = timeLimit;	// 2s with speed = 5

		//Debug.Log ("QTE OnEnable()");
	}

	void OnDisable()
	{
		if(panel != null) {
			panel.SetActive(false);
		}
		//Debug.Log ("QTE OnDisable()");
	}
	
	void Update () 
	{
		responseTime += Time.deltaTime;

		responseText.text = string.Format ("{0:F2}", responseTime);
		sliderTimeLeft.value = timeLimit - responseTime;
	}

	void ResetButtons()
	{
		buttonLeft.enabled = false;
		buttonLeft.targetGraphic.enabled = false;
		buttonRight.enabled = false;
		buttonRight.targetGraphic.enabled = false;
		buttonUp.enabled = false;
		buttonUp.targetGraphic.enabled = false;
	}

	void SetupButtons()
	{
		ResetButtons();

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
			buttonUp.targetGraphic.enabled = false;
		}
	}

	#region UI controls

	void OnClickButtonLeft()
	{
		responseTime = Time.time - startTime;
		player.GoLeft();
		noChoice = false;
	}
	
	void OnClickButtonRight()
	{
		responseTime = Time.time - startTime;
		player.GoRight();
		noChoice = false;
	}
	
	void OnClickButtonUp()
	{
		responseTime = Time.time - startTime;
		player.GoForward();
		noChoice = false;
	}

	#endregion
}
