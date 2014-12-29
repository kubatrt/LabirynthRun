using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Labyrinth : MonoBehaviour 
{
	public GameObject wallPrefab;

	public float scale = 0.5f;
	public float offset = 1f;
	public float wallHeight = 0f;
	//public float corridorSize = 1f;

	MazeGenerator maze;
	List<MazeCell> cells = new List<MazeCell>();

	GameObject wallContainer;
	GameObject triggerContainer;


	void Start () 
	{
		maze = GetComponent<MazeGenerator>();
		cells = maze.GetCells(); 
		CreateContainers();
		BuildWalls();
	}

	void CreateContainers()
	{
		if(GameObject.Find("_Walls") == null) {
			wallContainer = new GameObject("_Walls");
			wallContainer.transform.parent = transform;
		}
		if(GameObject.Find ("_Triggers") == null) {
			triggerContainer = new GameObject("_Triggers");
			triggerContainer.transform.parent = transform;
		}
	}

	//   TL TMC TR
	//    o--o--o  --o--o  --o--o
	//    |  |  |       |       |
	// ML o--A--o +  B  o +  C  |  + ...
	//    |  |  |       |       |
	//    o--o--o  --o--o  --o--o
	//   BL  BMC BR
	// o - cell 8 draw positions , C - centroid, cell center position, cs - corridor size )--(
	// 1. generate positions for all cells
	// 2. remove duplicates
	// 3. instantiate wall objects 
	void BuildWalls()
	{
		HashSet<Vector3> wallPositions = new HashSet<Vector3>();
		// debug only
		int duplicates = 0;	

		foreach(MazeCell cell in cells) 
		{
			Vector3 centroid = new Vector3((float)cell.Position.x * offset, wallHeight, (float)cell.Position.y * offset);
			
			Vector3 topLeft			 	= new Vector3(centroid.x - scale, centroid.y, centroid.z - scale);
			Vector3 topMiddleCenter 	= new Vector3(centroid.x, wallHeight, centroid.z - scale);
			Vector3 topRight 			= new Vector3(centroid.x + scale, centroid.y, centroid.z - scale);
			Vector3 middleLeft 			= new Vector3(centroid.x - scale, centroid.y, centroid.z);
			Vector3 middleRight 		= new Vector3(centroid.x + scale, centroid.y, centroid.z);
			Vector3 bottomLeft 			= new Vector3(centroid.x - scale, centroid.y, centroid.z + scale);
			Vector3 bottomMiddleCenter 	= new Vector3(centroid.x, centroid.y, centroid.z + scale);
			Vector3 bottomRight 		= new Vector3(centroid.x + scale, centroid.y, centroid.z + scale);

			// hashSet provides no duplicates
			if(!wallPositions.Add(topLeft)) duplicates++;
			if(!wallPositions.Add(topRight)) duplicates++;
			if(!wallPositions.Add(bottomLeft)) duplicates++;
			if(!wallPositions.Add(bottomRight)) duplicates++;

			// if there is no exit, build wall
			if(!cell.ExitNorth || cell.Position.y == 0 ) wallPositions.Add(topMiddleCenter); 
			if(!cell.ExitSouth || cell.Position.y == maze.Height - 1 ) wallPositions.Add(bottomMiddleCenter);
			if(!cell.ExitEast || cell.Position.x == maze.Width - 1 ) wallPositions.Add(middleRight);
			if(!cell.ExitWest || cell.Position.x == 0 ) wallPositions.Add(middleLeft);

		}
		Debug.Log ("Build completed:" + wallPositions.Count + " Duplicates:" + duplicates);

		if(wallPrefab == null) {
			Debug.LogError("No wall prefab selected!");
			return;
		}

		foreach(Vector3 pos in wallPositions) 
		{
			GameObject wallObject = (GameObject)GameObject.Instantiate(wallPrefab, pos, Quaternion.identity);
			wallObject.transform.parent = wallContainer.transform;
		}
	}

	void BuildGround()
	{
		// TODO: generate plane mesh adjusted to maze size and position
	}

}