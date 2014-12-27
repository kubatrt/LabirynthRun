using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


// | zapalanie bitu bits = bits & BitToCheck
// & sprawdzenie bitu isBit = bits | BitToCheck
public enum MazeCellExits
{
	None  = 0,	// 0000
	North = 1,	// 0001
	South = 2,	// 0011
	East  = 4,	// 0111
	West  = 8	// 1111
}


//----------------------------------------------------------------------------------------------------------------------
public struct GridLocation
{
	public int x;
	public int y;

	public GridLocation(int x, int y)
	{
		this.x = x;
		this.y = y;
	}
}

//----------------------------------------------------------------------------------------------------------------------
public interface IGridCell
{
	int Index { get; set;}
	GridLocation Location { get; set; }
	IGridCell North {get; set;}
	IGridCell South {get; set;}
	IGridCell East 	{get; set;}
	IGridCell West {get; set;}
}

//----------------------------------------------------------------------------------------------------------------------
class Grid<T> where T : IGridCell, new()
{
	public int Width;
	public int Height;

	public int Area
	{
		get { return Width * Height; }
	}

	public T[] CellGrid;

	public Grid(int width, int height)
	{
		Width = width;
		Height = height;
		CellGrid = new T[Width * Height];
		Initialize();
	}
	
	public int GridToCellIndex(float x, float y)
	{
		x = (x % Width + Width) % Width;
		y = (y % Height + Height) % Height;
		return (int)(x + y * Width);
	}
	
	public T GetCellAt(int x, int y)
	{
		return CellGrid[ GridToCellIndex(x, y)];
	}
	
	public T GetCellAt(GridLocation loc)
	{
		return CellGrid[ GridToCellIndex(loc.x, loc.y) ];
	}

	public GridLocation WrapCoordinates(int x, int y)
	{
		x = x % Width;
		y = y % Height;
		return new GridLocation(x, y);
	}

	void Initialize()
	{
		Debug.Log ("Initializing...");
		for(int x = 0; x < Width; x++)
		{
			for(int y = 0; y < Height; y++)
			{
				int index = GridToCellIndex(x,y);
				T cell = new T();
				cell.Location = new GridLocation(x, y);
				cell.Index = index;
				CellGrid[index] = cell;
				Debug.Log( cell.ToString() );
			}
		}

		Debug.Log ("Linking...");
		// link cells
		for(int x = 0; x < Width; x++)
		{
			for(int y = 0; y < Height; y++)
			{

				T cell = CellGrid[GridToCellIndex(x,y)];			
				cell.North = GetCellAt(x, y +1);
				cell.South = GetCellAt(x, y -1);
				cell.East = GetCellAt(x + 1, y);
				cell.West = GetCellAt(x - 1, y);
				// co jesli cell jest po za granicami, < 0 lub > Size, GridToCellIndex zaokragla
			}
		}
	}

}


//----------------------------------------------------------------------------------------------------------------------
public class MazeCell : IGridCell
{
	#region Interface implementation
	int index;
	public int Index { 
		get { return index; }
		set { index = value; }
	}

	GridLocation location;
	public GridLocation Location { 
		get { return location; } 
		set { location = value; } 
	}

	IGridCell north;
	public IGridCell North {
		get { return north; }
		set { north = value; }
	}

	IGridCell south;
	public IGridCell South {
		get { return south; }
		set { south = value; }
	}

	IGridCell east;
	public IGridCell East 	{
		get { return east;} 
		set { east = value; }
	}

	IGridCell west;
	public IGridCell West {
		get { return west; }
		set { west = value; }
	}

	#endregion

	#region Exits form cell

	public MazeCellExits Exits = MazeCellExits.None;


	public bool NoExits
	{
		get { return Exits == MazeCellExits.None; }
		set { Exits = MazeCellExits.None; }
	}

	public bool ExitNorth
	{
		get { return (Exits & MazeCellExits.North) == MazeCellExits.North; }
		set { Exits = Exits | MazeCellExits.North; }
	}

	public bool ExitSouth
	{
		get { return (Exits & MazeCellExits.South) == MazeCellExits.South; }
		set { Exits = Exits | MazeCellExits.South; }
	}

	public bool ExitEast
	{
		get { return (Exits & MazeCellExits.East) == MazeCellExits.East; }
		set { Exits = Exits | MazeCellExits.East; }
	}

	public bool ExitWest
	{
		get { return (Exits & MazeCellExits.West) == MazeCellExits.West; }
		set { Exits = Exits | MazeCellExits.West; }
	}
	
	public int TotalExits
	{
		get {
			int result = 0;
			if(ExitNorth) result++;
			if(ExitSouth) result++;
			if(ExitEast) result++;
			if(ExitWest) result++;
			return result;
		}
	}

	#endregion

	#region Other fields

	// An arbitrary weighting value that indicates the cell's distance from the origin cell.
	//public int CrawlDistance = 0;

	// A normalized weighting value that indicates the cell's distance
	// from the origin cell in relation to the rest of the maze.
	//public float NormalizedDistance = 0f;
	
	public bool IsStartCell = false;
	//public bool IsDeadEnd = false;
	//public bool Flagged = false;

	#endregion

	public override string ToString ()
	{
		return String.Format ( "Cell({0} Location [{1},{2}] Exits [ N: {3} S: {4} E: {5} W: {6}]",
		                      index, location.x, location.y, ExitNorth, ExitSouth, ExitEast, ExitWest);
	}
}

//----------------------------------------------------------------------------------------------------------------------
//[ExecuteInEditMode()]
public class MazeGenerator : MonoBehaviour
{
	public int Width = 4;
	public int Height = 4;
	public int Seed = 3141592;

	Grid<MazeCell> maze;
	Stack<MazeCell> visitedCells = new Stack<MazeCell>();


	void Start()
	{
		maze = new Grid<MazeCell>(Width, Height);
		maze.GetCellAt(0,0).IsStartCell = true;

		DebugMazeCells debugMaze = GetComponent<DebugMazeCells>();
		for(int x = 0; x < Width; x++)
		{
			for(int y = 0; y < Height; y++)
			{
				debugMaze.AddCell( maze.GetCellAt(x,y));
			}
		}
		Debug.Log ("Started.");
	}

	public void Generate()
	{
		Debug.Log ("Generating...");
		int visitedCells = 1;

		if(visitedCells < maze.Area)
		{

		}
	}

	void Update() 
	{

	}

}
