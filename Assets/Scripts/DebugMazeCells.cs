using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugMazeCells : MonoBehaviour
{

	private List<MazeCell> Cells = new List<MazeCell>();


	public void AddCell(MazeCell cell)
	{
		Cells.Add(cell);
	}

	// Draw the debug line.
	private void OnDrawGizmos()
	{
		Vector3 frame = new Vector3(0.5f, 0.2f, 0.5f);
		Vector3 scale = new Vector3(0.45f, 0.15f, 0.45f);
		
		foreach (MazeCell cell in Cells)
		{
			Vector3 centroid = new Vector3(cell.Location.x, 0f, cell.Location.y);
			Vector3 topLeft = new Vector3(centroid.x - 0.5f, 0f, centroid.z - 0.5f);
			Vector3 topRight = new Vector3(centroid.x + 0.5f, 0f, centroid.z - 0.5f);
			Vector3 bottomLeft = new Vector3(centroid.x - 0.5f, 0f, centroid.z + 0.5f);
			Vector3 bottomRight = new Vector3(centroid.x + 0.5f, 0f, centroid.z + 0.5f);
			
			// Draw color coded cell properties
			if (cell.IsStartCell)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawCube(centroid, scale);
			}
			else if (cell.IsDeadEnd)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawCube(centroid, scale);
			}
			
			// Draw normalized crawl distance
			Gizmos.color = new Color(0f, 0f, cell.NormalizedDistance, 1f);
			Gizmos.DrawWireCube(centroid, frame);
			
			// Draw wall lines
			Gizmos.color = Color.cyan;
			if (!cell.ExitNorth)
				Gizmos.DrawLine(topLeft, topRight);
			if (!cell.ExitSouth)
				Gizmos.DrawLine(bottomLeft, bottomRight);
			if (!cell.ExitEast)
				Gizmos.DrawLine(topRight, bottomRight);
			if (!cell.ExitWest)
				Gizmos.DrawLine(topLeft, bottomLeft);
		}
	}
}