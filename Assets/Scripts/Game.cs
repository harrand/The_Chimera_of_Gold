using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
/**
* Game contains static functions and constant expressions such as:
* default board size
* default number of camps per board
* default number of players per camp
* @author Harry Hollands
*/
public class Game : MonoBehaviour
{
    public const uint NUMBER_OBSTACLES = 13;
    public const uint NUMBER_CAMPS = 5;
    public const uint PLAYERS_PER_CAMP = 5;
    public int currentPlayer = 1;

    // These are edited in Unity Component settings; 5 is just the default.
	public uint tileWidth = 5, tileHeight = 5;

    private Board board;

	void Start()
	{
        /*
        new TestBoard(50, 50, 5, 5); // Perform Board Unit Test
        new TestTile();
        new TestCamp();
        new TestPlayer();
        */

        
		// Create a normal Board with Input attached. Both Board and InputController are attached to the root GameObject (this).
		this.board = Board.Create(this.gameObject, tileWidth, tileHeight);
		this.board.gameObject.AddComponent<InputController>();   
        /*if(isServer)
        {
            NetworkServer.Spawn(this.board.gameObject);
        }*/
    }

    private void Update()
    {
        /*if (Input.GetKeyDown("r"))
        {
            this.board.GetDice.Roll();
        }*/
    }

    public void DiceRoll()
    {
        this.board.GetDice.Roll();
    }
    

    void OnDestroy()
	{

	}

    /**
    * Returns the vertex of the GameObject parameter terrain (in world-space) positively furthest away from the origin (in model-space)
    * @author Harry Hollands
    * @param gameObject used to calculate how big the world is
    * @return a vector containing the furthest point from the origin used by the world
    */
	public static Vector3 MaxWorldSpace(GameObject gameObject)
	{
		Vector3 boundMax = gameObject.GetComponent<Terrain>().terrainData.bounds.max;
		return boundMax + gameObject.transform.position;
	}

    /**
    * Returns the vertex of the GameObject parameter terrain (in world-space) negatively furthest away from the origin (in model-space)
    * @author Harry Hollands
    * @param gameObject used to calculate the minimum of the world space
    * @return a vector containing the closest point to the origin used by the world
    */
	public static Vector3 MinWorldSpace(GameObject gameObject)
	{
		Vector3 boundMax = gameObject.GetComponent<Terrain>().terrainData.bounds.min;
		return boundMax + gameObject.transform.position;
	}

    /**
    * Returns the actual y-coordinate of the terrain-data in world-space.
    * @author Harry Hollands
    * @param gameObject the gameObject to retreive the y coordinate from
    * @param positionWorldSpace the x and z coordinate of the gameObjects desired y coordinate
    * @return returns the y coordinate
    */
	public static float InterpolateYWorldSpace(GameObject gameObject, Vector3 positionWorldSpace)
	{
		return gameObject.GetComponent<Terrain>().SampleHeight(positionWorldSpace);
	}

    public void BoardNextTurn()
    {
        this.board.NextTurn();
    }
}
