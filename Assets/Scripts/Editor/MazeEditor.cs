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
		}

		EditorGUILayout.HelpBox("Custom Maze Generator script editor", MessageType.Info);
	}
}
