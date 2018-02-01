using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/**
* The Board class acts like a flat 2D board in the 3D world.
* Board.cs applies to a GameObject with the following effects:
* Width (in number of tiles) = GameObject.Scale.X
* Height (in number of tiles) = GameObject.Scale.Y
*/
public class Board : MonoBehaviour
{
    /**
    * width and height use number of Tiles as units.
    */
    private uint numberCamps, numberObstacles;
    private Tile[] tiles;
	private Obstacle[] obstacles;
	private Camp[] camps;

	void Awake()
    {
		gameObject.name = "Board";
		// Set width and height (in Tiles) to be equal to the length and depth of the GameObject respectively (in pixels).
		this.GetWidthInTiles = Convert.ToUInt32(gameObject.transform.localScale.x);
		this.GetHeightInTiles = Convert.ToUInt32(gameObject.transform.localScale.z);
		this.tiles = new Tile[this.GetWidthInTiles * this.GetHeightInTiles];
		for(uint i = 0; i < this.tiles.Length; i++)
		{
			GameObject tileObject = Instantiate(Resources.Load("Prefabs/Tile")) as GameObject;
			tileObject.transform.parent = gameObject.transform;
			float xTile = i % this.GetWidthInTiles;
			float zTile = i / this.GetWidthInTiles;
			tileObject.transform.position = new Vector3(xTile, 0, zTile) * Game.TILE_SIZE;
			tileObject.transform.localScale *= Game.TILE_SIZE;
			tileObject.name = "Tile " + (i + 1);
			this.tiles[i] = tileObject.AddComponent<Tile>();
		}

		this.numberCamps = 5;
		this.numberObstacles = 13;

		this.obstacles = new Obstacle[this.numberObstacles];
		this.camps = new Camp[this.numberCamps];
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
	public float GetWidthInPixels{get{return this.GetWidthInTiles * Game.TILE_SIZE;}}
	public float GetHeightInPixels{get{return this.GetHeightInTiles * Game.TILE_SIZE;}}

	public Tile[] GetTiles{get{return this.tiles;}}
	public Camp[] GetCamps{get{return this.camps;}}
	public Obstacle[] GetObstacles{get{return this.obstacles;}}

}

