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
    public GameObject trapHolePrefab;
    public GameObject trapSpikesPrefab;
    public GameObject trapSawPrefab;
	public GameObject finishPrefab;
	public GameObject playerPrefab;
	public GameObject debugPrefab;
	public GameObject groundPrefab;

	// default values for prefab settings ... do not touch!
	public float size = 1f;			// prefab size
	public float distance = 2f; 	// world distance between walls after scaling
	public float scale = 2f;		// scale of instantiated walls objects
	public float offset = 4f;		// distance between centers of cells * offset = distance between cells
	public float wallHeight = 1.5f;

	public int Width { get { return maze.Width; } }
	public int Height { get { return maze.Height; } }


	MazeGenerator maze;
	List<MazeCell> cells;
	HashSet<Vector3> wallsWorldPositions;

	// Containers
	GameObject wallContainer, objectsContainer, triggersContainer, groundContainer;

	// local method, or use MazeGenerator.GridToRorld
	Vector3 MazeToWorld(GridPosition cellPos)
	{
		return new Vector3((float)cellPos.x * offset, wallHeight, (float)cellPos.y * offset);
	}

	void Awake () 
	{
		maze = GetComponent<MazeGenerator>();

		Debug.Log ("Labyrinth.Awake()");
	}

	void Start ()
	{
		// Create once or find existing!
		FindContainers();
		CreateContainers();

		Debug.Log ("Labyrinth.Start()");
	}


	public Vector3 GetStartCellRotation()
	{
		Vector3 startRotation = Vector3.zero;
		foreach(MazeCell cell in cells) {
			if(cell.IsStartCell) {
				if(cell.ExitEast) startRotation = new Vector3(0, 90f, 0);
				if(cell.ExitWest) startRotation = new Vector3(0, 270f, 0); 
				if(cell.ExitNorth) startRotation = new Vector3(0, 180f, 0);
				break;
			}
		}
		return startRotation;
	}

	public void CreateMaze()
	{
		maze = GetComponent<MazeGenerator>();
		maze.Generate ();
		cells = maze.GetCells();

		Debug.Log ("Labyrinth.CreateMaze()");
	}

	public void CreateMaze(string labName)
	{
		maze = GetComponent<MazeGenerator>();
		maze.LoadFromFile( Application.dataPath + "/Levels/" + labName);
		cells = maze.GetCells();

		Debug.Log ("Labyrinth.CreateMaze()");
	}

	public void CreateContainers()
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
		Debug.Log ("CreateContainers()");
	}

	private void FindContainers()
	{
		wallContainer = GameObject.Find ("_Walls");
		objectsContainer = GameObject.Find ("_Objects");
		triggersContainer = GameObject.Find ("_Triggers");
		groundContainer = GameObject.Find ("_Ground");
	}

	public void ClearMaze()
	{
		if(wallContainer == null)
			return;

		foreach(Transform t in wallContainer.GetComponentsInChildren<Transform>())
		{
			if(t != wallContainer.transform)
				Destroy(t.gameObject);
		}
		foreach(Transform t in objectsContainer.GetComponentsInChildren<Transform>())
		{
			if(t != objectsContainer.transform)
			Destroy(t.gameObject);
		}
		foreach(Transform t in triggersContainer.GetComponentsInChildren<Transform>())
		{
			if(t != triggersContainer.transform)
			Destroy(t.gameObject);
		}
		foreach(Transform t in groundContainer.GetComponentsInChildren<Transform>())
		{
			if(t != groundContainer.transform)
			Destroy(t.gameObject);
		}
		Debug.Log("ClearMaze");
	}

	public void ClearEditorMaze()
	{
		FindContainers();
		if(wallContainer != null) DestroyImmediate(wallContainer);
		if(objectsContainer != null) DestroyImmediate(objectsContainer);
		if(triggersContainer != null) DestroyImmediate(triggersContainer);
		if(groundContainer != null) DestroyImmediate(groundContainer);
	}

	/*public void CreatePlayer()
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
	}*/

	public void CreateGameObjects()
	{
		int triggersCounter = 0;

		foreach(MazeCell cell in cells) 
		{
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

            else if(cell.Trap == TrapType.Hole)
            {
                GameObject newObject = (GameObject)GameObject.Instantiate(
                    trapHolePrefab, MazeGenerator.GridToWorld(cell.Position, offset, 1), 
                    Quaternion.identity);
                newObject.transform.parent = objectsContainer.transform;
                newObject.name = "Hole Trap ";
            }

            else if (cell.Trap == TrapType.Spikes)
            {
                GameObject newObject = (GameObject)GameObject.Instantiate(
                    trapSpikesPrefab, MazeGenerator.GridToWorld(cell.Position, offset, 1),
                    Quaternion.identity);
				newObject.transform.parent = objectsContainer.transform;
                newObject.name = "Spikes Trap ";
            }

		}
	}

	public void BuildWalls()
	{
		//wallsWorldPositions.Clear();
		wallsWorldPositions = new HashSet<Vector3>();

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

		// Step II. Build walls
		List<Vector3> wallsList = new List<Vector3>(wallsWorldPositions);
		BuildWallsFromList(wallsList, wallPrefab);
		
		// Step III. Build missing walls between world positions
		float horizontalMax = maze.Height * offset - scale;
		List<Vector3> horizontalLine; 
		for(float z = -scale; z <= horizontalMax; z +=scale) 
		{
			horizontalLine = GetHorizontalWallPositionsAt((int)z);
			horizontalLine.Sort( (Vector3 a, Vector3 b) => { if(a.x > b.x) return 1; else if(a.x < b.x) return -1; else return 0; } );
			List<Vector3> newHorizontalLine = GenerateWallsBetweenHorizontally(horizontalLine);
			BuildWallsFromList(newHorizontalLine, wall2Prefab);
		}
		
		float verticalMax = maze.Width * offset - scale;
		List<Vector3> verticalLine;
		for(float x = -scale; x <= verticalMax; x +=scale)
		{
			verticalLine = GetVerticalWallPositionsAt((int)x);
			verticalLine.Sort( (Vector3 a, Vector3 b) => { if(a.z > b.z) return 1; else if(a.z < b.z) return -1; else return 0; } );
			List<Vector3> newVerticalLine = GenerateWallsBetweenVertically(verticalLine);
			BuildWallsFromList(newVerticalLine, wall2Prefab);
		}

		//Debug.Log ("### Total walls objects: " + debugObjectCount);
	}

	public void CreateGround()
	{
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
			wallObject.transform.SetParent(wallContainer.transform);
			//debugObjectCount++; //Debug.Log (string.Format ( "### Wall pos: {0}", pos));
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

	private List<Vector3> GetVerticalWallPositionsAt(int x)
	{
		List<Vector3> list = new List<Vector3>();
		foreach(Vector3 pos in wallsWorldPositions)
			if(pos.x == x)
				list.Add(pos);
		return list;
	}

	private List<Vector3> GetHorizontalWallPositionsAt(int z)
	{
		List<Vector3> list = new List<Vector3>();
		foreach(Vector3 pos in wallsWorldPositions)
			if(pos.z == z)
				list.Add(pos);
		return list;
	}

	#endregion
}
