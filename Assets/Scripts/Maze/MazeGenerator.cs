using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;


[ExecuteInEditMode()]
//----------------------------------------------------------------------------------------------------------------------
public class MazeGenerator : MonoBehaviour
{
	public int Width;
	public int Height;
	public int Seed = 3141592;
	public bool Wrap = false;
	public Vector2 startPosition;

	public Grid<MazeCell> mazeGrid;

	void Awake()
	{
		if(GameManager.Instance != null)
		{
			Width = GameManager.Instance.MazeWidth;
			Height = GameManager.Instance.MazeHeight;
			Debug.Log("MazeGenerator.Awake GameManager size CHANGED");
		}
		Debug.Log ("MazeGenerator.Awake GameManager size NOT CHANGED");
	}

	public void Generate()
	{
		//if (mazeGrid != null)
		//	Debug.LogWarning ("Maze already exists, data will be overwritten");

		mazeGrid = new Grid<MazeCell>(Width, Height);

		if(Seed != 0)
			UnityEngine.Random.seed = Seed;


		int distance = 0;
		int maxDistance = 0;
		int cellIndex = 0;

		// starting position
		Stack<GridPosition> visitedCells = new Stack<GridPosition>();
		GridPosition cellPos = mazeGrid.WrapCoordinates((int)startPosition.x, (int)startPosition.y);
		mazeGrid.GetCellAt(cellPos).IsStartCell = true;
		visitedCells.Push(cellPos);

		// iterate trough all cells
		while(visitedCells.Count > 0)
		{
			MazeCell cell = mazeGrid.GetCellAt(cellPos);
			cell.IsVisitted = true;
			cell.CrawlDistance = distance;

			// check valid exits
			MazeCellExits validExits = GetValidExits(cellPos);

			if(validExits != MazeCellExits.None)
			{
				distance++;
				visitedCells.Push(cellPos);

				// choose random exit from the available ones
				MazeCellExits exit = GetRandomExit(validExits);
				cell.Exits = cell.Exits | exit;

				// select next tile
				if(exit == MazeCellExits.North) {
					cellPos = new GridPosition(cellPos.x, cellPos.y - 1);
					exit = MazeCellExits.South;
				}
				else if (exit == MazeCellExits.South) {
					cellPos = new GridPosition(cellPos.x, cellPos.y + 1);
					exit = MazeCellExits.North;
				}
				else if (exit == MazeCellExits.West) {
					cellPos = new GridPosition(cellPos.x - 1, cellPos.y);
					exit = MazeCellExits.East;
				}
				else if (exit == MazeCellExits.East) {
					cellPos = new GridPosition(cellPos.x + 1, cellPos.y);
					exit = MazeCellExits.West;
				}

				// exit back
				cell = mazeGrid.GetCellAt(cellPos);
				cell.Exits = cell.Exits | exit;
				// set index
				cell.Index = ++cellIndex;
			}
			else 
			{
				if(maxDistance < distance)
					maxDistance = distance;

				distance--;

				// set dead end
				if(cell.TotalExits == 1)
					cell.IsDeadEnd = true;

				cellPos = visitedCells.Pop();
			}
			//Debug.Log (String.Format("ITER. #{2} distance {0} visited: {1}", distance, visitedCells.Count, cellIndex));
		}


		// normalize distance
		foreach(MazeCell cell in mazeGrid.CellsGrid)
			cell.NormalizedDistance = (float)cell.CrawlDistance / (float)maxDistance;

		// find maximum distance, make it finish
		foreach(MazeCell cell in mazeGrid.CellsGrid) {
			if(cell.CrawlDistance == maxDistance) {
				cell.IsFinishCell = true;
				//Debug.Log( String.Format("# Maze.Generate(). MaxDistance: {0} Finish: [{1},{2}] Cells: {3}", 
				//                         maxDistance, cell.Position.x, cell.Position.y, maze.Area ));
				break;
			}
		}	

		Debug.Log ("MazeGenerator.Generate()");
	}

	private MazeCellExits GetRandomExit(MazeCellExits validExits)
	{
		List<MazeCellExits> exits = new List<MazeCellExits>();
		if ((validExits & MazeCellExits.North) 	== MazeCellExits.North) 
			exits.Add(MazeCellExits.North);
		if ((validExits & MazeCellExits.South) 	== MazeCellExits.South) 
			exits.Add(MazeCellExits.South);
		if ((validExits & MazeCellExits.East) 	== MazeCellExits.East) 	
			exits.Add(MazeCellExits.East);
		if ((validExits & MazeCellExits.West) 	== MazeCellExits.West) 	
			exits.Add(MazeCellExits.West);
		
		int rand = (int)(UnityEngine.Random.value * exits.Count);
		if (rand == exits.Count) 
			rand--;
		
		return exits[rand];
	}

