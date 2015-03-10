using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode()]
public class EditCell : MonoBehaviour 
{
	public MazeCell cell;

	// Use this for initialization
	void Start () {
	
	}

	public void SetCell(MazeCell cellRef)
	{
		cell = cellRef;
		Debug.Log ("CELL " + cell.ToString());
	}

	void Update () 
	{
		if(Selection.activeGameObject.gameObject == this.gameObject) {
			Debug.Log ("Selected " + gameObject.name);
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		if(cell.ExitEast)
			Gizmos.DrawLine(transform.position, transform.position + Vector3.right);
		Gizmos.color = Color.blue;
		if(cell.ExitWest)
			Gizmos.DrawLine(transform.position, transform.position + Vector3.left);
		Gizmos.color = Color.green;
		if(cell.ExitNorth)
			Gizmos.DrawLine(transform.position, transform.position + Vector3.back);
		Gizmos.color = Color.red;
		if(cell.ExitSouth)
			Gizmos.DrawLine(transform.position, transform.position + Vector3.forward);
	}
}
