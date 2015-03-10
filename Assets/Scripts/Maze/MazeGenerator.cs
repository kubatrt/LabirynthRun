using UnityEngine;
using System;
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

	private Grid<MazeCell> maze;


	void Awake()
	{
		if(GameManager.Instance != null)
		{
			Width = GameManager.Instance.MazeWidth;
			Height = GameManager.Instance.MazeHeight;
			Debug.Log("MazeGenerator.Awake size CHANGED");
		}
		Debug.Log ("MazeGenerator.Awake size NOT CHANGED");
	}

	public void Generate()
	{
		maze = new Grid<MazeCell>(Width, Height);
		Stack<GridPosition> visitedCells = new Stack<GridPosition>();

		if(Seed != 0)
			UnityEngine.Random.seed = Seed;

		int distance = 0;
		int maxDistance = 0;
		int cellIndex = 0;

		// starting position
		GridPosition cellPos = maze.WrapCoordinates((int)startPosition.x, (int)startPosition.y);
		maze.GetCellAt(cellPos).IsStartCell = true;
		visitedCells.Push(cellPos);

		// iterate trough all cells
		while(visitedCells.Count > 0)
		{
			MazeCell cell = maze.GetCellAt(cellPos);
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
				cell = maze.GetCellAt(cellPos);
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
		foreach(MazeCell cell in maze.CellsGrid)
			cell.NormalizedDistance = (float)cell.CrawlDistance / (float)maxDistance;

		// find maximum distance, make it finish
		foreach(MazeCell cell in maze.CellsGrid) {
			if(cell.CrawlDistance == maxDistance) {
				cell.IsFinishCell = true;
				//Debug.Log( String.Format("# Maze.Generate(). MaxDistance: {0} Finish: [{1},{2}] Cells: {3}", 
				//                         maxDistance, cell.Position.x, cell.Position.y, maze.Area ));
				break;
			}
		}	
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

		if((Wrap ||cellPos.y != 0) && !maze.GetCellAt(cellPos.x, cellPos.y - 1).IsVisitted) {
			validExits = validExits | MazeCellExits.North;
		}
		if((Wrap || cellPos.y != Height - 1) && !maze.GetCellAt(cellPos.x, cellPos.y + 1).IsVisitted) {
			validExits = validExits | MazeCellExits.South;
		}
		if((Wrap || cellPos.x != 0) && !maze.GetCellAt(cellPos.x - 1, cellPos.y).IsVisitted) { 
			validExits = validExits | MazeCellExits.West; 
		}
		if((Wrap || cellPos.x != Width - 1) && !maze.GetCellAt(cellPos.x + 1, cellPos.y).IsVisitted) {
			validExits = validExits | MazeCellExits.East;
		}
		return validExits;
	}

	public void FindSolution()
	{
		/*List<GridPosition> solution = new List<GridPosition>();
		if(maze == null)
			return;

		Stack<GridPosition> visitedCells = new Stack<GridPosition>();
		
		int distance = 0;
		int maxDistance = 0;
		int cellIndex = 0;
		
		// starting position
		GridPosition cellPos = maze.WrapCoordinates((int)startPosition.x, (int)startPosition.y);
		visitedCells.Push(cellPos);

		bool endReached = false;

		while(!endReached)
		{
			MazeCell cell = maze.GetCellAt(cellPos);
			MazeCellExits exits = GetValidExits(cellPos);
			//MazeCellExits goExit = MazeCellExits.None;

			if((exits & MazeCellExits.North) == MazeCellExits.North) {
				cellPos = new GridPosition(cellPos.x, cellPos.y - 1);
			} 
			else if ((exits & MazeCellExits.West) == MazeCellExits.West)
			{
				cellPos = new GridPosition(cellPos.x - 1, cellPos.y);
			}
			else if ((exits & MazeCellExits.East) == MazeCellExits.East)
			{
				cellPos = new GridPosition(cellPos.x + 1, cellPos.y);
			}
			else if ((exits & MazeCellExits.South) == MazeCellExits.South)
			{
				cellPos = new GridPosition(cellPos.x, cellPos.y + 1);
			}
		}
		*/
	}

	public List<MazeCell> GetCells()
	{
		List<MazeCell> cells = new List<MazeCell>();
		for(int x = 0; x < Width; x++)
		{
			for(int y = 0; y < Height; y++)
			{
				cells.Add(maze.GetCellAt(x,y));
			}
		}
		return cells;
	}

	public MazeCell GetCellAt(int x, int y)
	{
		return maze.GetCellAt(x, y);
	}

	public MazeCell GetCellAtIndex(int index)
	{
		return maze.GetCellAtIndex(index);
	}

	public static Vector3 GridToWorld(GridPosition cellPos, float offset, float height)
	{
		return new Vector3((float)cellPos.x * offset, height, (float)cellPos.y * offset);
	}
}