	private MazeCellExits GetValidExits(GridPosition cellPos)
	{
		MazeCellExits validExits = MazeCellExits.None;

		if((Wrap ||cellPos.y != 0) && !mazeGrid.GetCellAt(cellPos.x, cellPos.y - 1).IsVisitted) {
			validExits = validExits | MazeCellExits.North;
		}
		if((Wrap || cellPos.y != Height - 1) && !mazeGrid.GetCellAt(cellPos.x, cellPos.y + 1).IsVisitted) {
			validExits = validExits | MazeCellExits.South;
		}
		if((Wrap || cellPos.x != 0) && !mazeGrid.GetCellAt(cellPos.x - 1, cellPos.y).IsVisitted) { 
			validExits = validExits | MazeCellExits.West; 
		}
		if((Wrap || cellPos.x != Width - 1) && !mazeGrid.GetCellAt(cellPos.x + 1, cellPos.y).IsVisitted) {
			validExits = validExits | MazeCellExits.East;
		}
		return validExits;
	}

	public List<MazeCell> GetCells()
	{
		List<MazeCell> cells = new List<MazeCell>();
		for(int x = 0; x < Width; x++)
		{
			for(int y = 0; y < Height; y++)
			{
				cells.Add(mazeGrid.GetCellAt(x,y));
			}
		}
		return cells;
	}

	public void SetCellAt(int x, int y, MazeCell cell)
	{
		mazeGrid.CellsGrid[mazeGrid.GridToCellIndex(x, y)] = cell;
	}

	public void SetCellAtIndex(int index, MazeCell newCell)
	{
		int i = 0;
		foreach(MazeCell cell in mazeGrid.CellsGrid)
		{
			if(cell.Index == index)
				break;
			++i;
		}

		mazeGrid.CellsGrid [i] = newCell;
		//Debug.Log("SetCellAtIndex [" + i + "] newCell: " + newCell.ToString());
	}

	public MazeCell GetCellAt(int x, int y)
	{
		return mazeGrid.GetCellAt(x, y);
	}

	public MazeCell GetCellAtIndex(int index)
	{
		return mazeGrid.GetCellAtIndex(index);
	}

	public static Vector3 GridToWorld(GridPosition cellPos, float offset, float height)
	{
		return new Vector3((float)cellPos.x * offset, height, (float)cellPos.y * offset);
	}

	public void SaveToFile(string path)
	{
		using(FileStream fileStream = File.Create(path))
		{
			BinaryWriter bw = new BinaryWriter(fileStream);

			bw.Write(mazeGrid.Width);
			bw.Write(mazeGrid.Height);

			for(int i=0; i < mazeGrid.Area; ++i)
			{
				bw.Write( MCSerialization.SerializeToString( mazeGrid.CellsGrid[i] ));
				//Debug.Log( mazeGrid.CellsGrid[i].ToString());
			}
		}
		Debug.Log ("SaveToFile: " + path);
	}
	
	public void LoadFromFile(string path)
	{
		using(FileStream fileStream = File.OpenRead(path))
		{
			BinaryReader br = new BinaryReader(fileStream);

			Width = br.ReadInt32 ();
			Height = br.ReadInt32();
			mazeGrid = new Grid<MazeCell>(Width, Height);

			for(int i=0; i < mazeGrid.Area; ++i)
			{
				mazeGrid.CellsGrid[i] = (MazeCell) MCSerialization.DeserializeFromString( br.ReadString());
				//Debug.Log(mazeGrid.CellsGrid[i].ToString());
			}
		}
		Debug.Log ("LoadFromFile: " + path);
	}

	public bool Validate()
	{
		bool startCell = false;
		bool finishCell = false;
		foreach(MazeCell cell in mazeGrid.CellsGrid) 
		{
			if(cell.IsFinishCell) {
				if(cell.TotalExits > 0) 
					finishCell = true; 
			}
			else if(cell.IsStartCell) {
				if(cell.TotalExits == 1) 
					startCell = true;
			}

			if(cell.TotalExits == 1)
				cell.IsDeadEnd = true;
			else
				cell.IsDeadEnd = false;
		}

		return finishCell && startCell;
	}
}
