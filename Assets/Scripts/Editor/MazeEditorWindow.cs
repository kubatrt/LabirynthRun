using UnityEngine;
using System.Collections;

using UnityEngine;
using UnityEditor;
public class MazeEditorWindow : EditorWindow 
{

	[MenuItem ("Maze/MazeEditor")]
	static void Initialize() 
	{
		MazeEditorWindow window = (MazeEditorWindow)EditorWindow.GetWindow (typeof (MazeEditorWindow));
	}

	string mazeName;

	void OnGUI () 
	{
		GUILayout.Label ("Maze settings", EditorStyles.boldLabel);
		mazeName = EditorGUILayout.TextField ("Level name", mazeName);

		GUILayout.Button("Save");
		GUILayout.Button("Load");
		GUILayout.Button("Refresh");
	}
}