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
    public const uint NUMBER_OBSTACLES = 13;
    public const uint NUMBER_CAMPS = 5;
    public const uint PLAYERS_PER_CAMP = 5;

    // These are edited in Unity Component settings; 5 is just the default.
	public uint tileWidth = 5, tileHeight = 5;

	void Start()
	{
        //new TestBoard(50, 50, 5, 5); // Perform Board Unit Test
        //new TestTile();
		//new TestCamp();

		// Create a normal Board with Input attached. Both Board and InputController are attached to the root GameObject (this).
		Board board = Board.Create(this.gameObject, tileWidth, tileHeight);
		board.gameObject.AddComponent<InputController>();   
    }

	void OnDestroy()
	{

	}

    /**
    * Returns the vertex of the GameObject parameter terrain (in world-space) positively furthest away from the origin (in model-space)
    */
	public static Vector3 MaxWorldSpace(GameObject gameObject)
	{
		Vector3 boundMax = gameObject.GetComponent<Terrain>().terrainData.bounds.max;
		return boundMax + gameObject.transform.position;
	}

    /**
    * Returns the vertex of the GameObject parameter terrain (in world-space) negatively furthest away from the origin (in model-space)
    */
	public static Vector3 MinWorldSpace(GameObject gameObject)
	{
		Vector3 boundMax = gameObject.GetComponent<Terrain>().terrainData.bounds.min;
		return boundMax + gameObject.transform.position;
	}

    /**
    * Returns the actual y-coordinate of the terrain-data in world-space.
    */
	public static float InterpolateYWorldSpace(GameObject gameObject, Vector3 positionWorldSpace)
	{
		return gameObject.GetComponent<Terrain>().SampleHeight(positionWorldSpace);
	}
}
