using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


[CustomEditor(typeof(Labyrinth))]
public class LabyrinthInspector : Editor 
{

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if(GUILayout.Button("Create maze"))
			CreateMaze();
		if(GUILayout.Button("Clear maze")) {
			Labyrinth labyrinth = (Labyrinth)target;
			labyrinth.ClearEditorMaze();
			Debug.Log ("ClearEditorMaze");
		}

		EditorGUILayout.HelpBox("Custom Labyrinth script editor", MessageType.Info);
	}

	private void CreateMaze()
	{
		Labyrinth labyrinth = (Labyrinth)target;
		if(labyrinth.transform.GetComponentsInChildren<Transform>().Length != 1)
			return;

		labyrinth.CreateContainers(); // only in editor
		labyrinth.CreateMaze();	// generate new data
		labyrinth.BuildWalls();
		labyrinth.CreateGameObjects();
		labyrinth.CreateGround();
	}

}
