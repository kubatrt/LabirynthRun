using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class QuickTimeEvent : MonoBehaviour 
{
	public RectTransform	panel;

	public float startTime;
	public float responseTime;
	public bool noChoice;

	public MoveDirections directions = new MoveDirections();

	public PlayerMecanimController player;

	public Button buttonLeft, buttonRight, buttonUp;
	Text responseText; 

	void Awake() 
	{
		buttonLeft = panel.transform.FindChild("ButtonLeft").GetComponent<Button>();
		buttonRight = panel.transform.FindChild("ButtonRight").GetComponent<Button>();
		buttonUp = panel.transform.FindChild("ButtonUp").GetComponent<Button>();

		buttonLeft.onClick.AddListener(OnClickButtonLeft);
		buttonRight.onClick.AddListener(OnClickButtonRight);
		buttonUp.onClick.AddListener(OnClickButtonUp);


		responseText = panel.transform.FindChild("TextResponse").GetComponent<Text>();
	}

	void Start() 
	{
		//player = GameObject.FindWithTag("Player").GetComponent <PlayerMecanimController>();
		// start as diabled
		//panel.gameObject.SetActive(false);
	}

	void OnEnable()
	{
		noChoice = true;
		startTime = Time.time;
		responseTime = 0;
		panel.gameObject.SetActive(true);
		SetupButtons();
		Debug.Log ("QTE OnEnable()");
	}

	void OnDisable()
	{
		panel.gameObject.SetActive(false);
		Debug.Log ("QTE OnDisable()");
	}
	
	void Update () 
	{

		responseTime += Time.deltaTime;
		responseText.text = responseTime.ToString();
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
		Debug.Log("SetupButtons() L: " + directions.Left + " R:" + directions.Right + " F:" + directions.Forward);
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
