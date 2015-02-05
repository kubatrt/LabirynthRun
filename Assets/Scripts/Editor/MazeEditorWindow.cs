using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/***************************
 * 	LEVEL FILE FORMAT
	name
	width
	height
	numOfCells
	cells[0...numOfCell] : cell
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
 	IGrid north; : index
 	IGrid south;
 	IGrid east;
 	IGrid west;
 }
*****************************/

public class MazeLevelHeader
{
	public string name;
	public int width;
	public int height;
	public int numOfCells;
}

public class MazeEditorWindow : EditorWindow 
{
	//private static string mazeName;
	//private static int mazeWidth, mazeHeight = 0;
	private static string[] levelFiles;

	private Vector2 scrollPosition;

	static MazeEditorWindow staticWindow;

	[MenuItem ("Maze/MazeEditor")]
	static void Initialize() 
	{
		// MazeEditorWindow window 
		MazeEditorWindow staticWindow  = (MazeEditorWindow)EditorWindow.GetWindow (typeof (MazeEditorWindow));
		staticWindow.Focus();
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
		Debug.Log(string.Format ("# WriteCell( {0} {1}", cell.Index, cell.Exits));
	}

	class CellRelation
	{
		int NorthIndex;
		int SouthIndex;
		int EastIndex;
		int WestIndex;
	}

	static void ReadCell(BinaryReader br, out MazeCell cell)
	{
		cell = new MazeCell();
		cell.IsStartCell = br.ReadBoolean();
		cell.IsFinishCell = br.ReadBoolean ();
		cell.IsDeadEnd = br.ReadBoolean();
		cell.Visitted = br.ReadBoolean();
		cell.CrawlDistance = br.ReadInt32();
		cell.NormalizedDistance = br.ReadInt32();
		cell.Exits = (MazeCellExits)br.ReadInt32();
		cell.Position.x = br.ReadInt32();
		cell.Position.y = br.ReadInt32();
		// build relations
		int NorthIndex = br.ReadInt32();
		int SouthIndex = br.ReadInt32();
		int EastIndex = br.ReadInt32();
		int WestIndex = br.ReadInt32();
	}

	static void BuildRelations(out List<MazeCell> cells)
	{
		cells = new List<MazeCell>();
	}

	static void SaveLevel(string fileName, string mazeName, int mazeWidth, int mazeHeight, List<MazeCell> cells)
	{
		try
		{			
			FileStream fout = new FileStream(fileName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
			BinaryWriter bw = new BinaryWriter(fout);

			// header
			bw.Write(mazeName);
			bw.Write(mazeWidth);
			bw.Write(mazeHeight);
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

	static void LoadLevel(string fileName, out string mazeName, out int mazeHeight, out int mazeWidth, out List<MazeCell> cells)
	{
		cells = new List<MazeCell>();
		mazeName = ""; mazeWidth = 0; mazeHeight = 0;

		try
		{
			FileStream fin = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
			BinaryReader br = new BinaryReader(fin);
			br.BaseStream.Seek(0, SeekOrigin.Begin);

			mazeName = br.ReadString();
			mazeWidth = br.ReadInt32();
			mazeHeight = br.ReadInt32();
			int numOfCells = br.ReadInt32();

			for(int i=0; i < numOfCells; ++i)
			{
				MazeCell cell;
			}
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
		Debug.Log("GetLevelFiles: " + projectPath);
	}


	//string[] filesArray = new string[] { "Level example 1", "Level example 2", "Level example 3", "Level example 4", "Level example 5" };
	string mazeName;
	void OnGUI () 
	{
		GUILayout.Label ("Maze settings", EditorStyles.boldLabel);
		mazeName = EditorGUILayout.TextField ("Level name", mazeName);

		GUILayout.Button("Save");
		GUILayout.Button("Load");

		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
		foreach( string filestr in levelFiles )
		{
			EditorGUILayout.BeginHorizontal();

			GUILayout.Label(filestr, EditorStyles.boldLabel);
			GUILayout.Button("Load",  GUILayout.Width(48));

			EditorGUILayout.EndHorizontal();
			//string fg = EditorGUILayout.ObjectField( filestr, go, typeof( GameObject ) );
		} 
		EditorGUILayout.EndScrollView();

		GUILayout.Button("Refresh");
	}
}