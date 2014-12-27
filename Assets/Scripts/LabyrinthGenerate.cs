using UnityEngine;
using System.Collections;

public class LabyrinthGenerate : MonoBehaviour 
{

	public GameObject	wallPrefab;

	// int xPos, yPos;

	// TODO:
	//public int width, height;
	//public int corridorWidth = 3;
	//public int wallWidth = 1;
	//public int wallHeight = 4;

	DFS maze;


	void Start () 
	{
		maze = new DFS();
		//CreateGrid ();
	}
	
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			//createAtRandomCellPos();
		}
	}

	public int step = 1;
	void CreateGrid()
	{
		//int beginX = width * 2;
		//int beginY = height * 2;

		/*for(int y = 4; y < maze.height; y++)
		{
			for(int x = 0; x < maze.width; x++)
			{
				GameObject wall = (GameObject)Instantiate(wallPrefab);
				wall.transform.position = new Vector3( maze.CellAt(x,y).x, maze.CellAt(x,y).y, 0.5f );
				wall.transform.parent = this.transform;
			}
		}*/

		// horizontal
		/*for(int y = -beginY; y < beginY; y++)
		{
			for(int x = -beginX; x < beginX ; x++)
			{
				GameObject wall = (GameObject)Instantiate(wallPrefab);
				wall.transform.position = new Vector3(y + wall.transform.localScale.x / 2f, 
				                                      wall.transform.localScale.y / 2f , 
				                                      x + wall.transform.localScale.z / 2f);
				wall.transform.parent = this.transform;
			}
		}*/

		// vertical PO CO?
		/*for(int y = -startY; y <= startY; y+=step)
		{
			for(int x = -startX; x <= startX; x++)
			{
				//if(y%2 == 0 && j%4 != 0 || y%2 != 0 && (j%2 != 0 && j%4 == 0))
				//if(y == 0 && 
				{ 
					GameObject wall = (GameObject)Instantiate(wallPrefab);
					wall.transform.position = new Vector3(y+0.5f, wall.transform.localScale.y / 2f ,x+0.5f);
					wall.transform.parent = this.transform;
				}
			}
		}*/
	}
	/*
	int getCellXPos(int x)
	{
		int n = width;
		if(x <= n)
		{
			int xFirstCell;
			//xFirstCell = -((n / 2) * 4 - 2);
			xFirstCell = -(n*2 - 2);
			x = xFirstCell + (x - 1)*4;
			return x;
		}
		else
		{
			Debug.LogError("CELL POSITION ERROR");
			return 0;
		}
	}

	int getCellYPos(int y) 
	{
		int n = height;
		if(y <= n)
		{
			int yFirstCell;
			//yFirstCell = -((n / 2) * 4 - 2);
			yFirstCell = -(n*2 - 2);
			y = yFirstCell + (y - 1)*4;
			return y;
		}
		else
		{
			Debug.LogError("CELL POSITION ERROR");
			return 0;
		}
	}

	int randomCellXPoss()
	{
		int max = width+1;
		int x = Random.Range (1, max);
		return getCellXPos(x);
	}

	int randomCellYPoss()
	{
		int max = height+1;
		int y = Random.Range (1, max);
		return getCellYPos(y);
	}

	void createAtRandomCellPos()
	{
		GameObject obj = (GameObject)Instantiate (Resources.Load ("Finish"));
		int x = randomCellYPoss ();
		int z = randomCellXPoss ();
		obj.transform.position = new Vector3(x+0.5f,2,z+0.5f);
	}
	*/
}