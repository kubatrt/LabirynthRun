using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode()]
public class EditorCell : MonoBehaviour 
{
	private MazeCell cell;
	private MazeGenerator	mazeGenerator;
	public bool IsSelected { set; get; }
	
	void Start () 
	{
		mazeGenerator = GameObject.FindObjectOfType<MazeGenerator>();
	}

	public void SetCell(MazeCell cellRef)
	{
		cell = cellRef;

		if(cell == null)
			return;

		ApplyChanges();
	}

	public void ApplyChanges() 
	{
		if(cell.IsStartCell)
			renderer.sharedMaterial = Resources.Load<Material>("Editor/EditorCellStart");
		else if(cell.IsFinishCell)
			renderer.sharedMaterial = Resources.Load<Material>("Editor/EditorCellEnd");
		else
			renderer.sharedMaterial = Resources.Load<Material>("Editor/EditorCell");
	}

	void Update()
	{

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
