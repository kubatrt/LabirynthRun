using UnityEngine;
using UnityEditor;
using System.Collections;


[ExecuteInEditMode(), CanEditMultipleObjects]
public class EditorCell : MonoBehaviour 
{
	public MazeCell cell;
	public int editorIndex;


	private MazeGenerator	mazeGenerator;

	void Start () 
	{
		mazeGenerator = GameObject.FindObjectOfType<MazeGenerator>();
	}

	public void SetCell(MazeCell cellRef)
	{
		cell = cellRef;

		if(cell == null)
			return;

		UpdateChanges();
	}

	public void ApplyChanges() 
	{	
		mazeGenerator.SetCellAtIndex (editorIndex, cell);
	}

	public void UpdateChanges()
	{
		editorIndex = cell.Index;

		// set material
		if(cell.IsStartCell)
			renderer.sharedMaterial = Resources.Load<Material>("Editor/EditorCellStart");
		else if(cell.IsFinishCell)
			renderer.sharedMaterial = Resources.Load<Material>("Editor/EditorCellFinish");
		else if(cell.IsDeadEnd)
			renderer.sharedMaterial = Resources.Load<Material>("Editor/EditorCellDeadEnd");
		else
			renderer.sharedMaterial = Resources.Load<Material>("Editor/EditorCell");

	}

	void OnDrawGizmos()
	{

		Gizmos.color = Color.yellow; // Eeast - x+1
		if(cell.ExitEast)
			Gizmos.DrawLine(transform.position, transform.position + Vector3.right * 0.5f);

		Gizmos.color = Color.blue; // Eeast - x-1
		if(cell.ExitWest)
			Gizmos.DrawLine(transform.position, transform.position + Vector3.left * 0.5f);

		Gizmos.color = Color.green; // North - z+1
		if(cell.ExitNorth)
			Gizmos.DrawLine(transform.position, transform.position + Vector3.back * 0.5f);

		Gizmos.color = Color.red; // North - z-1
		if(cell.ExitSouth)
			Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * 0.5f);
	}
}
