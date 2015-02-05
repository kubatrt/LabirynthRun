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
 * cell {
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
 	IGrid north; : index
 	IGrid south;
 	IGrid east;
 	IGrid west;
 }
*/

public class MazeLevelHeader
{
	public string name;
	public int width;
	public int height;
	public int numOfCells;
}

public class MazeEditorWindow : EditorWindow 
{
	private static string mazeName;
	private static int mazeWidth, mazeHeight = 0;
	private static string[] levelFiles;

	[MenuItem ("Maze/MazeEditor")]
	static void Initialize() 
	{
		MazeEditorWindow window = (MazeEditorWindow)EditorWindow.GetWindow (typeof (MazeEditorWindow));
		window.Focus();
		GetLevelFiles(out levelFiles);
	}

	static void WriteCell(BinaryWriter bw, MazeCell cell)
	{
		bw.Write(cell.IsStartCell);
		bw.Write(cell.IsFinishCell);
		bw.Write(cell.IsDeadEnd);
		bw.Write(cell.Visitted);
		bw.Write(cell.CrawlDistance);
		bw.Write(cell.NormalizedDistance);
		bw.Write((int)cell.Exits);
		bw.Write(cell.Index);
		bw.Write(cell.Position.x);
		bw.Write(cell.Position.y);
		bw.Write(cell.North.Index);
		bw.Write(cell.South.Index);
		bw.Write(cell.East.Index);
		bw.Write(cell.West.Index);
		Debug.Log(string.Format ("# BW.Write( {0} {1}", cell.Index, cell.Exits));
	}

	static void SaveLevel(string fileName, string levelName, int width, int height, List<MazeCell> cells)
	{
		try
		{			
			FileStream fout = new FileStream(fileName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
			BinaryWriter bw = new BinaryWriter(fout);

			bw.Write(levelName);
			bw.Write(width);
			bw.Write(height);
			bw.Write(cells.Count);

			int n=0;
			foreach(MazeCell cell in cells)
			{
				WriteCell(bw, cell); ++n;
			}
			
			bw.Close();
			Debug.Log (string.Format ("## Level written in {0} with {1} cells.", fileName, n));
		}
		catch(IOException e)
		{
			Debug.LogException(e);
		}
	}

	static void LoadLevel(string fileName, out List<MazeCell> cells)
	{
		cells = new List<MazeCell>();
		string name;
		int width, height;

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

	static void GetLevelFiles(out string[] files)
	{
		string projectPath = Application.dataPath + "/Levels";
		files = System.IO.Directory.GetFiles(projectPath);
		Debug.Log(projectPath);
	}

	private Vector2 scrollPosition;
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
			EditorGUILayout.BeginHorizontal();

			GUILayout.Label(filestr, EditorStyles.boldLabel);
			GUILayout.Button("Load");

			EditorGUILayout.EndHorizontal();
			//string fg = EditorGUILayout.ObjectField( filestr, go, typeof( GameObject ) );
		} 
		EditorGUILayout.EndScrollView();

		GUILayout.Button("Refresh");
	}
}