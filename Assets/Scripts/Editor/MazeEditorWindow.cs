using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MazeEditorWindow : EditorWindow 
{
	static MazeEditorWindow staticWindow;
	
	[MenuItem ("Tools/MazeEditor")]
	static void Initialize() 
	{
		// MazeEditorWindow window 
		MazeEditorWindow staticWindow  = (MazeEditorWindow)EditorWindow.GetWindow (typeof (MazeEditorWindow));
		staticWindow.Focus();
		//GetListLevelFiles(out levelFiles);
	}


	/*class CellRelation
	{
		int NorthIndex;
		int SouthIndex;
		int EastIndex;
		int WestIndex;
	}

	class MazeLevelHeader
	{
		public string name;
		public int width;
		public int height;
		public int numOfCells;
	}*/

	//private static string mazeName;
	//private static int mazeWidth, mazeHeight = 0;
	private static List<String> levelFiles = new List<String>();

	private Vector2 scrollPosition;


	static void WriteCell(BinaryWriter bw, MazeCell cell)
	{
		bw.Write(cell.IsStartCell);
		bw.Write(cell.IsFinishCell);
		bw.Write(cell.IsDeadEnd);
		bw.Write(cell.IsVisitted);
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

	static void ReadCell(BinaryReader br, out MazeCell cell)
	{
		cell = new MazeCell();
		cell.IsStartCell = br.ReadBoolean();
		cell.IsFinishCell = br.ReadBoolean ();
		cell.IsDeadEnd = br.ReadBoolean();
		cell.IsVisitted = br.ReadBoolean();
		cell.CrawlDistance = br.ReadInt32();
		cell.NormalizedDistance = br.ReadInt32();
		cell.Exits = (MazeCellExits)br.ReadInt32();
		cell.Position.x = br.ReadInt32();
		cell.Position.y = br.ReadInt32();

		// build relations
		/*int NorthIndex = br.ReadInt32();
		int SouthIndex = br.ReadInt32();
		int EastIndex = br.ReadInt32();
		int WestIndex = br.ReadInt32();*/
	}

	static void BuildRelations(out List<MazeCell> cells)
	{
		cells = new List<MazeCell>();

		// TODO
	}

	static void SaveLevel(string mazeName, int mazeWidth, int mazeHeight, List<MazeCell> cells)
	{
		try
		{			
			FileStream fout = new FileStream(mazeName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
			BinaryWriter bw = new BinaryWriter(fout);

			// header
			bw.Write(mazeName);
			bw.Write(mazeWidth);
			bw.Write(mazeHeight);
			bw.Write(cells.Count);

			int n=0;
			foreach(MazeCell cell in cells)
			{
				WriteCell(bw, cell); 
				n++;
			}
			
			bw.Close();
			Debug.Log (string.Format ("## Level written in {0} with {1} cells.", mazeName, n));
		}
		catch(IOException e)
		{
			Debug.LogException(e);
		}
	}

	static void LoadLevel(string fileName, out string mazeName, out int mazeHeight, out int mazeWidth, out List<MazeCell> cells)
	{
		cells = new List<MazeCell>();
		mazeName = ""; 
		mazeWidth = 0; 
		mazeHeight = 0;

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
				//MazeCell cell;
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

	string objectName;
	string lastSelected;
	bool repaint = false;

	void Update()
	{
		// selected object on scene
		if (Selection.activeGameObject && Selection.activeGameObject.name != lastSelected)
		{
			objectName = Selection.activeGameObject.name;
			this.Repaint();

			lastSelected = Selection.activeGameObject.name;
			repaint = true;
		}
		else if (Selection.activeGameObject == null && repaint == true)
		{
			objectName = "Please Select an Object";
			this.Repaint();

			repaint = false;
			lastSelected = "";
		}
	}


	readonly string levelsDir = Application.dataPath + "/Levels/";
	string mazeName;

	void OnGUI () 
	{
		GUILayout.Label ("Maze settings", EditorStyles.boldLabel);
		mazeName = EditorGUILayout.TextField ("Level name", mazeName);

		if(GUILayout.Button("Save"))
		{
			MazeGenerator mazeGen = FindObjectOfType<MazeGenerator>();
			mazeGen.SaveToFile(levelsDir + mazeName + ".maze");
			RefreshFiles();
		}

		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
		foreach(string file in levelFiles )
		{
			EditorGUILayout.BeginHorizontal();

			GUILayout.Label(System.IO.Path.GetFileName(file), EditorStyles.boldLabel);
			if(GUILayout.Button("Load",  GUILayout.Width(48)))
			{
				MazeGenerator maze = FindObjectOfType<MazeGenerator>();
				maze.LoadFromFile(file);
				// find MazeGeneratorInspector
				MazeGeneratorInspector[] editors = (MazeGeneratorInspector[]) Resources.FindObjectsOfTypeAll(typeof(MazeGeneratorInspector));
				if (editors.Length > 0)
				{
					editors[0].CreateEditorObjects(maze);
				}
			}

			EditorGUILayout.EndHorizontal();
			//string fg = EditorGUILayout.ObjectField( filestr, go, typeof( GameObject ) );
		} 
		EditorGUILayout.EndScrollView();

		if(GUILayout.Button("Refresh")) {
			RefreshFiles();
		}
		GUILayout.Space (5);
		GUILayout.Label ("Selected object: " + objectName);
		GUILayout.Space (5);
	}

	private void RefreshFiles()
	{
		levelFiles.Clear();
		string projectPath = Application.dataPath + "/Levels/";
		string[] files = System.IO.Directory.GetFiles(projectPath, "*.maze", SearchOption.TopDirectoryOnly);
		levelFiles.AddRange(files);
		Debug.Log("GetLevelFiles: " + projectPath);
	}
}

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

