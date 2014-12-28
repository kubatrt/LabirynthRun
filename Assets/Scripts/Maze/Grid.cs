using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


//----------------------------------------------------------------------------------------------------------------------
public class GridPosition
{
	public int x;
	public int y;
	
	public GridPosition(int x, int y)
	{
		this.x = x;
		this.y = y;
	}
}


//----------------------------------------------------------------------------------------------------------------------
public interface IGridCell
{
	int 			Index { get; set;}
	GridPosition 	Position { get; set; }
	
	IGridCell 	North {get; set;}
	IGridCell 	South {get; set;}
	IGridCell 	East {get; set;}
	IGridCell 	West {get; set;}
}


//----------------------------------------------------------------------------------------------------------------------
public class Grid<T> where T : IGridCell, new()
{
	public int Width;
	public int Height;
	
	public int Area
	{
		get { return Width * Height; }
	}
	
	public T[] CellsGrid;


	public Grid(int width, int height)
	{
		Width = width;
		Height = height;
		CellsGrid = new T[Width * Height];
		Initialize();
	}
	
	public int GridToCellIndex(float x, float y)
	{
		// Wrapping from 0 to Size
		x = (x % Width + Width) % Width; 
		y = (y % Height + Height) % Height;

		return (int)(x + y * Width);
	}
	
	public T GetCellAt(int x, int y)
	{
		return CellsGrid[ GridToCellIndex(x, y)];
	}
	
	public T GetCellAt(GridPosition loc)
	{
		return CellsGrid[ GridToCellIndex(loc.x, loc.y) ];
	}
	
	public GridPosition WrapCoordinates(int x, int y)
	{
		x = x % Width;
		y = y % Height;
		return new GridPosition(x, y);
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
				cell.Position = new GridPosition(x, y);
				cell.Index = index;
				CellsGrid[index] = cell;
				// Debug.Log( cell.ToString() );
			}
		}
		
		// Link neighbor cells 
		for(int x = 0; x < Width; x++)
		{
			for(int y = 0; y < Height; y++)
			{
				T cell = CellsGrid[GridToCellIndex(x,y)];			
				cell.North = GetCellAt(x, y +1);
				cell.South = GetCellAt(x, y -1);
				cell.East = GetCellAt(x + 1, y);
				cell.West = GetCellAt(x - 1, y);
			}
		}
	}
	
}