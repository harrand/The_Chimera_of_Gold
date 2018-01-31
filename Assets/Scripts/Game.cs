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
		// Right now just run the Board Unit Test.
		new TestBoard();
	}

	void OnDestroy()
	{

	}
}
