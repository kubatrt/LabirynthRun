using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


//----------------------------------------------------------------------------------------------------------------------
public class SortByX : IComparer<Vector3>
{
	public int Compare( Vector3 a, Vector3 b)
	{
		if(a.x > b.x) return 1;
		else if (a.x < b.x) return -1;
		else return 0;
	}
}

//----------------------------------------------------------------------------------------------------------------------
public class SortByZ : IComparer<Vector3>
{
	public int Compare( Vector3 a, Vector3 b)
	{
		if(a.z > b.z) return 1;
		else if (a.z < b.z) return -1;
		else return 0;
	}
}

//----------------------------------------------------------------------------------------------------------------------
class Vector3ListHelper
{
	public static float Position_FindMaxX(List<Vector3> positions) {
		float max = 0;
		foreach(Vector3 pos in positions) {
			if(pos.x > max)
				max = pos.x;
		}
		return max;
	}

	public static float Position_FindMinX(List<Vector3> positions) {
		float min = 0;
		foreach(Vector3 pos in positions) {
			if(pos.x < min)
				min = pos.x;
		}
		return min;
	}

	public static float Position_FindMaxZ(List<Vector3> positions) {
		float max = 0;
		foreach(Vector3 pos in positions) {
			if(pos.z > max)
				max = pos.z;
		}
		return max;
	}

	public static float Position_FindMinZ(List<Vector3> positions) {
		float min = 0;
		foreach(Vector3 pos in positions) {
			if(pos.z < min)
				min = pos.z;
		}
		return min;
	}
}

//----------------------------------------------------------------------------------------------------------------------
public class Labyrinth : MonoBehaviour 
{

	public GameObject wallPrefab;
	public GameObject wall2Prefab;
	public GameObject worldPrefab;
	public GameObject triggerPrefab;
	public GameObject finishPrefab;
	public GameObject playerPrefab;

	public float size = 1f;	// prefab size
	public float distance = 2f; // world distance between walls after scaling

	public float scale = 0.5f;
	public float offset = 1f;
	public float wallHeight = 0f;

	public int debugObjectCount = 0;

	MazeGenerator maze;
	List<MazeCell> cells = new List<MazeCell>();

	GameObject wallContainer;
	GameObject triggerContainer;

	// containers for positions
	HashSet<Vector3> wallsWorldPos;
	List<Vector3> 	cellWorldPos;

	// TODO: build separate container for cells positions or modify exiting MazeCell's property

