using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(MazeGenerator))]
public class MazeGeneratorInspector : Editor 
{
	bool generated = false;
	string tipMessage = "Custom Maze Generator script editor";

	private List<EditorCell> editorCellsList = new List<EditorCell>();


	public void OnEnable()
	{
		Debug.Log ("MazeGenerator.Enable()");

	}

	public void OnDisable()
	{
		Debug.Log ("MazeGenerator.Disable()");	
	}

	public void CreateEditorObjects(MazeGenerator maze)
	{
		if(generated){ 
			Debug.LogWarning("Already generated!");
			return;
		}
		GameObject editorContrainer = GameObject.Find ("_EDITOR");

		foreach (MazeCell cell in maze.GetCells()) 
		{
			if (editorContrainer != null) 
			{
				EditorCell edCellObject = (EditorCell)Instantiate (
					Resources.Load<EditorCell> ("Editor/EditorCell"), 
					MazeGenerator.GridToWorld(cell.Position, 1f, 0f), // new Vector3 (cell.Position.x, 0f, cell.Position.y), 
					Quaternion.identity);

				edCellObject.name = "EditorCell_" + cell.Index;
				edCellObject.transform.parent = editorContrainer.transform;
				edCellObject.SetCell (cell);

				editorCellsList.Add (edCellObject);
			}
		}
		generated = true;
	}

	public override void OnInspectorGUI()
	{
		MazeGenerator maze = (MazeGenerator)target;
		DrawDefaultInspector ();

		if (GUILayout.Button ("Generate editor maze")) 
		{
			if (generated) {
				tipMessage = "Maze already generated. First clear it.";
				return;
			}

			maze.Generate ();
			CreateEditorObjects(maze);
			//maze.transform.GetComponent<DebugDrawMazeCells>().UpdateCells();

			tipMessage = "New maze data generated!";
			SceneView.RepaintAll ();
		}

		if (GUILayout.Button ("Clear editor maze")) 
		{
			GameObject container = GameObject.Find ("_EDITOR");
			for (int i = container.transform.childCount - 1; i >= 0; i--) 
			{
				DestroyImmediate (container.transform.GetChild (i).gameObject);
			}

			if(editorCellsList != null)
				editorCellsList.Clear();
			//maze.GetComponent<DebugDrawEditorMazeCells>().UpdateCells();

			generated = false;
			tipMessage = "Maze cleared. All objects destroyed.";
		}

		if (GUILayout.Button ("Break all exits")) 
		{
			foreach(EditorCell edcell in editorCellsList)
			{
				edcell.BreakExits();
			}
		}

		EditorGUILayout.HelpBox(tipMessage, MessageType.Info);
	}
}
