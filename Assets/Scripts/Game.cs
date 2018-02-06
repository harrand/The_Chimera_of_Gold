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
	public const uint TILE_SIZE = 5;
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
		Board board = Board.Create(this.gameObject, width, height, tileWidth, tileHeight);
		board.gameObject.AddComponent<InputController>();
        
    }

	void OnDestroy()
	{

	}
}
