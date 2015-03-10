using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(EditorCell))]
public class EditorCellEditor : Editor 
{

	public override void OnInspectorGUI()
	{
		EditorCell cell = (EditorCell)target;
		DrawDefaultInspector();

		if(GUILayout.Button("Apply changes")) {
			if(cell != null)
				cell.ApplyChanges();
		}
		
		EditorGUILayout.HelpBox("Editor cell window" , MessageType.Info);

		GUILayout.Label("North");
		GUILayout.Label("South");
		GUILayout.Label("East");
		GUILayout.Label("West");

	}
}
