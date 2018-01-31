using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/**
* The Board class acts like a flat 2D board in the 3D world.
* Board.cs applies to a GameObject with the following effects:
* Width (in number of tiles) = GameObject.Scale.X
* Height (in number of tiles) = GameObject.Scale.Z
*/
public class Board : MonoBehaviour
{
    /**
    * width and height use number of Tiles as units.
    */
    private uint width, height, numberCamps, numberObstacles;
    private Tile[] tiles;

	void Awake()
    {
        gameObject.name = "Board";
    }

	void Start()
	{

	}
	
	void Update()
    {
		
	}
}

