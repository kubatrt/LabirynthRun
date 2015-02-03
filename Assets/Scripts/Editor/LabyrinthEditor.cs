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
	}
	
	private void ClearMaze()
	{
		GameObject walls = GameObject.Find("_Walls"); DestroyImmediate(walls);
		GameObject objects = GameObject.Find ("_Objects"); DestroyImmediate(objects);
		GameObject triggers = GameObject.Find("_Triggers"); DestroyImmediate(triggers);
	}
}
