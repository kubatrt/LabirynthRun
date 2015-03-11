using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(EditorCell))]
public class EditorCellInspector : Editor 
{

	public override void OnInspectorGUI()
	{
		EditorCell cell = (EditorCell)target;
		DrawDefaultInspector();

		if(GUILayout.Button("Update changes")) {
			if(cell != null)
				cell.UpdateChanges();
		}
		if(GUILayout.Button("Apply changes")) {
			if(cell != null)
				cell.ApplyChanges();
		}
		
		EditorGUILayout.HelpBox("Editor cell window" , MessageType.Info);
	}
}
