using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


[CustomEditor(typeof(Labyrinth))]
public class LabyrinthEditor : Editor 
{

	public override void OnInspectorGUI()
	{
		//Labyrinth labyrinth = (Labyrinth)target;
		DrawDefaultInspector();

		if(GUILayout.Button("Create maze"))
			CreateMaze();
		if(GUILayout.Button("Clear maze"))
			ClearMaze();

		EditorGUILayout.HelpBox("Custom Labyrinth script editor", MessageType.Info);
	}

	private void CreateMaze()
	{
		Labyrinth labyrinth = (Labyrinth)target;
		if(labyrinth.transform.GetComponentsInChildren<Transform>().Length != 1)
			return;
		
		labyrinth.CreateMaze();
		labyrinth.BuildWalls();
		labyrinth.CreateGameObjects();
		labyrinth.CreateGround();
	}

	private void ClearMaze()
	{
		Labyrinth labyrinth = (Labyrinth)target;
		labyrinth.ClearMaze();
	}
}
