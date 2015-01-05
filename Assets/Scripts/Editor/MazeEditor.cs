using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(MazeGenerator))]
public class MazeEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		//MazeGenerator maze = (MazeGenerator)target;
		DrawDefaultInspector();
	}
}
