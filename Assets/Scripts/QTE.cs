using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class QTE : MonoBehaviour
{
	public GameObject	PanelUI;	// assign in editor
	public bool 		NoChoice;
	public float 		TimeLimit;
	
	protected PlayerMecanimController player;
	protected float startTime;
	protected float responseTime;
	
	protected void OnEnable()
	{
		NoChoice = true;
		startTime = Time.time;
		responseTime = 0;
		PanelUI.SetActive(true);
	}
	
	protected void OnDisable()
	{
		if(PanelUI != null)
			PanelUI.SetActive(false);
	}
	
}