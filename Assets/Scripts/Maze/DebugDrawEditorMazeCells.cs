﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]
public class DebugDrawEditorMazeCells : MonoBehaviour
{
	[Range(0, 1)]
	public float opacity = 0f;

	private List<MazeCell> cells = new List<MazeCell>();

	public void UpdateCells()
	{
		cells = GetComponent<MazeGenerator>().GetCells();
	}

	void OnDrawGizmos()
	{
		UpdateCells();

		Vector3 frame = new Vector3(0.5f, 0.2f, 0.5f);
		Vector3 scale = new Vector3(0.45f, 0.15f, 0.45f);
		float factor = 0.48f;

		foreach (MazeCell cell in cells)
		{
			Vector3 centroid = new Vector3(cell.Position.x, 0f, cell.Position.y);
			Vector3 topLeft = new Vector3(centroid.x - factor, 0f, centroid.z - factor);
			Vector3 topRight = new Vector3(centroid.x + factor, 0f, centroid.z - factor);
			Vector3 bottomLeft = new Vector3(centroid.x - factor, 0f, centroid.z + factor);
			Vector3 bottomRight = new Vector3(centroid.x + factor, 0f, centroid.z + factor);
			

			if (cell.IsFinishCell)
			{
				Gizmos.color = new Color(0f,0f,1f, 0.25f);
				Gizmos.DrawCube(centroid, scale);
			}
			else if (cell.IsStartCell)
			{
				Gizmos.color = new Color(1f,0f,0.5f, 0.25f);
				Gizmos.DrawCube(centroid, scale);
			}
			else if (cell.IsDeadEnd)
			{
				Gizmos.color = new Color(1f,0f,0f, 0.25f);
				Gizmos.DrawCube(centroid, scale);
			}
			else if (cell.Trap != TrapType.None)
			{
				Gizmos.color = Gizmos.color = new Color(0, 0f,0f, 0.5f);
				Gizmos.DrawCube (centroid, scale);
			}
			else if (cell.TotalExits > 2)
			{
				Gizmos.color = Gizmos.color = new Color(1f,1f,0f, 0.15f * cell.TotalExits);
				Gizmos.DrawCube (centroid, scale);
			}


			// Normalized distance
			Gizmos.color = new Color(cell.NormalizedDistance, cell.NormalizedDistance, cell.NormalizedDistance, opacity);
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