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
		public bool north;
		public bool south;
		public bool east;
		public bool west;

		public bool visited = false;
		//public bool IsStartCell = false;
		//public bool IsEndCell = false;
		//public bool IsDeadEnd = false;

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
			return string.Format("Cell position[{0},{1}] walls N: {2} S: {3} E: {4} W: {5}", 
			                     position.x, position.y, north, south, east, west);
		}
	}

	public int width = 8;
	public int height = 8;
	int sizeX;
	int sizeY;

	Stack<MazeCell> cellStack; // LIFO
	MazeCell currentCell;
	//MazeCell nextCell;
	MazeCell[,] mazeCells;

	public bool generated = false;

	public Vector2 CellAt(int x, int y) {
		return mazeCells[x, y].position;
	}
	public bool CellNorthWallAt(int x, int y) {
		return mazeCells[x, y].north;
	}
	public bool CellSouthAt(int x, int y) {
		return mazeCells[x, y].south;
	}	
	public bool CellEastAt(int x, int y) {
		return mazeCells[x, y].east;
	}	
	public bool CellWestAt(int x, int y) {
		return mazeCells[x, y].west;
	}




	void Awake()
	{
		//currentCell = new MazeCell();
		//nextCell = new MazeCell();
		cellStack = new Stack<MazeCell>();

		
		Generation();
	}

	void Update()
	{
		if(!generated) {
			Generation();
		}
	}

	void Generation()
	{
		Debug.Log("Generating maze...");
		cellStack.Clear();
		sizeX = width;
		sizeY = height;
		mazeCells = new MazeCell[sizeX, sizeY];

		for(int y = 0; y < sizeY; y++)
		{
			for(int x = 0; x < sizeX; x++)
			{
				mazeCells[x,y] = new MazeCell(x, y);	// set position related to place in array
				mazeCells[x,y].SetAllWalls(true);

				// consider maze boundries cells
				if(x == 0)
					mazeCells[x,y].west = false;
				if(x == sizeX - 1)
					mazeCells[x,y].east = false;
				if(y == 0)
					mazeCells[x,y].north = false;
				if(y == sizeY - 1)
					mazeCells[x,y].south = false;

				//Debug.Log ( string.Format ("mazeCells [{0},{1}] = {2}", x, y, mazeCells[x,y].ToString() ) );  
			}
		}

		// do tests passes?
		/*if(!GetNeighbors_4x4_UT()) {
			Debug.LogError ("YOU SHALL NOT PASS!!!");
			return;
		}*/

		
		// set random cell, set it current and set visitedCells at 1
		//setCellPos (currentCell, Random.Range(1,sizeX+1), Random.Range(1,sizeY+1));
		//currentCell = mazeCells[1,1];
		currentCell = mazeCells[ Random.Range(0,sizeX-1), Random.Range(0,sizeY-1)];

		//cellStack.Push(currentCell);
		int visitedCells = 1;

		int totalCells = sizeX * sizeY;
		
		while(visitedCells < totalCells) 
		{
			currentCell.visited = true;
			// find all neighboors of currentCell with all wall intact
			List<MazeCell> neighbors = GetNeighbors(currentCell);

			if(neighbors.Count > 0) {
				// choose one at random
				MazeCell nextCell = neighbors[Random.Range(0, neighbors.Count)];

				// knock down the wall between it nad currCell
				if(nextCell.position.y > currentCell.position.y) // go north
				{
					currentCell.north = false;
					nextCell.south = false;
				}
				if(nextCell.position.y < currentCell.position.y) // go south
				{
					currentCell.south = false;
					nextCell.north = false;
				}
				if(nextCell.position.x > currentCell.position.x) // go east
				{
					currentCell.east = false;
					nextCell.west = false;
				}
				if(nextCell.position.x < currentCell.position.x) // go west
				{
					currentCell.west = false;
					nextCell.east = false;
				}

				// set next
				currentCell = mazeCells[(int)nextCell.position.x, (int)nextCell.position.y];
				//cellStack.Push(currentCell);
				//visitedCells++;

			}
			//else {
			//	cellStack.Pop();
			//	currentCell = cellStack.Peek();
				// pop the most recent cell entry of the cellStack

			//}

			visitedCells++;

		}//while

		Debug.Log("Generation complete");
		generated = true;
	}

	bool GetNeighbors_UT()
	{
		// representation in memory: 4x4
		//[x:0,y:0]
		// 1, 2, 3, 4
		// 5, 6, 7, 8
		// 6, 7, 8, 9
		//10,11,12,13 [x:3,y:3]
		// TEST CELLS: 1, 6, 12, 13
	
		bool result = true;

		if(GetNeighbors(mazeCells[0,0]).Count == 2)
			Debug.Log ("OK!");
		else
			result = false;

		if(GetNeighbors(mazeCells[1,1]).Count == 4)
			Debug.Log ("OK!");
		else
			result = false;

		if(GetNeighbors(mazeCells[2,3]).Count == 3)
			Debug.Log ("OK!");
		else
			result = false;

		if(GetNeighbors(mazeCells[3,3]).Count == 2)
			Debug.Log ("OK!");
		else
			result = false;

		return result;
	}

	// Get unvisitted neighbors
	List<MazeCell> GetNeighbors(MazeCell cell)
	{
		List<MazeCell> neighbors = new List<MazeCell>();

		if(/*cell.position.y - 1 >= 0 &&*/ cell.north) {
			MazeCell northNeighbor = mazeCells[(int)cell.position.x, (int)cell.position.y - 1];
			if (!northNeighbor.visited) 
				neighbors.Add(northNeighbor);
		}
		if(/*cell.position.y + 1 < sizeY &&*/ cell.south) {
			MazeCell southNeighbor = mazeCells[(int)cell.position.x, (int)cell.position.y + 1];
			if (!southNeighbor.visited) 
				neighbors.Add(southNeighbor);
		}
		if(/*cell.position.x + 1 < sizeX &&*/ cell.east) {
			MazeCell eastNeighbor = mazeCells[(int)cell.position.x + 1, (int)cell.position.y];
			if (!eastNeighbor.visited)  
				neighbors.Add(eastNeighbor);
		}
		if(/*cell.position.x - 1 >= 0 &&*/ cell.west) {
			cell.ToString();
			MazeCell westNeighbor = mazeCells[(int)cell.position.x - 1, (int)cell.position.y];
			if (!westNeighbor.visited)  
				neighbors.Add(westNeighbor);
		}


		return neighbors;
	}

	private void OnDrawGizmos()
	{
		// draw only with generated data
		if(!generated)
			return;

		Vector3 frame1 = new Vector3(0.5f, 0.2f, 0.5f);
		Vector3 idScale = new Vector3(0.45f, 0.15f, 0.45f);

		Debug.Log ("OnDrawGizmos Gizmos");
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


				// Draw edge lines
				Gizmos.color = Color.cyan;
				if (!cell.north)
					Gizmos.DrawLine(topLeft, topRight);
				if (!cell.south)
					Gizmos.DrawLine(bottomLeft, bottomRight);
				if (!cell.east)
					Gizmos.DrawLine(topRight, bottomRight);
				if (!cell.west)
					Gizmos.DrawLine(topLeft, bottomLeft);
			}
		}
	}
/*
	void setCellPos(CellPos cell, int x, int y)
	{
		cell.x = x;
		cell.y = y;
	}
	
	CellWalls fromCellPosToCellWalls(CellPos cell, CellWalls[,] maze)
	{
		
		return maze [cell.x,cell.y];
	}
*/
}
