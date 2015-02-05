using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(MazeGenerator))]
public class MazeEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		MazeGenerator maze = (MazeGenerator)target;
		DrawDefaultInspector();

		if(GUILayout.Button("Generate maze"))
		{
			maze.Generate();
			maze.transform.GetComponent<DebugDrawMazeCells>().UpdateCells();


			foreach(MazeCell cell in maze.GetCells())
			{
				Vector3 position = new Vector3(cell.Position.x, 0f, cell.Position.y);
				GameObject editorCell = (GameObject)Instantiate(Resources.Load<GameObject>("EditorCell"), position, Quaternion.identity);
				editorCell.name = "EditorCell-" + cell.Index;
				editorCell.GetComponent<EditorMazeCell>().cell = cell;
			}

			SceneView.RepaintAll();
		}

		EditorGUILayout.HelpBox("Custom Maze Generator script editor", MessageType.Info);
	}
}
