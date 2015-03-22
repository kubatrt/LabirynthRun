	using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(EditorCell))]
public class EditorCellInspector : Editor 
{
	public SerializedProperty intIndex;

	private bool eastExitLastState;
	private bool westExitLastState ;
	private bool northExitLastState;
	private bool southExitLastState;


	void OnEnable()
	{
		EditorCell edCell = (EditorCell)target;
		SetLastExitStates(edCell);
		//Debug.Log(edCell.name + "OnEnable");
	}

	void OnDisable()
	{
		//EditorCell edCell = (EditorCell)target;
		//Debug.Log(edCell.name + "OnDisable");
	}

	void SetLastExitStates(EditorCell edCell)
	{
		northExitLastState = edCell.cell.ExitNorth;
		southExitLastState = edCell.cell.ExitSouth;
		eastExitLastState = edCell.cell.ExitEast;
		westExitLastState = edCell.cell.ExitWest;
	}

	void UpdateLastExitStates(EditorCell edCell)
	{
		if(northExitLastState != edCell.cell.ExitNorth) {
			edCell.AdjustNorthExit();
			northExitLastState = edCell.cell.ExitNorth;
		}
		if(southExitLastState != edCell.cell.ExitSouth) {
			edCell.AdjustSouthExit();
			southExitLastState = edCell.cell.ExitSouth;
		}
		if(eastExitLastState != edCell.cell.ExitEast) {
			edCell.AdjustEastExit();
			eastExitLastState = edCell.cell.ExitEast;
		}
		if(westExitLastState != edCell.cell.ExitWest) {
			edCell.AdjustWestExit();
			westExitLastState = edCell.cell.ExitWest;
		}
	}

	public override void OnInspectorGUI()
	{
		EditorCell edCell = (EditorCell)target;
		DrawDefaultInspector();

		if(GUILayout.Button ("Break all cell exits")) {
			edCell.BreakExits();
			UpdateLastExitStates(edCell);
			return;
		}
		
		if (GUI.changed)
		{
			UpdateLastExitStates(edCell);
			Debug.Log ("GUI.changed!");
		}

		EditorGUILayout.HelpBox(
			"  S\n" +
			"W + E\n" +
			"  N\n" +
			"[W][S][A][D] for quick cell exits editing [B] for break exits", MessageType.Info);
	}


	void OnSceneGUI() 
	{
		EditorCell edCell = (EditorCell)target;
		Event e = Event.current;

		switch (e.type)
		{
			case EventType.keyDown:
			{				
				if (Event.current.keyCode == KeyCode.S)
				{
					edCell.cell.ExitNorth = !edCell.cell.ExitNorth;
					edCell.AdjustNorthExit(); 
					northExitLastState = edCell.cell.ExitNorth;
				}

				if (Event.current.keyCode == KeyCode.W)
				{
					edCell.cell.ExitSouth = !edCell.cell.ExitSouth;
					edCell.AdjustSouthExit();
					southExitLastState = edCell.cell.ExitSouth;
				}

				if (Event.current.keyCode == KeyCode.D)
				{
					edCell.cell.ExitEast = !edCell.cell.ExitEast; 
					edCell.AdjustEastExit();
					eastExitLastState = edCell.cell.ExitEast;
				}
				if (Event.current.keyCode == KeyCode.A)
				{
					edCell.cell.ExitWest = !edCell.cell.ExitWest;
					edCell.AdjustWestExit(); 
					westExitLastState = edCell.cell.ExitWest;
					
				}
				if(Event.current.keyCode == KeyCode.B) 
				{
					edCell.BreakExits();
					UpdateLastExitStates(edCell);
				}
				break;
			}
		}
	}

}
