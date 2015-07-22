using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class QTE : MonoBehaviour
{
	public GameObject	PanelUI;	// assign in editor UI representation

	public bool 		WasNoChoice;
	public float 		TimeLimit;
	
	protected PlayerMecanimController player;
	protected float startTime;
	protected float responseTime;

	public virtual void Start()
	{
		player = GameObject.FindObjectOfType<PlayerMecanimController>();
	}

	public virtual void PlayerResponse()
	{
		WasNoChoice = false;
		responseTime = Time.time - startTime;
	}

	public virtual void OnEnable()
	{
		WasNoChoice = true;
		startTime = Time.time;
		responseTime = 0;

		PanelUI.SetActive(true);

        Debug.Log("Enabled PanelUI Name" + PanelUI.name);
	}
	
	public virtual void OnDisable()
	{
        if (PanelUI != null) {
            PanelUI.SetActive(false);

            Debug.Log("Disabled PanelUI Name" + PanelUI.name);
        }

	}
	
}