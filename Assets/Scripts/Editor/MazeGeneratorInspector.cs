using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(MazeGenerator))]
public class MazeGeneratorInspector : Editor 
{
	private static List<EditorCell> editorCellsList = new List<EditorCell>();
	private static bool generated = false;
	private static bool debugDraw = true;

	private string tipMessage = "Custom Maze Generator script editor";
	private DebugDrawEditorMazeCells	debugGizmos;


	public void CreateEditorObjects(MazeGenerator maze)
	{
		if(generated) { 
			Debug.LogWarning("CreateEditorObjects: already generated!");
			return;
		}

		GameObject editorContrainer = GameObject.FindGameObjectWithTag ("MazeEditor");
		foreach (MazeCell cell in maze.GetCells()) 
		{
			if (editorContrainer != null) 
			{
				EditorCell edCellObject = (EditorCell)Instantiate (
					Resources.Load<EditorCell> ("Editor/EditorCell"), 	// load from Resources
					MazeGenerator.GridToWorld(cell.Position, 1f, 0f),
					Quaternion.identity);

				edCellObject.name = "editorcell_" + cell.Index;
				edCellObject.transform.parent = editorContrainer.transform;
				edCellObject.cell = cell;

				editorCellsList.Add(edCellObject);
			}
		}
		generated = true;
	}

	public override void OnInspectorGUI()
	{
		MazeGenerator maze = (MazeGenerator)target;
		DrawDefaultInspector();

		debugDraw = GUILayout.Toggle(debugDraw, "Add DebugDraw component", EditorStyles.toggle);

		if (GUILayout.Button("Generate new maze")) 
		{
			if (generated) {
				tipMessage = "Maze already generated. First clear it.";
				return;
			}

			maze.Generate ();
			CreateEditorObjects(maze);

			if(debugDraw)
				if(maze.gameObject.GetComponent<DebugDrawEditorMazeCells>() == null)
					maze.gameObject.AddComponent<DebugDrawEditorMazeCells>();

			tipMessage =  string.Format ("New maze data {0} x {1} generated!", maze.Width, maze.Height);
			SceneView.RepaintAll();
		}

		if (GUILayout.Button("Clear editor maze")) 
		{
			if(editorCellsList != null)
				editorCellsList.Clear();

			GameObject container = GameObject.FindGameObjectWithTag("MazeEditor");
			if(container != null) {
				for (int i = container.transform.childCount - 1; i >= 0; i--) {
					DestroyImmediate (container.transform.GetChild (i).gameObject);
				}
			}

			generated = false;
			tipMessage = "Maze cleared. All objects destroyed.";
		}

		if (GUILayout.Button("Break all exits")) 
		{
			foreach(EditorCell edcell in editorCellsList)
			{
				edcell.BreakExits();
			}
		}

		EditorGUILayout.HelpBox(tipMessage, MessageType.Info);
	}
}
