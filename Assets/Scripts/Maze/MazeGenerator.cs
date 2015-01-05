using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



//----------------------------------------------------------------------------------------------------------------------
public class MazeGenerator : MonoBehaviour
{
	public int Width = 8;
	public int Height = 8;
	public int Seed = 3141592;
	public bool Wrap = false;

	public Vector2 startPosition;
	[SerializeField] Vector2 finishPosition;

	Grid<MazeCell> maze;

	void Awake()
	{
		Generate();
	}

	public void Generate()
	{
		maze = new Grid<MazeCell>(Width, Height);
		Stack<GridPosition> visitedCells = new Stack<GridPosition>();
		// regular maze
		//UnityEngine.Random.seed = Seed;
		// random maze
		Seed = UnityEngine.Random.seed;


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
			cell.Visitted = true;
			cell.CrawlDistance = distance;

			// check valid exits
			MazeCellExits validExits = MazeCellExits.None;
			// pozycje != 0 i jest nieodwiedzona
			if((Wrap ||cellPos.y != 0) && !maze.GetCellAt(cellPos.x, cellPos.y - 1).Visitted) {
				validExits = validExits | MazeCellExits.North;
			}
			if((Wrap || cellPos.y != Height - 1) && !maze.GetCellAt(cellPos.x, cellPos.y + 1).Visitted) {
				validExits = validExits | MazeCellExits.South;
			}
			if((Wrap || cellPos.x != 0) && !maze.GetCellAt(cellPos.x - 1, cellPos.y).Visitted) { 
				validExits = validExits | MazeCellExits.West; 
			}
			if((Wrap || cellPos.x != Width - 1) && !maze.GetCellAt(cellPos.x + 1, cellPos.y).Visitted) {
				validExits = validExits | MazeCellExits.East;
			}


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
				cell.Index = cellIndex++;
			}
			else 
			{
				if(maxDistance < distance)
					maxDistance = distance;

				distance--;

				if(cell.TotalExits == 1)
					cell.IsDeadEnd = true;


				cellPos = visitedCells.Pop();
			}
			//Debug.Log (String.Format("ITER. #{2} distance {0} visited: {1}", distance, visitedCells.Count, cellIndex));
		}


		// normalize distance
		foreach(MazeCell cell in maze.CellsGrid)
			cell.NormalizedDistance = (float)cell.CrawlDistance / (float)maxDistance;

		// find first cell find maximum distance, make it finish
		foreach(MazeCell cell in maze.CellsGrid) {
			if(cell.CrawlDistance == maxDistance) {
				cell.IsFinishCell = true;
				finishPosition = new Vector2(cell.Position.x, cell.Position.y);	// debug
				break;
			}
		}

		Debug.Log( String.Format("# Maze.Generate(). MaxDistance: {0} Finish: [{1},{2}]", 
		                         maxDistance, finishPosition.x, finishPosition.y)); 
		//maze = mazeGrid;
		//return mazeGrid;
	}

	MazeCellExits GetRandomExit(MazeCellExits validExits)
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
		if (rand == exits.Count) rand--;
		
		return exits[rand];
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
