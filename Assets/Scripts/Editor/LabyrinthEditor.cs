using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


[CustomEditor(typeof(Labyrinth))]
public class LabyrinthEditor : Editor 
{
	void OnEnable()
	{
		Debug.Log ("LabyrinthEditor.OnEnable()");
	}

	void OnDisable()
	{
		Debug.Log ("LabyrinthEditor.OnDisable()");
	}


	void GenerateMaze()
	{
		Labyrinth labyrinth = (Labyrinth)target;
		if(labyrinth.transform.GetComponentsInChildren<Transform>().Length != 1)
			return;

		labyrinth.GenerateMaze();
		labyrinth.BuildWalls();
		labyrinth.CreateGameObjects();
	}

	void ClearMaze()
	{
		GameObject walls = GameObject.Find("_Walls"); DestroyImmediate(walls);
		GameObject objects = GameObject.Find ("_Objects"); DestroyImmediate(objects);
		GameObject triggers = GameObject.Find("_Triggers"); DestroyImmediate(triggers);
	}

	public override void OnInspectorGUI()
	{
		//Labyrinth labyrinth = (Labyrinth)target;
		DrawDefaultInspector();


		if(GUILayout.Button("Generate maze"))
			GenerateMaze();
		if(GUILayout.Button("Clear maze"))
			ClearMaze();

		EditorGUILayout.HelpBox("Custom Labyrinth script editor", MessageType.Info);
	}
}
