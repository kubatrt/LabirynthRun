using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MazeEditorWindow : EditorWindow 
{
	readonly string LevelsDirectory = Application.dataPath + "/Levels/";

	static MazeEditorWindow staticWindow;

	[MenuItem ("Tools/MazeEditor")]
	static void Initialize() 
	{
		// MazeEditorWindow window 
		MazeEditorWindow staticWindow  = (MazeEditorWindow)EditorWindow.GetWindow (typeof (MazeEditorWindow));
		staticWindow.Focus();
		staticWindow.RefreshFilesList();
	}

	public MazeGenerator maze = null;
	public string mazeName;
	public int mazeWidth, mazeHeight = 0;

	private List<String> levelFiles = new List<String>();
	private Vector2 scrollPosition;	
	private string objectName;
	private string lastSelected;
	private bool repaint = false;

	void Update()
	{
		// selected object on scene
		if (Selection.activeGameObject && Selection.activeGameObject.name != lastSelected)
		{
			objectName = Selection.activeGameObject.name;
			this.Repaint();

			lastSelected = Selection.activeGameObject.name;
			repaint = true;
		}
		else if (Selection.activeGameObject == null && repaint == true)
		{
			objectName = "Please Select an Object";
			this.Repaint();

			repaint = false;
			lastSelected = "";
		}
	}




	void OnGUI () 
	{
		GUILayout.Label ("Maze settings", EditorStyles.boldLabel);

		// check if MazeGenerator is assigned
		//maze = (MazeGenerator) EditorGUILayout.ObjectField("MazeGenerator: ", maze, typeof(MazeGenerator));
		maze = GameObject.FindObjectOfType<MazeGenerator>();
		if(!maze) {
			EditorGUILayout.HelpBox("Select MazeGenerator object to work with!", MessageType.Warning);
			return;
		}
		mazeName = EditorGUILayout.TextField ("Name: ", mazeName);
		EditorGUILayout.LabelField("Size: " + maze.Width + " x "+ maze.Height );


		// find MazeGeneratorInspector
		MazeGeneratorInspector mazeGeneratorInspector = null;
		MazeGeneratorInspector[] editors = (MazeGeneratorInspector[]) Resources.FindObjectsOfTypeAll(typeof(MazeGeneratorInspector));
		if (editors.Length > 0)
		{
			mazeGeneratorInspector =  editors[0];
		}
		if(mazeGeneratorInspector == null) {
			return;
		}

		if(GUILayout.Button("Save"))
		{
			MazeGenerator mazeGen = FindObjectOfType<MazeGenerator>();
			mazeGen.SaveToFile(LevelsDirectory + mazeName + ".maze");
			RefreshFilesList();
		}

		// file list
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
		foreach(string file in levelFiles )
		{
			EditorGUILayout.BeginHorizontal();

			GUILayout.Label(System.IO.Path.GetFileName(file), EditorStyles.boldLabel);
			if(GUILayout.Button("Load",  GUILayout.Width(48)))
			{
				maze.LoadFromFile(file);
				mazeGeneratorInspector.CreateEditorObjects(maze);
				mazeName = System.IO.Path.GetFileNameWithoutExtension(file);
			}

			EditorGUILayout.EndHorizontal();
		} 
		EditorGUILayout.EndScrollView();


		if(GUILayout.Button("Refresh")) 
		{
			RefreshFilesList();
		}

		GUILayout.Space (5);
		GUILayout.Label ("Selected object: " + objectName);
		GUILayout.Space (5);
	}

	private void RefreshFilesList()
	{
		levelFiles.Clear();
		string projectPath = Application.dataPath + "/Levels/";
		string[] files = System.IO.Directory.GetFiles(projectPath, "*.maze", SearchOption.TopDirectoryOnly);
		levelFiles.AddRange(files);
		Debug.Log("GetLevelFiles: " + projectPath);
	}
}
