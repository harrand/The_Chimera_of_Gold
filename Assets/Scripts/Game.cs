using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Game shall contain static functions and constant expressions such as:
* default board size
* default number of camps per board
* default number of players per camp
*/

public class Game : MonoBehaviour
{
	public const uint TILE_SIZE = 15;
    public const uint NUMBER_OBSTACLES = 13;
    public const uint NUMBER_CAMPS = 5;
    public const uint PLAYERS_PER_CAMP = 5;

	public float width = 50, height = 50;
	public uint tileWidth = 5, tileHeight = 5;


	void Start()
	{
        //new TestBoard(50, 50, 5, 5); // Perform Board Unit Test
        //new TestTile();
		//new TestCamp();

		
		// Create a normal board with Input attached.
		Board board = Board.Create(this.gameObject, tileWidth, tileHeight);
		board.gameObject.AddComponent<InputController>();   
    }

	void OnDestroy()
	{

	}

	public static Vector3 MaxWorldSpace(GameObject gameObject)
	{
		Vector3 boundMax = gameObject.GetComponent<Terrain>().terrainData.bounds.max;
		return boundMax + gameObject.transform.position;
	}

	public static Vector3 MinWorldSpace(GameObject gameObject)
	{
		Vector3 boundMax = gameObject.GetComponent<Terrain>().terrainData.bounds.min;
		return boundMax + gameObject.transform.position;
	}
	/*
	private static Vector3[] VerticesWorldSpace(GameObject gameObject)
	{
		Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
		Vector3[] verticesWorldSpace = new Vector3[mesh.vertices.Length];
		for(uint i = 0; i < mesh.vertices.Length; i++)
		{
			verticesWorldSpace[i] = gameObject.transform.localToWorldMatrix.MultiplyPoint3x4(mesh.vertices[i]);
		}
		return verticesWorldSpace;
	}

	private static Vector3 MinWorldSpace(GameObject gameObject)
	{
		Vector3[] verticesWorldSpace = Game.VerticesWorldSpace(gameObject);
		float x = verticesWorldSpace[0].x, y = verticesWorldSpace[0].y, z = verticesWorldSpace[0].z;
		foreach(Vector3 vertex in verticesWorldSpace)
		{
			x = Mathf.Min(x, vertex.x);
			y = Mathf.Min(y, vertex.y);
			z = Mathf.Min(z, vertex.z);
		}
		return new Vector3(x, y, z);
	}

	private static Vector3 MaxWorldSpace(GameObject gameObject)
	{
		Vector3[] verticesWorldSpace = Game.VerticesWorldSpace(gameObject);
		float x = verticesWorldSpace[0].x, y = verticesWorldSpace[0].y, z = verticesWorldSpace[0].z;
		foreach(Vector3 vertex in verticesWorldSpace)
		{
			x = Mathf.Max(x, vertex.x);
			y = Mathf.Max(y, vertex.y);
			z = Mathf.Max(z, vertex.z);
		}
		return new Vector3(x, y, z);
	}
	*/
}
