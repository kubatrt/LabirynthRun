using UnityEngine;
using System.Collections;


public class TriggerTrapGround : MonoBehaviour {

    PlayerMecanimController player;

	public TrapOrientation	orientation;
	public TrapType	type;

    private bool isInside;

	void Start () 
    {
        player = GameObject.FindObjectOfType<PlayerMecanimController>();
		CheckOrientation();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

		player.EnterTrap(type, orientation);
		Debug.Log ("Trap enter");
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        Vector3 playerPos = player.gameObject.transform.position; playerPos.y = 0;
        Vector3 triggerPos = transform.position; triggerPos.y = 0;

        float distance = Vector3.Distance(triggerPos, playerPos);
        distance = Mathf.Abs(distance);
        //float trapRange = 0f;
		

        //trapRange += 0.25f;            // <- extra distance between player and edge of trap

        //if (distance < trapRange && !isInside)
        //{
        //    isInside = true;
            //player.MoveOverTrapArea();
        //}
		Debug.Log ("Trap distance: " + distance);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        //isInside = false;

        //player.RunPlayer();
		Debug.Log ("Trap exit");
    }

    private void CheckOrientation()
    {
        float dist = 2f;
        Ray ray = new Ray(transform.position, transform.forward);
        Ray rayRight = new Ray(transform.position, transform.right);
        if (!Physics.Raycast(ray, dist))
        {
			orientation = TrapOrientation.NorthSouth;
			Debug.Log ("Orientation set NorthSouth ");
        }
        else if (!Physics.Raycast(rayRight, dist))
        {
			orientation = TrapOrientation.EastWest;
			Debug.Log ("Orientation set EastWest ");
        }

    }

}
