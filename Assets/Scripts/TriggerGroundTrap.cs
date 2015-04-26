using UnityEngine;
using System.Collections;

public enum TrapLocation
{
    North_South,
    East_West
}

public class TriggerGroundTrap : MonoBehaviour 
{
	PlayerMecanimController player;

    public TrapLocation trapLocation;

    private bool isInside;
	
	void Start()
	{
		player = GameObject.FindObjectOfType<PlayerMecanimController>();

        SetStartPos();
        SetTriggerSize();
	}

    void Update()
    {
        //SetStartPos();
        //SetTriggerSize(); // only for tests


        if (GameManager.Instance.state != GameState.Run )
        {
            gameObject.renderer.enabled = false;
        }
        else
            gameObject.renderer.enabled = true;
    }
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag != "Player")
			return;
		
		//Quaternion playerRotation = player.gameObject.transform.rotation; 
		//transform.rotation = playerRotation;

        player.EnterTheTrapArea();
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag != "Player")
			return;

        Vector3 playerPos = player.gameObject.transform.position; playerPos.y = 0;
        Vector3 triggerPos = transform.position; triggerPos.y = 0;

        float distance = Vector3.Distance(triggerPos, playerPos);
        distance = Mathf.Abs(distance);
        float trapRange = 0f;

        switch (trapLocation)
        {
            case TrapLocation.North_South:
                trapRange = transform.localScale.z / 2f;        //   <- distance to the 
                break;                                          //   <-                      
            case TrapLocation.East_West:                        //   <- edge of trap
                trapRange = transform.localScale.x / 2f;
                break;
        }

        trapRange += 0.25f;            // <- extra distance between player and edge of trap

        if( distance < trapRange && !isInside)
        {
            isInside = true;
            player.MoveOverTrapArea();
        }

	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag != "Player")
			return;

        isInside = false;

        player.RunPlayer();
	}

    private void SetStartPos()
    {
        float dist = 2f;
        Ray ray = new Ray(transform.position, transform.forward);
        Ray rayRight = new Ray(transform.position, transform.right);
        if(!Physics.Raycast(ray, dist))
        {
            trapLocation = TrapLocation.North_South;
        }
        else if(!Physics.Raycast(rayRight, dist))
        {
            trapLocation = TrapLocation.East_West;
        }
    }

    private void SetTriggerSize()
    {
        BoxCollider coll = GetComponent<BoxCollider>();

        switch(trapLocation)
        {
            case TrapLocation.North_South:
                transform.localScale = new Vector3(3f, transform.localScale.y, 1.5f);
                coll.size = new Vector3(1f, coll.size.y, 5f);
                break;
            case TrapLocation.East_West:
                transform.localScale = new Vector3(1.5f, transform.localScale.y, 3f);
                coll.size = new Vector3(5f, coll.size.y, 1f);
                break;
        }
    }

}
