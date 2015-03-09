using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Miscellaneous utility classes.
*/

public class SortVector3ByX : IComparer<Vector3>
{
	public int Compare( Vector3 a, Vector3 b)
	{
		if(a.x > b.x) return 1;
		else if (a.x < b.x) return -1;
		else return 0;
	}
}

public class SortVector3ByZ : IComparer<Vector3>
{
	public int Compare( Vector3 a, Vector3 b)
	{
		if(a.z > b.z) return 1;
		else if (a.z < b.z) return -1;
		else return 0;
	}
}