using UnityEngine;
using System.Collections;

public enum MazeCellExits
{
	None  = 0,
	North = 1,
	South = 2,
	East  = 4,
	West  = 8
}

public class Maze
{
	class MazeCell
	{
		public Vector2 position;
		public int index;

		public MazeCell(float x, float y)
		{
			position.x = x;
			position.y = y;
		}
	}

	public int width = 4;
	public int height = 4;

	public int Area {
		get { return width * height; }
	}

	public int seed = 3141592;

	public Vector2 initialPosition = Vector2.zero;

	MazeCell[] grid;

	// Use this for initialization
	void Initialize()
	{

	}
	
	void Generate()
	{

	}
}
