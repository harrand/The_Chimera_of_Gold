using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/**
* The Board class acts like a flat 2D board in the 3D world.
* Board.cs applies to a GameObject with the following effects:
* Width (in number of Tiles) = GameObject.Scale.X
* Height (in number of Tiles) = GameObject.Scale.Y
*/
public class Board : MonoBehaviour
{
    /**
    * width and height use number of Tiles as units.
    */
    private uint numberCamps, numberObstacles;
    public Tile[] Tiles { get; private set; }
	public Obstacle[] Obstacles { get; private set; }
	public Camp[] Camps { get; private set; }

	void Awake()
    {
		gameObject.name = "Board";
		// Set width and height (in Tiles) to be equal to the length and depth of the GameObject respectively (in pixels).
		this.GetWidthInTiles = Convert.ToUInt32(gameObject.transform.localScale.x);
		this.GetHeightInTiles = Convert.ToUInt32(gameObject.transform.localScale.z);
		this.Tiles = new Tile[this.GetWidthInTiles * this.GetHeightInTiles];
		for(uint i = 0; i < this.Tiles.Length; i++)
		{
            float xTile = i % this.GetWidthInTiles;
            float zTile = i / this.GetWidthInTiles;
            this.Tiles[i] = Tile.Create(this, xTile, zTile);
            GameObject tileObject = this.Tiles[i].gameObject;
			tileObject.transform.position = new Vector3(xTile, 0, zTile) * Game.TILE_SIZE;
			tileObject.transform.localScale *= Game.TILE_SIZE;
			tileObject.name = "Tile " + (i + 1);
		}

		this.numberCamps = 5;
		this.numberObstacles = 13;

		this.Obstacles = new Obstacle[this.numberObstacles];
		this.Camps = new Camp[this.numberCamps];
    }

	void Start()
	{

	}
	
	void Update()
    {
		
	}

    public void ResetTurns() { }

    public void NextTurn() { }

	/**
	 * Getters for width and height (in units of Tiles and pixels respectively)
	 */
	public uint GetWidthInTiles{get; private set;}
	public uint GetHeightInTiles{get; private set;}
	public float GetWidthInPixels{get{return this.GetWidthInTiles * Game.TILE_SIZE;}}
	public float GetHeightInPixels{get{return this.GetHeightInTiles * Game.TILE_SIZE;}}
    public Player PlayerTurn { get; private set; }

}

