using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



//----------------------------------------------------------------------------------------------------------------------
//[ExecuteInEditMode()]
public class MazeGenerator : MonoBehaviour
{
	public int Width = 4;
	public int Height = 4;
	public int Seed = 3141592;
	public bool Wrap = false;

	public Vector2 startPosition;
	public Vector2 finishPosition;

	Grid<MazeCell> maze;

	void Awake()
	{
		maze = Generate();
		AddDebugDraw();
	}

	public Grid<MazeCell> Generate()
	{
		Debug.Log ("Generating...");
		Grid<MazeCell> mazeGrid = new Grid<MazeCell>(Width, Height);
		Stack<GridPosition> visitedCells = new Stack<GridPosition>();
		//Random.seed = Seed;
		int distance = 0;
		int maxDistance = 0;

		// set finish point
		GridPosition cellFinishPos = mazeGrid.WrapCoordinates((int)finishPosition.x, (int)finishPosition.y);
		mazeGrid.GetCellAt(cellFinishPos).IsFinishCell = true; 

		// initial point
		GridPosition cellPos = mazeGrid.WrapCoordinates((int)startPosition.x, (int)startPosition.y);
		mazeGrid.GetCellAt(cellPos).IsStartCell = true;
		visitedCells.Push(cellPos);

		while(visitedCells.Count > 0)
		{
			MazeCell cell = mazeGrid.GetCellAt(cellPos);
			cell.Flagged = true;
			cell.CrawlDistance = distance;

			// check valid exits, Wrap seems doesnt work as expect...
			MazeCellExits validExits = MazeCellExits.None;
			if((Wrap || cellPos.x != 0) && !mazeGrid.GetCellAt(cellPos.x - 1, cellPos.y).Flagged) { 
				validExits = validExits | MazeCellExits.West; 
			}
			if((Wrap || cellPos.x != Width - 1) && !mazeGrid.GetCellAt(cellPos.x + 1, cellPos.y).Flagged) {
				validExits = validExits | MazeCellExits.East;
			}
			if((Wrap || cellPos.y != 0) && !mazeGrid.GetCellAt(cellPos.x, cellPos.y + 1).Flagged) {
				validExits = validExits | MazeCellExits.South;
			}
			if((Wrap || cellPos.y != Height - 1) && !mazeGrid.GetCellAt(cellPos.x, cellPos.y - 1).Flagged) {
				validExits = validExits | MazeCellExits.North;
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
					cellPos = new GridPosition(cellPos.x, cellPos.y -1);
					exit = MazeCellExits.South;
				}
				else if (exit == MazeCellExits.South)
				{
					cellPos = new GridPosition(cellPos.x, cellPos.y + 1);
					exit = MazeCellExits.North;
				}
				else if (exit == MazeCellExits.West)
				{
					cellPos = new GridPosition(cellPos.x - 1, cellPos.y);
					exit = MazeCellExits.East;
				}
				else if (exit == MazeCellExits.East)
				{
					cellPos = new GridPosition(cellPos.x + 1, cellPos.y);
					exit = MazeCellExits.West;
				}

				// exit back
				cell = mazeGrid.GetCellAt(cellPos);
				cell.Exits = cell.Exits | exit;
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
		}

		// normalize distance
		//foreach(MazeCell cell in mazeGrid.CellGrid)
		//	cell.NormalizedDistance = (float)cell.CrawlDistance / (float)maxDistance;

		return mazeGrid;
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
		if (rand == exits.Count) 
			rand--;
		
		return exits[rand];
	}

	void ExampleDebug()
	{
		maze = new Grid<MazeCell>(Width, Height);
		maze.GetCellAt(0,0).IsStartCell = true;
	}

	void AddDebugDraw()
	{
		gameObject.AddComponent<DebugDrawMazeCells>();
		DebugDrawMazeCells debugMaze = GetComponent<DebugDrawMazeCells>();
		for(int x = 0; x < Width; x++)
		{
			for(int y = 0; y < Height; y++)
			{
				debugMaze.AddCell( maze.GetCellAt(x,y));
			}
		}
	}
}
