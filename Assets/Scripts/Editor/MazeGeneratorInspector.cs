using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(MazeGenerator))]
public class MazeGeneratorInspector : Editor 
{
	bool generated = false;
	string tipMessage = "Custom Maze Generator script editor";

	public void CreateEditorObjects(MazeGenerator maze)
	{
		if(generated){ 
			Debug.LogWarning("Already generated!");
			return;
		}
		GameObject editorContrainer = GameObject.Find ("_EDITOR");
		
		foreach (MazeCell cell in maze.GetCells()) {
			Vector3 position = new Vector3 (cell.Position.x, 0f, cell.Position.y);
			if (editorContrainer != null) {
				GameObject editorCell = (GameObject)Instantiate (Resources.Load<GameObject> ("Editor/EditorCell"), position, Quaternion.identity);
				editorCell.name = "EditorCell_" + cell.Index;
				editorCell.transform.parent = editorContrainer.transform;
				
				EditorCell edCell = editorCell.GetComponent<EditorCell> ();
				edCell.SetCell (cell);
			}
		}
		generated = true;
	}

	public override void OnInspectorGUI()
	{
		MazeGenerator maze = (MazeGenerator)target;
		DrawDefaultInspector ();

		if (GUILayout.Button ("Generate editor maze")) {
			if (generated) {
				tipMessage = "Maze already generated. First clear it.";
				return;
			}

			maze.Generate ();

			//maze.transform.GetComponent<DebugDrawMazeCells>().UpdateCells();

			CreateEditorObjects(maze);

			//maze.GetComponent<DebugDrawEditorMazeCells>().UpdateCells();
			tipMessage = "New maze data generated!";
			SceneView.RepaintAll ();
		}

		if (GUILayout.Button ("Clear editor maze")) {
			GameObject container = GameObject.Find ("_EDITOR");
			for (int i = container.transform.childCount-1; i>=0; i--) {
				DestroyImmediate (container.transform.GetChild (i).gameObject);
			}

			//maze.GetComponent<DebugDrawEditorMazeCells>().UpdateCells();
			generated = false;
			tipMessage = "Maze cleared. All objects destroyed.";
		}

		if (GUILayout.Button ("Run test")) {
			Labyrinth lab = maze.gameObject.AddComponent<Labyrinth>();
			lab.CreateGround();
			lab.BuildWalls();
		}

		EditorGUILayout.HelpBox(tipMessage, MessageType.Info);
	}
}
