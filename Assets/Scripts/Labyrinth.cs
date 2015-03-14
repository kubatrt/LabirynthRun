using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


//----------------------------------------------------------------------------------------------------------------------
public class Labyrinth : MonoBehaviour 
{
	//public PlayerMecanimController player;

	public GameObject wallPrefab;
	public GameObject wall2Prefab;
	public GameObject deadEndPrefab;
	public GameObject triggerPrefab;
	public GameObject finishPrefab;
	public GameObject playerPrefab;
	public GameObject mapCameraPrefab;
	public GameObject debugPrefab;
	public GameObject groundPrefab;

	// default values for prefab settings ... do not touch!
	public float size = 1f;			// prefab size
	public float distance = 2f; 	// world distance between walls after scaling
	public float scale = 2f;		// scale of instantiated walls objects
	public float offset = 4f;		// distance between centers of cells * offset = distance between cells
	public float wallHeight = 1.5f;

	int debugObjectCount;

	MazeGenerator maze;
	List<MazeCell> cells = new List<MazeCell>();
	HashSet<Vector3> wallsWorldPositions = new HashSet<Vector3>();

	GameObject wallContainer;
	GameObject objectsContainer;
	GameObject triggersContainer;
	GameObject groundContainer;

	GameObject mapCamera;

	// local method, or use MazeGenerator.GridToRorld
	Vector3 MazeToWorld(GridPosition cellPos)
	{
		return new Vector3((float)cellPos.x * offset, wallHeight, (float)cellPos.y * offset);
	}

	void Awake () 
	{
		maze = GetComponent<MazeGenerator>();

		// return if maze already exists
		if(transform.GetComponentsInChildren<Transform>().Length != 1)
			return;
		
		CreateMaze();
		BuildWalls();
		CreateGround();
		CreatePlayer ();
		CreateGameObjects();
		CreateGround();

		Debug.Log ("Labyrinth.Awake()");
	}

	void Start ()
	{
		SetCamerasAtStart ();
		Debug.Log ("Labyrinth.Start()");
	}

	public void SetCamerasAtStart()
	{
		// set cameras at start position
		float x = (((float)(maze.Width)/2-1)*4)+2;
		float z = (((float)(maze.Height)/2-1)*4)-2;
		PlayerCamera.Instance.SetStartUpPosition(x,x*4,z);
		PlayerCamera.Instance.SetPosition(x,x*4,z);
		MapCamera.Instance.SetPosition (x, 5, z);
		MapCamera.Instance.SetCameraSize (((maze.Width + maze.Height)/2)*4);
	}

	public void CreateMaze()
	{
		maze = GetComponent<MazeGenerator>();
		string mazeFile = Application.dataPath + "/Levels/test.maze";
		//maze.Width = 8; maze.Height = 8;
		maze.LoadFromFile (mazeFile);
		//maze.Generate ();
		cells = maze.GetCells();

		debugObjectCount = 0;
		CreateContainers();
		Debug.Log ("Labyrinth.CreateMaze()");
	}

	void CreateContainers()
	{
		if(GameObject.Find("_Walls") == null) {
			wallContainer = new GameObject("_Walls");
			wallContainer.transform.parent = transform;
		} 

		if(GameObject.Find ("_Objects") == null) {
			objectsContainer = new GameObject("_Objects");
			objectsContainer.transform.parent = transform;
		}

		if(GameObject.Find ("_Triggers") == null) {
			triggersContainer = new GameObject("_Triggers");
			triggersContainer.transform.parent = transform;
		}
		if(GameObject.Find ("_Ground") == null) {
			groundContainer = new GameObject("_Ground");
			groundContainer.transform.parent = transform;
		}
	}

	public void ClearMaze()
	{
		if(Application.isEditor) {
			if(wallContainer != null) DestroyImmediate(wallContainer);
			if(objectsContainer != null) DestroyImmediate(objectsContainer);
			if(triggersContainer != null) DestroyImmediate(triggersContainer);
			if(groundContainer != null) DestroyImmediate(groundContainer);
		}
		else
		{
			if(wallContainer != null) Destroy(wallContainer);
			if(objectsContainer != null) Destroy(objectsContainer);
			if(triggersContainer != null) Destroy(triggersContainer);
			if(groundContainer != null) Destroy(groundContainer);
		}

	}

