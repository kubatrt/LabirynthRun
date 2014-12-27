using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: flags
public enum ECellWalls
{
	North 	= 1,
	South 	= 2,
	East 	= 4,
	West 	= 8,
	None 	= 0,
};

[ExecuteInEditMode()]
public class DFS : MonoBehaviour 
{

	class MazeCell
	{
		public Vector2 position;

		// Walls
		public bool north = true;
		public bool south = true;
		public bool east = true;
		public bool west = true;


		public MazeCell()
		{
			position = Vector2.zero;
			SetAllWalls(false);
		}

		public MazeCell(float x, float y) {
			position = new Vector2(x, y);
			SetAllWalls(false);
		}

		public void SetAllWalls(bool val) {
			north = val;
			south = val;
			east = val;
			west = val;
		}

		public override string ToString ()
		{
			return string.Format("\t Cell [{0},{1}] walls N: {2} S: {3} E: {4} W: {5}", 
			                     position.x, position.y, north, south, east, west);
		}
	}


	public int Width = 2;
	public int Height = 3;
	int sizeX;
	int sizeY;

	Stack<Vector2> cellStack; // LIFO
	MazeCell[,] mazeCells;
	MazeCell currentCell;

	public bool IsGenerated = false;


	public Vector2 CellAt(int x, int y) {
		return mazeCells[x, y].position;
	}

	void Awake()
	{
		cellStack = new Stack<Vector2>();
		Generation();
	}

	void Update()
	{
		if(!IsGenerated) {
			Generation();
		}
	}

	void Generation()
	{
		Debug.Log("Generating maze...");
		cellStack.Clear();
		sizeX = Width;
		sizeY = Height;
		mazeCells = new MazeCell[sizeX, sizeY];
		int totalCells = sizeX * sizeY;

		for(int y = 0; y < sizeY; y++)
		{
			for(int x = 0; x < sizeX; x++)
			{
				mazeCells[x,y] = new MazeCell(x, y);	// set position related to place in array

				//Debug.Log ( string.Format ("mazeCells [{0},{1}] = {2}", x, y, mazeCells[x,y].ToString() ) );  
			}
		}

		// set random cell, set it current and set visitedCells at 1
		currentCell = mazeCells[0,0];
		cellStack.Push(currentCell.position);
		int visitedCells = 1;

		Debug.Log ( "START from + " + currentCell.ToString());

		while(visitedCells < totalCells) 
		{
			List<MazeCell> neighbors = GetNeighbors(currentCell);
			Debug.Log ("For = " + currentCell.ToString());

			if(neighbors.Count > 0) {
				// choose one at random
				MazeCell nextCell = neighbors[Random.Range(0, neighbors.Count)];
				Debug.Log ("Chosed nextCell=" + nextCell.ToString());

				// knock down the wall between it nad currCell
				if(nextCell.position.y > currentCell.position.y) // go north
				{
					mazeCells[ (int)currentCell.position.x, (int)currentCell.position.y].north = false;
					mazeCells[(int) nextCell.position.x, (int)nextCell.position.x].south = false;
					Debug.Log("north");
				}
				else if(nextCell.position.y < currentCell.position.y) // go south
				{
					mazeCells[ (int)currentCell.position.x, (int)currentCell.position.y].south = false;
					mazeCells[(int) nextCell.position.x, (int)nextCell.position.x].north = false;
					Debug.Log("south");
				}
				else if(nextCell.position.x > currentCell.position.x) // go east
				{
					mazeCells[ (int)currentCell.position.x, (int)currentCell.position.y].east = false;
					mazeCells[(int) nextCell.position.x, (int)nextCell.position.x].west = false;
					Debug.Log("east");

				}
				else if(nextCell.position.x < currentCell.position.x) // go west
				{
					mazeCells[ (int)currentCell.position.x, (int)currentCell.position.y].west = false;
					mazeCells[(int) nextCell.position.x, (int)nextCell.position.x].east = false;
					Debug.Log("west");
				}

				// set next

				currentCell = mazeCells[(int)nextCell.position.x, (int)nextCell.position.y];
				cellStack.Push(currentCell.position);
				visitedCells++;
				Debug.Log ("Visiting++: " + currentCell.ToString());
			}
			else {
				// pop the most recent cell entry of the cellStack
				cellStack.Pop(); 
				Debug.Log ("No naighbors, Pop()");

				currentCell = mazeCells[ (int)cellStack.Peek().x, (int)cellStack.Peek().y];

			}

			//visitedCells++;

		}//while*/

		Debug.Log("Generation complete " + visitedCells + " / " + totalCells);
		IsGenerated = true;
	}

