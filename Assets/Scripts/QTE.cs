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

	public virtual void Start()
	{
		player = GameObject.FindObjectOfType<PlayerMecanimController>();
	}

	public virtual void OnEnable()
	{
		NoChoice = true;
		startTime = Time.time;
		responseTime = 0;
		PanelUI.SetActive(true);
	}
	
	public virtual void OnDisable()
	{
		if(PanelUI != null)
			PanelUI.SetActive(false);
	}
	
}