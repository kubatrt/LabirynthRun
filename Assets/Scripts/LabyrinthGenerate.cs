using UnityEngine;
using System.Collections;

public class LabyrinthGenerate : MonoBehaviour {
	
	public int width, height;
	// int xPos, yPos;

	public int corridorWidth = 3;
	public int wallWidth = 1;
	public int wallHeight = 4;

	// Use this for initialization
	void Start () {
		createGrid ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			createAtRandomCellPos();
		}
	}

	void createGrid()
	{
		int x = width * 2;
		int y = height * 2;
		// horizontal
		for(int i = -y; i <= y; i++)
		{
			for(int j = -x; j <= x; j+=4)
			{
				GameObject pillar = (GameObject)Instantiate (Resources.Load ("Pillar"));
				pillar.transform.position = new Vector3(i + 0.5f, 2 , j + 0.5f);
			}
		}
		// vertical
		for(int i = -y; i <= y; i+=4)
		{
			for(int j = -x; j <= x; j++)
			{
				if(y%2 == 0 && j%4 != 0 || y%2 != 0 && (j%2 != 0 && j%4 == 0))
				{ 
					GameObject pillar = (GameObject)Instantiate (Resources.Load ("Pillar"));
					pillar.transform.position = new Vector3(i+0.5f,2,j+0.5f);
				}
			}
		}
	}

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
		GameObject cos = (GameObject)Instantiate (Resources.Load ("Finish"));
		int x = randomCellYPoss ();
		int z = randomCellXPoss ();
		cos.transform.position = new Vector3(x+0.5f,2,z+0.5f);
	}
}