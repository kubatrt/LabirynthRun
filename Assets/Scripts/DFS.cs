using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * DFS:
	create a CellStack (LIFO) to hold a list of cell locations  
	set TotalCells = number of cells in grid  
	choose a cell at random and call it CurrentCell  
	set VisitedCells = 1  
		while VisitedCells < TotalCells 
		find all neighbors of CurrentCell with all walls intact   
			if one or more found 
				choose one at random  
				knock down the wall between it and CurrentCell  
				push CurrentCell location on the CellStack  
				make the new cell CurrentCell  
				add 1 to VisitedCellselse 
				pop the most recent cell entry off the CellStack  
				make it CurrentCell
			endIf
		endWhile
*/

public class DFS : MonoBehaviour 
{
	
	struct CellPos
	{
		public int xPos;
		public int yPos;
	}
	
	struct CellWalls
	{
		public bool northWall;
		public bool southWall;
		public bool eastWall;
		public bool westWall;
	};
	
	public int xCellSize = 4;
	public int yCellSize = 4;

	/*
	void labyrinthGenerator()
	{
		// maze size
		int totalCells = xCellSize * yCellSize;
		// LIFO
		CellPos[] cellStack;
		cellStack = new CellPos[totalCells];
		//  cell's fields
		CellPos currentCell = new CellPos();
		CellPos nextCell = new CellPos();
		int visitedCells;
		// create fiction maze as 2d table and set all walls true (create grid)
		CellWalls[,] maze;
		maze = new CellWalls[xCellSize,yCellSize];
		for(int i=0; i<yCellSize; i++)
		{
			for(int j=0; j<xCellSize; j++)
			{
				maze[i,j].northWall = true;
				maze[i,j].southWall = true;
				maze[i,j].westWall = true;
				maze[i,j].eastWall = true;
			}
		}
		
		// set random cell, set it current and set visitedCells at 1
		setCellPos (currentCell, Random.Range(1,xCellSize+1), Random.Range(1,yCellSize+1));
		visitedCells = 1;
		
		// main algoritm loop
		while(visitedCells < totalCells) 
		{
			// find all neighboors of currentCell with all wall intact
			List<CellPos> neighboors = getNeighboors(currentCell, maze);
			int neighboorsNum = neighboors.Count;
			if(neighboorsNum > 0)
			{
				// choose one at random
				int choice = Random.Range(1,neighboorsNum+1);
				switch(choice)
				{
				case 1:
					nextCell = neighboors[0];
					break;
				case 2:
					nextCell = neighboors[1];
					break;
				case 3:
					nextCell = neighboors[2];
					break;
				case 4:
					nextCell = neighboors[3];
					break;
				}
				//knock down the wall between it nad currCell
				if(nextCell.xPos == currentCell.xPos && nextCell.yPos < currentCell.yPos)
				{
					fromCellPosToCellWalls(currentCell, maze).northWall = false;
					fromCellPosToCellWalls(nextCell, maze).southWall = false;
				}
				else if(nextCell.xPos == currentCell.xPos && nextCell.yPos > currentCell.yPos)
				{
					fromCellPosToCellWalls(currentCell, maze).southWall = false;
					fromCellPosToCellWalls(nextCell, maze).northWall = false;
				}
				else if(nextCell.xPos > currentCell.xPos && nextCell.yPos == currentCell.yPos)
				{
					fromCellPosToCellWalls(currentCell, maze).eastWall = false;
					fromCellPosToCellWalls(nextCell, maze).westWall = false;
				}
				else if(nextCell.xPos < currentCell.xPos && nextCell.yPos == currentCell.yPos)
				{
					fromCellPosToCellWalls(currentCell, maze).westWall = false;
					fromCellPosToCellWalls(nextCell, maze).eastWall = false;
				}
				// next steps of algorithm
				
			}
			else
			{
				// pop the most recent cell entry of the cellStack
				// make it currentCell
			}
			
			
		}
		
	}
	
	void setCellPos(CellPos cell, int x, int y)
	{
		cell.xPos = x;
		cell.yPos = y;
	}
	
	CellWalls fromCellPosToCellWalls(CellPos cell, CellWalls[,] maze)
	{
		
		return maze [cell.xPos,cell.yPos];
	}
	
	List<CellPos> getNeighboors(CellPos currentCell, CellWalls maze) // return list of neighboors cells positions
	{
		List<CellPos> neighboors = new List<CellPos>();
		
		CellPos northNeighboorPos, southNeighboorPos, eastNeighboorPos, westNeighboorPos;
		CellWalls northNeighboor, southNeighboor, eastNeighboor, westNeighboor;
		
		northNeighboorPos = setCellPos(currentCell, currentCell.xPos, currentCell.yPos - 1);
		//northNeigboor = fromCellPosToCellWalls (northNeighboorPos, maze);  -- alternative
		northNeighboor = maze [northNeighboorPos.xPos,northNeighboorPos.yPos];
		
		southNeighboorPos = setCellPos(currentCell, currentCell.xPos, currentCell.yPos + 1);
		southNeighboor = maze [southNeighboorPos.xPos,southNeighboorPos.yPos];
		
		eastNeighboorPos = setCellPos(currentCell, currentCell.xPos + 1, currentCell.yPos);
		eastNeighboor = maze [eastNeighboorPos.xPos,eastNeighboorPos.yPos];
		
		westNeighboorPos = setCellPos(currentCell, currentCell.xPos - 1, currentCell.yPos);
		westNeighboor = maze [westNeighboorPos.xPos,westNeighboorPos.yPos];
		
		if(northNeighboor.northWall && northNeighboor.southWall && northNeighboor.eastWall && northNeighboor.westWall)
		{
			neighboors.Add(northNeighboorPos);
		}
		else if(southNeighboor.northWall && southNeighboor.southWall && southNeighboor.eastWall && southNeighboor.westWall)
		{
			neighboors.Add(southNeighboorPos);
		}
		else if(eastNeighboor.northWall && eastNeighboor.southWall && eastNeighboor.eastWall && eastNeighboor.westWall)
		{
			neighboors.Add(eastNeighboorPos);
		}
		else if(westNeighboor.northWall && westNeighboor.southWall && westNeighboor.eastWall && westNeighboor.westWall)
		{
			neighboors.Add(westNeighboorPos);
		}
		
		return neighboors;
	}
	*/
}