	public void CreatePlayer()
	{
		foreach(MazeCell cell in cells) 
		{
			if(cell.IsStartCell) 
			{
				// create player and set rotation
				GameObject newObject = (GameObject)GameObject.Instantiate(
					playerPrefab, MazeGenerator.GridToWorld(cell.Position, offset, 0f),Quaternion.identity);
				newObject.transform.parent = transform;
			}
		}
	}

	public void CreateGameObjects()
	{
		int triggersCounter = 0;
		foreach(MazeCell cell in cells) 
		{
			/*if(cell.IsStartCell) 
			{
				// create player and set rotation
				GameObject newObject = (GameObject)GameObject.Instantiate(
					playerPrefab, MazeGenerator.GridToWorld(cell.Position, offset, 0f),Quaternion.identity);
				newObject.transform.parent = objectsContainer.transform;


				if(cell.ExitEast)
				{
					newObject.transform.Rotate(new Vector3(0,90f,0));
				}
				else if(cell.ExitWest)
				{
					newObject.transform.Rotate(new Vector3(0,-90f,0));
				}
				else if(cell.ExitNorth)
				{
					newObject.transform.Rotate(new Vector3(0,180f,0));
				}
			}*/
			if(cell.IsFinishCell) 
			{
				GameObject newObject = (GameObject)GameObject.Instantiate(
					finishPrefab, MazeGenerator.GridToWorld(cell.Position, offset, wallHeight), finishPrefab.transform.rotation);
				newObject.transform.parent = objectsContainer.transform;	
			}
			else if(cell.TotalExits > 2) 
			{
				GameObject newObject = (GameObject)GameObject.Instantiate(
					triggerPrefab, MazeGenerator.GridToWorld(cell.Position, offset, triggerPrefab.transform.localScale.y/2f), Quaternion.identity);
				newObject.transform.parent = triggersContainer.transform;
				newObject.name = "TriggerCrossroad(MoreWays) " + triggersCounter++;
				newObject.GetComponent<TriggerCrossroad>().crossingType = TriggerCrossing.MoreWays;

			}
			else if((cell.ExitNorth && (cell.ExitWest || cell.ExitEast)) || 
			        (cell.ExitSouth && (cell.ExitWest || cell.ExitEast)))
			{
				GameObject newObject = (GameObject)GameObject.Instantiate(
					triggerPrefab, MazeGenerator.GridToWorld(cell.Position, offset, triggerPrefab.transform.localScale.y/2f), Quaternion.identity);
				newObject.transform.parent = triggersContainer.transform;
				newObject.name = "TriggerCrossroad(OneWay) " + triggersCounter++;
				newObject.GetComponent<TriggerCrossroad>().crossingType = TriggerCrossing.OneWay;

			} 
			/*if(cell.TotalExits > 2) // debug
			{
				GameObject newObject = (GameObject)GameObject.Instantiate(
					debugPrefab, MazeGenerator.GridToWorld(cell.Position, offset, debugPrefab.transform.localScale.y/2f), Quaternion.identity);
				newObject.transform.parent = objectsContainer.transform;	
			}*/
		}
	}

