using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
//[RequireComponent(typeof(MeshCollider))]
public class EditorMazeCell : MonoBehaviour 
{
	public MazeCell	cell;

	void Update()
	{

	}

	void OnDrawGizmos()
	{
		if(cell == null)
			return;

		Vector3 centroid = new Vector3(cell.Position.x, 0f, cell.Position.y);
		Vector3 scale = new Vector3(0.75f, 0.75f, 0.75f);

		Gizmos.color = new Color(0f,1f,0f, 0.5f);
		Gizmos.DrawCube(centroid, scale);
	}
}
