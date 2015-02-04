using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/* LEVEL FILE FORMAT
 * name
 * width
 * height
 * numOfCells
 * cells[0...numOfCell] : cell
 * 
 cell {
 	bool IsStartCell
	bool IsFinishCell
 	bool IsDeadEnd
 	bool Visitted	
 	int CrawlDistance;	
 	float NormalizedDistance;
 	int Exits; [enum

	int index;
	locationX; locationY;

 	[relation]	
 	IGrid north;
 	IGrid south;
 	IGrid east;
 	IGrid west;
 }
*/

public class MazeEditorWindow : EditorWindow 
{
	private string mazeName;

	[MenuItem ("Maze/MazeEditor")]
	static void Initialize() 
	{
		MazeEditorWindow window = (MazeEditorWindow)EditorWindow.GetWindow (typeof (MazeEditorWindow));
	}

	static void SaveLevel(string fileName)
	{
		try
		{
			
			FileStream fout = new FileStream(fileName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
			BinaryWriter bw = new BinaryWriter(fout);
			
			//  write data...
			// Write()
			
			bw.Close();
		}
		catch(IOException e)
		{
			Debug.LogException(e);
		}
	}

	static void LoadLevel(string fileName)
	{
		try
		{
			FileStream fin = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
			BinaryReader br = new BinaryReader(fin);
			br.BaseStream.Seek(0, SeekOrigin.Begin);

			// read data...
			// ReadString() ReadInt32() ReadBoolean() ReadSingle()

			br.Close();
		}
		catch(IOException e)
		{
			Debug.LogException(e);
		}

	}


	Vector2 scrollPosition;
	string[] filesArray = new string[5] { "Level1 : first one : 6 : 6", "Level2", "Level3", "Level4", "Level5" } ;

	void OnGUI () 
	{
		GUILayout.Label ("Maze settings", EditorStyles.boldLabel);
		mazeName = EditorGUILayout.TextField ("Level name", mazeName);

		GUILayout.Button("Save");
		GUILayout.Button("Load");

		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
		foreach( string filestr in filesArray )
		{
			// propertyField
			EditorGUILayout.SelectableLabel(filestr);
			//string fg = EditorGUILayout.ObjectField( filestr, go, typeof( GameObject ) );
		} 
		EditorGUILayout.EndScrollView();

		GUILayout.Button("Refresh");
	}
}