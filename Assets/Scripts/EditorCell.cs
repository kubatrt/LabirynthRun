using UnityEngine;
//using UnityEditor;
using System.Collections;


[ExecuteInEditMode()]
public class EditorCell : MonoBehaviour 
{
	public MazeCell cell;

	private MazeGenerator	mazeGenerator;

	void Start () 
	{
		mazeGenerator = GameObject.FindObjectOfType<MazeGenerator>();
	}

	public void BreakExits()
	{
		cell.NoExits = true;
	}

	public void AdjustEastExit()
	{
		if(cell.Position.x+1 < mazeGenerator.Width)
		{
			if(cell.ExitEast == true) {
				
				mazeGenerator.GetCellAt(cell.Position.x+1, cell.Position.y).ExitWest = true; Debug.Log ("AdjustEastExit: true" );
			} else if(cell.ExitEast == false) {
				mazeGenerator.GetCellAt(cell.Position.x+1, cell.Position.y).ExitWest = false; Debug.Log ("AdjustEastExit: false");
			}
			
		}
	}
	
	public void AdjustWestExit()
	{
		if(cell.Position.x-1 >= 0)
		{
			if(cell.ExitWest) {		
				mazeGenerator.GetCellAt(cell.Position.x-1, cell.Position.y).ExitEast = true; Debug.Log ("AdjustWestExit: true" );
			} else {
				mazeGenerator.GetCellAt(cell.Position.x-1, cell.Position.y).ExitEast = false; Debug.Log ("AdjustWestExit: false");
			}
		}
	}
	
	public void AdjustNorthExit()
	{
		if(cell.Position.y-1 >= 0 )
		{
			if(cell.ExitNorth) {			
				mazeGenerator.GetCellAt(cell.Position.x, cell.Position.y-1).ExitSouth = true; Debug.Log ("AdjustNorthExit: true" );
			} else {
				mazeGenerator.GetCellAt(cell.Position.x, cell.Position.y-1).ExitSouth = false; Debug.Log ("AdjustNorthExit: true" );
			}
		}
	}
	
	public void AdjustSouthExit()
	{
		if(cell.Position.y+1 < mazeGenerator.Height)
		{
			if(cell.ExitSouth) {
				mazeGenerator.GetCellAt(cell.Position.x, cell.Position.y+1).ExitNorth = true; Debug.Log ("AdjustSouthExit: true" );
			} else {
				mazeGenerator.GetCellAt(cell.Position.x, cell.Position.y+1).ExitNorth = false; Debug.Log ("AdjustSouthExit: true" );
			}
		}
		
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red; // Eeast - x+1 // right
		if(cell.ExitEast)
			Gizmos.DrawLine(transform.position, transform.position + Vector3.right * 0.5f);

		Gizmos.color = Color.red; // Eeast - x-1 // left
		if(cell.ExitWest)
			Gizmos.DrawLine(transform.position, transform.position + Vector3.left * 0.5f);

		Gizmos.color = Color.blue; // North - z+1 / z-1 back / BUG!?
		if(cell.ExitNorth)
			Gizmos.DrawLine(transform.position, transform.position + Vector3.back * 0.5f);

		Gizmos.color = Color.blue; // North - z-1 / z+1 forward
		if(cell.ExitSouth)
			Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * 0.5f);
	}
}
