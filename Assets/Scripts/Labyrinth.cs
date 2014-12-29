using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Labyrinth : MonoBehaviour 
{

	public GameObject	wallPrefab;

	List<MazeCell> cells = new List<MazeCell>();


	void Start () 
	{
		cells = GetComponent<MazeGenerator>().GetCells();
		// in Awake() maze is generated, now its ready to use...

		foreach(MazeCell cell in cells) {
			// do something with cells
		}
	}

	void Update()
	{

	}

}