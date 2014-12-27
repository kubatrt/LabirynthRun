using UnityEngine;
using System.Collections;

public class CameraGUI : MonoBehaviour {

	//public static CameraGUI Instance;

	Animator animator;
	bool isAnimating = false;

	public Camera mapCamera;
	bool isChanging = false;

	float gameTime;
	//float mapTime = 3;

	int buttonHeight, buttonWidth;
	int screenCenterWidth, screenCenterHeight;
	int upArrowX,upArrowY,leftArrowX,leftArrowY,rightArrowX,rightArrowY,downArrowX,downArrowY;

	public GUISkin startCountdown, dedText;


	public Texture2D upArrow, leftArrow, rightArrow, downArrow, backButton;

	void Awake()
	{
		animator = GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () 
	{
		//set dimensions
		buttonWidth = (Screen.width+Screen.height)/12;
		buttonHeight = (Screen.width+Screen.height)/12;
		screenCenterWidth = Screen.width / 2;
		screenCenterHeight = Screen.height / 2;
		upArrowX = screenCenterWidth - buttonWidth / 2;
		upArrowY = screenCenterHeight - buttonHeight / 2 - Screen.height / 3;
		leftArrowX = screenCenterWidth - buttonWidth / 2 - Screen.width / 3;
		leftArrowY = screenCenterHeight - buttonHeight / 2;
		rightArrowX = screenCenterWidth - buttonWidth / 2 + Screen.width / 3;
		rightArrowY = screenCenterHeight - buttonHeight / 2;
		downArrowX = screenCenterWidth - buttonWidth / 2;
		downArrowY = screenCenterHeight - buttonHeight / 2 - Screen.height / 3;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// time update
		// set gameTime same as gameTime in player
		gameTime = PlayerController.Instance.gameTime;
		//mapTime -= Time.deltaTime;

		if (gameTime > 0 && Input.GetKeyDown (KeyCode.Mouse0)) 
		{
			PlayerController.Instance.gameTime = 0;
		}

		if(gameTime < 0)
		{
			startAnim ();
		}
	}

	void startAnim()
	{
		// aniamtion start
		if(! isAnimating)
		{
			isAnimating = true;
			animator.enabled = !animator.enabled;
			animation.playAutomatically = true;
		}
	}

	void changeCamera()
	{
		PlayerController.Instance.changeMoving ();
		isChanging = !isChanging;
		camera.enabled = !camera.enabled;
		mapCamera.enabled = !mapCamera.enabled;
	}

	void OnGUI()
	{
		// COMUNICATE ABOUT DED
		if(PlayerController.Instance.isDead == true)
		{
			GUI.skin = dedText;
			GUI.Box(new Rect(Screen.width/2-50 ,Screen.height/2-50,100,100)," DED ");
		}

		// BACK TO MENU BUTTON
		if (gameTime < -2) 
		{
			if (GUI.Button (new Rect (1, 1, buttonWidth, buttonHeight),backButton))
			{
				Application.LoadLevel(0);
			}
		}
		// GAME START COUNTDOWN
		if(gameTime > 0)
		{
			GUI.skin = startCountdown;
			GUI.Box(new Rect(Screen.width/2-50 ,Screen.height/2-60,100,100),"" + ((int)gameTime + 1));

		}
		// MAP BUTTON
		if (isChanging == false && gameTime < -2) 
		{
			//camera.enabled = true;
			if (GUI.Button (new Rect (1, Screen.height-buttonHeight, buttonWidth, buttonHeight),"MAPA")
			    && !PlayerController.Instance.chanceToChoice)
			{
				changeCamera();
				PlayerController.Instance.speed = 0;
				Invoke("changeCamera",3);
				PlayerController.Instance.speed = 5;
			}
		}
		// MOVEMENT
		if(PlayerController.Instance.chanceToChoice == true)
		{
			// UP 
			if(PlayerController.Instance.upArrow == true)
			{
				if(GUI.Button(new Rect(upArrowX,upArrowY,buttonWidth,buttonHeight),upArrow))
				{
					PlayerController.Instance.angle = 0;
					PlayerController.Instance.breakSlowAndGo();
					// signal that player made choice
					PlayerController.Instance.chanceToChoice = false;
				}
			}
			// RIGHT
			if(PlayerController.Instance.rightArrow == true)
			{
				if(GUI.Button(new Rect(rightArrowX,rightArrowY,buttonWidth,buttonHeight),rightArrow))
				{
					PlayerController.Instance.angle = 90;
					PlayerController.Instance.breakSlowAndGo();
					// signal that player made choice
					PlayerController.Instance.chanceToChoice = false;
				}
			}
			// LEFT
			if(PlayerController.Instance.leftArrow == true)
			{
				if(GUI.Button(new Rect(leftArrowX,leftArrowY,buttonWidth,buttonHeight),leftArrow))
				{
					PlayerController.Instance.angle = -90;
					PlayerController.Instance.breakSlowAndGo();
					// signal that player made choice
					PlayerController.Instance.chanceToChoice = false;
				}
			}
		}

	}

}
