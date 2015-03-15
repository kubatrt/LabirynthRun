	using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(EditorCell))]
public class EditorCellInspector : Editor 
{
	public SerializedProperty intIndex;

	bool eastExitLastState;
	bool westExitLastState ;
	bool northExitLastState;
	bool southExitLastState;


	void OnEnable()
	{
		EditorCell edCell = (EditorCell)target;
		eastExitLastState = edCell.cell.ExitEast;
		westExitLastState = edCell.cell.ExitWest;
		northExitLastState = edCell.cell.ExitNorth;
		southExitLastState = edCell.cell.ExitSouth;
	}

	public override void OnInspectorGUI()
	{
		EditorCell edCell = (EditorCell)target;
		DrawDefaultInspector();

		if(GUILayout.Button("Apply property change")) {
			if(edCell != null) {
				edCell.ApplyChanges();
				edCell.UpdateMaterials();
			}
		}

		if(GUILayout.Button ("Break cell exits")) {
			edCell.BreakExits();
		}

		if (GUI.changed)
		{
			if(eastExitLastState != edCell.cell.ExitEast) {
				edCell.AdjustEastExit();
				eastExitLastState = edCell.cell.ExitEast;
			}
			if(westExitLastState != edCell.cell.ExitWest) {
				edCell.AdjustWestExit();
				westExitLastState = edCell.cell.ExitWest;
			}	
			if(northExitLastState != edCell.cell.ExitNorth) {
				edCell.AdjustNorthExit();
				northExitLastState = edCell.cell.ExitNorth;
			}
			if(southExitLastState != edCell.cell.ExitSouth) {
				edCell.AdjustSouthExit();
				southExitLastState = edCell.cell.ExitSouth;
			}
		}

		EditorGUILayout.HelpBox("  S\nW  E\n  N\n" , MessageType.Info);
	}

}
