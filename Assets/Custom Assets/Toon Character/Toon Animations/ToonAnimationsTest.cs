using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToonAnimationsTest : MonoBehaviour 
{
	public Animator animator;
	public string[] animations;
	public bool crossFade;
	public int maxButtons = 10;

	public string removeTextFromButton;
	public float autoChangeDelay;

	string lastAnim;
	int page = 0;
	int pages;

	void Start () 
	{
		pages = (int)Mathf.Ceil ((animations.Length - 1) / maxButtons);
	}
	
	void OnGUI () 
	{
		if(!animator)
			animator = GameObject.FindObjectOfType<Animator>();

		//Time Scale Vertical Slider
		//Time.timeScale = GUI.VerticalSlider (Rect (185, 50, 20, 150), Time.timeScale, 2.0, 0.0);
		//Check if there are more in list than max buttons (true adds "next" and "prev" buttons)
		if(animations.Length > maxButtons){
			//Prev button
			if(GUI.Button(new Rect(20,(maxButtons+1)*20,75,20),"Prev"))
				if(page > 0) 
					page--; 
				else 
					page = pages;
			//Next button
			if(GUI.Button(new Rect(95,(maxButtons+1)*20,75,20),"Next"))
				if(page < pages)
					page++;
				else 
					page = 0;

			if(GUI.Button(new Rect(Screen.width - 120,20,100,20),"Teachy Tess"))
				animator = GameObject.Find("Teachy Tess").GetComponent<Animator>();
			if(GUI.Button(new Rect(Screen.width - 120,20*2,100,20),"Archer Girl"))
				animator = GameObject.Find("Archer Girl").GetComponent<Animator>();


			GUI.Label(new Rect(Screen.width / 2f,20,150,22), "Current: " + animator.gameObject.name);

			//Page text
			GUI.Label(new Rect(60,(maxButtons + 2)*20,150,22), "Page" + (page+1) + " / " + (pages+1));
		}

		//Calculate how many buttons on current page (last page might have less)
		int pageButtonCount = animations.Length - (page*maxButtons);
		if(pageButtonCount > maxButtons) 
			pageButtonCount = maxButtons;

		//Adds buttons based on how many particle systems on page
		for(int i=0; i < pageButtonCount; i++)
		{
			string buttonText = animations[i + (page * maxButtons)];
			if(removeTextFromButton != "")
				buttonText = buttonText.Replace(removeTextFromButton, "");

			if(GUI.Button(new Rect(20,i*18+18,150,18),buttonText))
			{
				if(crossFade) 
				{	
					if(lastAnim == (animations[i + page * maxButtons]))
						this.animator.Play("");
					animator.CrossFade(animations[i + page * maxButtons], 0.1f);
					this.lastAnim = animations[i + page * maxButtons];
				}
				else 
				{
					animator.Play(animations[i + page * maxButtons]);
				}
	
			}
		}
	}

}