	void Start () 
	{
		maze = GetComponent<MazeGenerator>();
		cells = maze.GetCells(); 
		CreateContainers();
		BuildWalls();
		//CreateObjects();
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

	void CreateObjects()
	{
		// TODO: spawn other objects
		foreach(Vector3 pos in cellWorldPos) 
		{
			GameObject worldObject = (GameObject)GameObject.Instantiate(worldPrefab, pos, Quaternion.identity);
			worldObject.transform.parent = wallContainer.transform;	
		}
	}


	void BuildWalls()
	{
		wallsWorldPos = new HashSet<Vector3>();
		int duplicates = 0;	// debug only

		foreach(MazeCell cell in cells) 
		{

			Vector3 centroid = new Vector3((float)cell.Position.x * offset, wallHeight, (float)cell.Position.y * offset);
			//cellWorldPos.Add(centroid);

			Vector3 topLeft			 	= new Vector3(centroid.x - scale, centroid.y, centroid.z - scale);
			Vector3 topMiddleCenter 	= new Vector3(centroid.x, wallHeight, centroid.z - scale);
			Vector3 topRight 			= new Vector3(centroid.x + scale, centroid.y, centroid.z - scale);
			Vector3 middleLeft 			= new Vector3(centroid.x - scale, centroid.y, centroid.z);
			Vector3 middleRight 		= new Vector3(centroid.x + scale, centroid.y, centroid.z);
			Vector3 bottomLeft 			= new Vector3(centroid.x - scale, centroid.y, centroid.z + scale);
			Vector3 bottomMiddleCenter 	= new Vector3(centroid.x, centroid.y, centroid.z + scale);
			Vector3 bottomRight 		= new Vector3(centroid.x + scale, centroid.y, centroid.z + scale);

			// hashSet provides no duplicates
			if(!wallsWorldPos.Add(topLeft)) duplicates++;
			if(!wallsWorldPos.Add(topRight)) duplicates++;
			if(!wallsWorldPos.Add(bottomLeft)) duplicates++;
			if(!wallsWorldPos.Add(bottomRight)) duplicates++;

			// if there is no exit, build wall
			if(!cell.ExitNorth || cell.Position.y == 0 ) wallsWorldPos.Add(topMiddleCenter); 
			if(!cell.ExitSouth || cell.Position.y == maze.Height - 1 ) wallsWorldPos.Add(bottomMiddleCenter);
			if(!cell.ExitEast || cell.Position.x == maze.Width - 1 ) wallsWorldPos.Add(middleRight);
			if(!cell.ExitWest || cell.Position.x == 0 ) wallsWorldPos.Add(middleLeft);

		}
		Debug.Log ("### Build completed:" + wallsWorldPos.Count + " Duplicates:" + duplicates);
		

		// Build objects
		foreach(Vector3 pos in wallsWorldPos) 
		{
			GameObject wallObject = (GameObject)GameObject.Instantiate(wallPrefab, pos, Quaternion.identity);
			wallObject.transform.parent = wallContainer.transform;
			//Debug.Log (string.Format ( " Wall pos: {0}", pos));
			debugObjectCount++;
		}

		// TODO: FIND BEFORE Min and Max vector coordinate
		BuildMissingWallsBetween();

		Debug.Log ("## Total walls objects: " + debugObjectCount);
	}

	// Build missing walls between line:  find all in row x

	void BuildMissingWallsBetween()
	{

		List<Vector3> horizontalLine;
		for(int z = -2; z <= 30; z +=2)
		{
			horizontalLine = TraverseHorizontalAt(z);
			horizontalLine.Sort(new SortByX());
			List<Vector3> newHorizontalLine = GenerateWallsBetweenHorizontally(horizontalLine);
			BuildWallBetween(newHorizontalLine);
		}
		
		//int = range * 
		List<Vector3> verticalLine;
		for(int x = -2; x <= 30; x +=2)
		{
			verticalLine = TraverseVerticalAt(x);
			verticalLine.Sort(new SortByZ());
			List<Vector3> newVerticalLine= GenerateWallsBetweenVertically(verticalLine);
			BuildWallBetween(newVerticalLine);
		}

	}

	List<Vector3> GenerateWallsBetweenHorizontally(List<Vector3> positions)
	{
		List<Vector3> newPositions = new List<Vector3>(); 
		for(int i = 0; i < positions.Count - 1; i++)
		{
			Vector3 current = positions[i];
			Vector3 next = positions[i+1];
			float dist = Mathf.Abs (current.x - next.x);
			if(dist == distance) {
				Vector3 between = new Vector3( current.x + distance/2f, current.y, current.z);
				newPositions.Add (between);
			}
			Debug.Log ("["+ i + "] p1.x: " + current.x + " p2.x: " + next.x +  " # d: " + dist);
		}
				
		Debug.Log ("# GenerateWallsBetweenHorizontally return: " + newPositions.Count);
		return newPositions;
	}


	List<Vector3> GenerateWallsBetweenVertically(List<Vector3> positions)
	{
		List<Vector3> newPositions = new List<Vector3>(); 
		for(int i = 0; i < positions.Count - 1; i++)
		{
			Vector3 current = positions[i]; 
			Vector3 next = positions[i+1]; 
			float dist = Mathf.Abs (current.z - next.z);
			if(dist == distance) {
				Vector3 between = new Vector3( current.x , current.y, current.z + distance/2f);
				newPositions.Add (between);
			}
			Debug.Log ("["+ i + "] p1.z: " + current.z + " p2.z: " + next.z +  " # d: " + dist);
		}
		
		Debug.Log ("# GenerateWallsBetweenVertically return: " + newPositions.Count);
		return newPositions;
	}

	void BuildWallBetween(List<Vector3> list)
	{
		foreach(Vector3 pos in list) 
		{
			GameObject wallObject = (GameObject)GameObject.Instantiate(wall2Prefab, pos, Quaternion.identity);
			wallObject.transform.parent = wallContainer.transform;
			debugObjectCount++;
			//Debug.Log (string.Format ( "=> Wall pos: {0}", pos));
		}
	}

	List<Vector3> TraverseVerticalAt(int x)
	{
		List<Vector3> list = new List<Vector3>();
		foreach(Vector3 pos in wallsWorldPos)
		{
			if(pos.x == x)
				list.Add(pos);
		}
		//Debug.Log ("TraverseVertical return: " + list.Count);
		return list;
	}

	List<Vector3> TraverseHorizontalAt(int z)
	{
		List<Vector3> list = new List<Vector3>();
		foreach(Vector3 pos in wallsWorldPos)
		{
			if(pos.z == z)
				list.Add(pos);
		}
		//Debug.Log ("TraverseHorizontal return: " + list.Count);
		return list;
	}

}