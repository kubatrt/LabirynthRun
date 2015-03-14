	using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(EditorCell))]
public class EditorCellInspector : Editor 
{
	public SerializedProperty intIndex;

	void OnEnable()
	{

	}

	public override void OnInspectorGUI()
	{
		EditorCell cell = (EditorCell)target;
		DrawDefaultInspector();

		if(GUILayout.Button("Apply changes")) {
			if(cell != null) {
				cell.ApplyChanges();
				cell.UpdateChanges();
			}
		}
		
		EditorGUILayout.HelpBox("Editor cell window" , MessageType.Info);
	}

}
