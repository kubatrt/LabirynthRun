using UnityEngine;
using System;
using System.Collections;


//----------------------------------------------------------------------------------------------------------------------
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
public class MazeCell : IGridCell
{
	#region Interface implementation
	int index;
	public int Index { 
		get { return index; }
		set { index = value; }
	}
	
	GridPosition location;
	public GridPosition Position { 
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
	public int CrawlDistance = 0;
	
	// A normalized weighting value that indicates the cell's distance
	// from the origin cell in relation to the rest of the maze.
	public float NormalizedDistance = 0f;
	
	public bool IsStartCell = false;
	public bool IsFinishCell = false;
	public bool IsDeadEnd = false;
	public bool Flagged = false;
	
	#endregion
	
	public override string ToString ()
	{
		return String.Format ( "Cell({0} Location [{1},{2}] Exits [ N: {3} S: {4} E: {5} W: {6}]",
		                      index, location.x, location.y, ExitNorth, ExitSouth, ExitEast, ExitWest);
	}
}