	public void BuildWalls()
	{
		wallsWorldPositions.Clear();

		// Step I. Generate basic data, all corners and middles in cells
		foreach(MazeCell cell in cells) 
		{
			Vector3 centroid = MazeGenerator.GridToWorld(cell.Position, offset, wallHeight);
			
			Vector3 topLeft			 	= new Vector3(centroid.x - scale, centroid.y, centroid.z - scale);
			Vector3 topMiddleCenter 	= new Vector3(centroid.x, wallHeight, centroid.z - scale);
			Vector3 topRight 			= new Vector3(centroid.x + scale, centroid.y, centroid.z - scale);
			Vector3 middleLeft 			= new Vector3(centroid.x - scale, centroid.y, centroid.z);
			Vector3 middleRight 		= new Vector3(centroid.x + scale, centroid.y, centroid.z);
			Vector3 bottomLeft 			= new Vector3(centroid.x - scale, centroid.y, centroid.z + scale);
			Vector3 bottomMiddleCenter 	= new Vector3(centroid.x, centroid.y, centroid.z + scale);
			Vector3 bottomRight 		= new Vector3(centroid.x + scale, centroid.y, centroid.z + scale);
			
			// hashSet provides 'no duplicates'
			wallsWorldPositions.Add(topLeft);
			wallsWorldPositions.Add(topRight);
			wallsWorldPositions.Add(bottomLeft);
			wallsWorldPositions.Add(bottomRight);
			
			// if there is no exit, build wall
			if(!cell.ExitNorth || cell.Position.y == 0 ) wallsWorldPositions.Add(topMiddleCenter); 
			if(!cell.ExitSouth || cell.Position.y == maze.Height - 1 ) wallsWorldPositions.Add(bottomMiddleCenter);
			if(!cell.ExitEast || cell.Position.x == maze.Width - 1 ) wallsWorldPositions.Add(middleRight);
			if(!cell.ExitWest || cell.Position.x == 0 ) wallsWorldPositions.Add(middleLeft);
		}
		//Debug.Log ("## Generation walls positions completed:" + wallsWorldPositions.Count);
		
		List<Vector3> wallsList = new List<Vector3>();
		foreach(Vector3 pos in wallsWorldPositions) {
			wallsList.Add(pos);
		}
		
		// Step II. Build walls
		BuildWallsFromList(wallsList, wallPrefab);
		
		// Step III. Build missing walls between world positions
		float horizontalMax = maze.Height * offset - scale;
		List<Vector3> horizontalLine; 
		for(float z = -scale; z <= horizontalMax; z +=scale) 
		{
			horizontalLine = GetHorizontalPositionsAt((int)z);
			horizontalLine.Sort(new SortVector3ByX());
			List<Vector3> newHorizontalLine = GenerateWallsBetweenHorizontally(horizontalLine);
			BuildWallsFromList(newHorizontalLine, wall2Prefab);
		}
		
		float verticalMax = maze.Width * offset - scale;
		List<Vector3> verticalLine;
		for(float x = -scale; x <= verticalMax; x +=scale)
		{
			verticalLine = GetVerticalPositions((int)x);
			verticalLine.Sort(new SortVector3ByZ());
			List<Vector3> newVerticalLine = GenerateWallsBetweenVertically(verticalLine);
			BuildWallsFromList(newVerticalLine, wall2Prefab);
		}

		//Debug.Log ("### Total walls objects: " + debugObjectCount);
	}

	public void CreateGround()
	{
		Debug.Log ("Create ground");
		foreach(MazeCell cell in cells) {
			if(cell.HasGround) {
				GameObject groundTile = 
					(GameObject) Instantiate(groundPrefab, MazeGenerator.GridToWorld( cell.Position, offset, 0), 
				                             groundPrefab.transform.rotation);
				groundTile.transform.parent = groundContainer.transform;
			}
		}
	}

	#region Generating walls data

	private void BuildWallsFromList(List<Vector3> list, GameObject wall)
	{
		foreach(Vector3 pos in list) 
		{
			GameObject wallObject = (GameObject)GameObject.Instantiate(wall, pos, Quaternion.identity);
			wallObject.transform.parent = wallContainer.transform;
			debugObjectCount++;
			//Debug.Log (string.Format ( "### Wall pos: {0}", pos));
		}
	}

	private List<Vector3> GenerateWallsBetweenHorizontally(List<Vector3> positions)
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
			//Debug.Log ("["+ i + "] p1.x: " + current.x + " p2.x: " + next.x +  " # d: " + dist);
		}
				
		//Debug.Log ("# GenerateWallsBetweenHorizontally return: " + newPositions.Count);
		return newPositions;
	}

	private List<Vector3> GenerateWallsBetweenVertically(List<Vector3> positions)
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
			//Debug.Log ("["+ i + "] p1.z: " + current.z + " p2.z: " + next.z +  " # d: " + dist);
		}

		//Debug.Log ("# GenerateWallsBetweenVertically return: " + newPositions.Count);
		return newPositions;
	}

	private List<Vector3> GetVerticalPositions(int x)
	{
		List<Vector3> list = new List<Vector3>();
		foreach(Vector3 pos in wallsWorldPositions)
			if(pos.x == x)
				list.Add(pos);
		return list;
	}

	private List<Vector3> GetHorizontalPositionsAt(int z)
	{
		List<Vector3> list = new List<Vector3>();
		foreach(Vector3 pos in wallsWorldPositions)
			if(pos.z == z)
				list.Add(pos);
		return list;
	}

	#endregion
}
