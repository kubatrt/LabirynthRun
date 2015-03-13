using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode(), CanEditMultipleObjects]
public class EditorCell : MonoBehaviour 
{
	public MazeCell cell;
	private MazeGenerator	mazeGenerator;
	public bool IsSelected { set; get; }

	public int index;

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
		mazeGenerator.SetCellAtIndex (index, cell);
	}

	public void UpdateChanges()
	{
		index = cell.Index;
		
		// set material TODO: change
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
		Gizmos.color = Color.yellow;
		if(cell.ExitEast)
			Gizmos.DrawLine(transform.position, transform.position + Vector3.right * 0.5f);

		Gizmos.color = Color.blue;
		if(cell.ExitWest)
			Gizmos.DrawLine(transform.position, transform.position + Vector3.left * 0.5f);

		Gizmos.color = Color.green;
		if(cell.ExitNorth)
			Gizmos.DrawLine(transform.position, transform.position + Vector3.back * 0.5f);

		Gizmos.color = Color.red;
		if(cell.ExitSouth)
			Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * 0.5f);
	}
}
