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

	#region Toolbar

	[MenuItem ("Labyrinth/Scene/Maze Editor")]
	static void LoadEditor()
	{
		EditorApplication.OpenScene( Application.dataPath + "/Scenes/Maze Editor.unity");
		Debug.Log ("Load editor");
	}

	[MenuItem ("Labyrinth/Scene/Krystian")]
	static void LoadMazeSceneKrystian()
	{
		EditorApplication.OpenScene( Application.dataPath + "/Scenes/Random Maze (Krystian).unity");
	}

	[MenuItem ("Labyrinth/Scene/Main Game")]
	static void LoadMazeSceneKuba()
	{
		EditorApplication.OpenScene( Application.dataPath + "/Scenes/Game.unity");
	}

	[MenuItem ("Labyrinth/Maze Editor")]
	static void Initialize() 
	{
		MazeEditorWindow staticWindow  = (MazeEditorWindow)EditorWindow.GetWindow (typeof (MazeEditorWindow));
		staticWindow.title = "Maze Editor";
		staticWindow.Focus();
		staticWindow.RefreshFiles();
	}

	#endregion



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
		MazeGenerator maze = GameObject.FindObjectOfType<MazeGenerator>();
		//maze = GameObject.FindObjectOfType<MazeGenerator>();
		//maze = EditorGUILayout.ObjectField("MazeGenerator: ", maze, typeof(MazeGenerator)) as MazeGenerator;

		if(maze == null) {
			EditorGUILayout.HelpBox("Select MazeGenerator object!", MessageType.Warning);
			return;
		}

		mazeName = EditorGUILayout.TextField ("Name: ", mazeName);
		EditorGUILayout.LabelField("Size: " + maze.Width + " x "+ maze.Height );


		// find MazeGeneratorInspector
		MazeGeneratorInspector mazeGeneratorInspector = null;
		MazeGeneratorInspector[] editors = (MazeGeneratorInspector[]) Resources.FindObjectsOfTypeAll(typeof(MazeGeneratorInspector));
		if (editors.Length > 0)
		{
			mazeGeneratorInspector = editors[0];
		}
		if(mazeGeneratorInspector == null) {
			return;
		}

		if(GUILayout.Button("Save"))
		{
			MazeGenerator mazeGen = FindObjectOfType<MazeGenerator>();
			if(!mazeGen.Validate()) {	
				Debug.LogWarning("Validation failed! Check IsStartCell have exact ONE exit. Check  IsFinishCell have at least ONE. Not saved.");
				return;
			}	
			mazeGen.SaveToFile(LevelsDirectory + mazeName + ".maze");
			RefreshFiles();
		}

		// file list
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
		foreach(string file in levelFiles )
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(10);

			if(GUILayout.Button("X", GUILayout.Width(16)))
			{
				if(EditorUtility.DisplayDialog("Skasuj plik", "Na pewno chcesz usunąć plik?", "Tak", "Anuluj")) 
				{
					System.IO.File.Delete( System.IO.Path.GetFullPath(file) );
					RefreshFiles();
				}
			}

			GUILayout.Label(System.IO.Path.GetFileName(file), EditorStyles.boldLabel);

			if(GUILayout.Button("Load",  GUILayout.Width(48)))
			{
				maze.LoadFromFile(file);
				mazeGeneratorInspector.CreateEditorObjects(maze);
				mazeName = System.IO.Path.GetFileNameWithoutExtension(file);

				// add gizmos
				if(maze.gameObject.GetComponent<DebugDrawEditorMazeCells>() == null)
					maze.gameObject.AddComponent<DebugDrawEditorMazeCells>();
			}

			EditorGUILayout.EndHorizontal();
		} 
		EditorGUILayout.EndScrollView();

		if(GUILayout.Button("Refresh")) 
		{
			RefreshFiles();
		}

		GUILayout.Space (5);
		GUILayout.Label ("Selected object: " + objectName);
		GUILayout.Space (5);
	}

	private void RefreshFiles()
	{
		levelFiles.Clear();
		string projectPath = Application.dataPath + "/Levels/";
		string[] files = System.IO.Directory.GetFiles(projectPath, "*.maze", SearchOption.TopDirectoryOnly);
		levelFiles.AddRange(files);
		Debug.Log("GetLevelFiles: " + projectPath);
	}
}
