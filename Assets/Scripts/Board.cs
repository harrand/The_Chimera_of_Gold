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
    private uint numberCamps, numberObstacles;
    private Tile[] tiles;

	void Awake()
    {
		gameObject.name = "Board";
		// Set width and height (in Tiles) to be equal to the length and depth of the GameObject respectively (in pixels).
		this.GetWidthInTiles = Convert.ToUInt32(gameObject.transform.localScale.x);
		this.GetHeightInTiles = Convert.ToUInt32(gameObject.transform.localScale.z);
		this.tiles = new Tile[this.GetWidthInTiles * this.GetHeightInTiles];
    }

	void Start()
	{

	}
	
	void Update()
    {
		
	}

	/**
	 * Getters for width and height (in units of Tiles and pixels respectively)
	 */
	public uint GetWidthInTiles{get; private set;}
	public uint GetHeightInTiles{get; private set;}
	public uint GetWidthInPixels{get{return this.GetWidthInTiles * Game.TILE_SIZE;}}
	public uint GetHeightInPixels{get{return this.GetHeightInTiles * Game.TILE_SIZE;}}

	public Tile[] GetTiles{get{return this.tiles;}}
}