	// Get unvisitted neighbors
	List<MazeCell> GetNeighbors(MazeCell cell)
	{
		List<MazeCell> neighbors = new List<MazeCell>();

		if(cell.position.y - 1 > 0) {
			MazeCell northNeighbor = mazeCells[(int)cell.position.x, (int)cell.position.y - 1];
			if (!cellStack.Contains(northNeighbor.position)) // unvisitted
				neighbors.Add(northNeighbor);
		}
		if(cell.position.y + 1 < sizeY) {
			MazeCell southNeighbor = mazeCells[(int)cell.position.x, (int)cell.position.y + 1];
			if (!cellStack.Contains(southNeighbor.position)) 
				neighbors.Add(southNeighbor);
		}
		if(cell.position.x + 1 < sizeX) {
			MazeCell eastNeighbor = mazeCells[(int)cell.position.x + 1, (int)cell.position.y];
			if (!cellStack.Contains(eastNeighbor.position))  
				neighbors.Add(eastNeighbor);
		}
		if(cell.position.x - 1 > 0) {
			cell.ToString();
			MazeCell westNeighbor = mazeCells[(int)cell.position.x - 1, (int)cell.position.y];
			if (!cellStack.Contains(westNeighbor.position))  
				neighbors.Add(westNeighbor);
		}
		Debug.Log ("Neigbors: " + neighbors.Count);
		return neighbors;
	}

	private void OnDrawGizmos()
	{
		// draw only with generated data
		if(!IsGenerated)
			return;

		for(int y = 0; y < sizeY; y++)
		{
			for(int x = 0; x < sizeX; x++)
			{
				MazeCell cell = mazeCells[x,y];
				
				Vector3 centroid = new Vector3(cell.position.x, 0f, cell.position.y);
				Vector3 topLeft = new Vector3(centroid.x - 0.5f, 0f, centroid.z - 0.5f);
				Vector3 topRight = new Vector3(centroid.x + 0.5f, 0f, centroid.z - 0.5f);
				Vector3 bottomLeft = new Vector3(centroid.x - 0.5f, 0f, centroid.z + 0.5f);
				Vector3 bottomRight = new Vector3(centroid.x + 0.5f, 0f, centroid.z + 0.5f);

				Gizmos.color = Color.cyan;
				if (cell.north)
					Gizmos.DrawLine(topLeft, topRight);
				if (cell.south)
					Gizmos.DrawLine(bottomLeft, bottomRight);
				if (cell.east)
					Gizmos.DrawLine(topRight, bottomRight);
				if (cell.west)
					Gizmos.DrawLine(topLeft, bottomLeft);


				Vector3 cellcentroid = new Vector3(cell.position.x, 0f, cell.position.y);
				Vector3 cellTopLeft = new Vector3(centroid.x - 0.25f, 0f, centroid.z - 0.25f);
				Vector3 cellTopRight = new Vector3(centroid.x + 0.25f, 0f, centroid.z - 0.25f);
				Vector3 cellBottomLeft = new Vector3(centroid.x - 0.25f, 0f, centroid.z + 0.25f);
				Vector3 cellBottomRight = new Vector3(centroid.x + 0.25f, 0f, centroid.z + 0.25f);

				if(cellStack.Contains(cell.position))
				{

					Gizmos.color = Color.red;
					Gizmos.DrawLine(cellTopLeft, cellTopRight);
					Gizmos.DrawLine(cellBottomLeft, cellBottomRight);
					Gizmos.DrawLine(cellTopRight, cellBottomRight);
					Gizmos.DrawLine(cellTopLeft, cellBottomLeft);
				}
			}
		}
	}

}
