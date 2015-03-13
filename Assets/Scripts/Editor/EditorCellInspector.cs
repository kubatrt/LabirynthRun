	using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(EditorCell))]
public class EditorCellInspector : Editor 
{
	public SerializedProperty enumTypeMask;
	public SerializedProperty intIndex;

	void OnEnable()
	{
		enumTypeMask = serializedObject.FindProperty ("cell.Exits");
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
