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

	void Start()
	{
        new TestBoard(); // Perform Board Unit Test
        new TestTile();
		//gameObject.AddComponent<Board>(); // Create the real Board from the 'Root' gameobject.
	}

	void OnDestroy()
	{

	}
}
