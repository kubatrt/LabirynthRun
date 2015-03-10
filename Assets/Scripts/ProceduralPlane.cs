using UnityEngine;
using System.Collections;

// Generated mesh
public class ProceduralPlane : MonoBehaviour 
{
	public Vector2 size = new Vector2(1,1);

	private void Awake()
	{
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter> ();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
	
		if(meshFilter.sharedMesh == null){
			meshFilter.sharedMesh = new Mesh();
		}
		
		Mesh mesh = meshFilter.sharedMesh;

		Vector3[] verts = new Vector3[] {
			new Vector3 (-size.x * 0.5f, 0, -size.y * 0.5f),
			new Vector3 (-size.x * 0.5f, 0,size.y * 0.5f),
			new Vector3 (size.x * 0.5f, 0, size.y * 0.5f),
			new Vector3 (size.x * 0.5f, 0, -size.y * 0.5f)
		};

		int[] triangles = new int[]{
			0,1,3,
			3,1,2			
		};

		Vector2[] uv = new Vector2[]{
			new Vector2(0,0),
			new Vector2(0,1),
			new Vector2(1,1),
			new Vector2(1,0)
		};

		mesh.Clear();
		mesh.vertices = verts;
		mesh.triangles = triangles;
		mesh.uv = uv;

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();

		meshRenderer.sharedMaterial = new Material(Shader.Find("Unlit/Texture"));
		meshRenderer.sharedMaterial.shader = Shader.Find("Unlit/Texture");
	}
}
