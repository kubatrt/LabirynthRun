using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(MazeGenerator))]
public class MazeGeneratorEditor : Editor 
{

	public override void OnInspectorGUI()
	{
		MazeGenerator maze = (MazeGenerator)target;
		DrawDefaultInspector();

		if(GUILayout.Button("Generate Editor maze"))
		{
			maze.Generate();
			//maze.transform.GetComponent<DebugDrawMazeCells>().UpdateCells();

			GameObject editorContrainer = GameObject.Find("_EDITOR");

			foreach(MazeCell cell in maze.GetCells())
			{

				Vector3 position = new Vector3(cell.Position.x, 0f, cell.Position.y);
				if(editorContrainer != null) {
					GameObject editorCell = (GameObject)Instantiate(Resources.Load<GameObject>("EditCell"), position, Quaternion.identity);
					editorCell.name = "EditorCell-" + cell.Index;
					editorCell.transform.parent = editorContrainer.transform;
					editorCell.GetComponent<EditCell>().SetCell(cell);
				}
			}

			SceneView.RepaintAll();
		}
		if(GUILayout.Button("Clear editor maze"))
		{
			GameObject editorContrainer = GameObject.Find("_EDITOR");
			foreach(Transform t in editorContrainer.transform)
			{
				DestroyImmediate(t.gameObject);
			}
		}

		EditorGUILayout.HelpBox("Custom Maze Generator script editor", MessageType.Info);
	}
}
