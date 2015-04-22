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
        SetStartPos();
        SetTriggerSize(); // only for tests
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

        if( distance < 1.5f && !isInside)
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
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        if(!Physics.Raycast(ray, dist))
        {
            trapLocation = TrapLocation.North_South;
        }
        else
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
                coll.size = new Vector3(1f, coll.size.y, 2.0f);
                break;
            case TrapLocation.East_West:
                coll.size = new Vector3(2.0f, coll.size.y, 1f);
                break;
        }
    }

}
