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

		this.Camps = new Camp[this.numberCamps];
        for (uint i = 0; i < numberCamps; i++)
            this.Camps[i] = Camp.Create(this, this.Tiles[i]);
        this.Obstacles = new Obstacle[this.numberObstacles];
        for (uint i = 0; i < numberObstacles; i++)
            this.Obstacles[i] = Obstacle.Create(this, this.Tiles[i]);
    }

	void Start()
	{

	}
	
	void Update()
    {
		
	}

    public bool TileOccupiedByObstacle(Tile t)
    {
        return this.TileOccupiedByObstacle(t.PositionTileSpace);
    }

    public bool TileOccupiedByObstacle(Vector2 tilePosition)
    {
        foreach (Obstacle obstacle in this.Obstacles)
            if (obstacle.CurrentTile.PositionTileSpace == tilePosition)
                return true;
        return false;
    }

    public void ResetTurns()
    {
        this.PlayerTurn = this.Camps[0].TeamPlayers[0];
    }
		
    public void NextTurn()
    {
		int campId = -1, playerId = -1;
		// Set campId and playerId to the corresponding indices for the current Player
        for(uint campCounter = 0; campCounter < this.Camps.Length; campCounter++)
        {
            // paired codang
            for(uint playerCounter = 0; playerCounter < this.Camps[campCounter].TeamPlayers.Length; playerCounter++)
            {
				if(this.Camps[campCounter].TeamPlayers[playerCounter] == this.PlayerTurn)
				{
					campId = (int)campCounter;
					playerId = (int)playerCounter;
				}
            }
        }
		// If neither campId or playerId have changed, then the current player wasnt even found and something is going to go very wrong.
		if(campId == -1 || playerId == -1)
		{
			Debug.LogError("Board::PlayerTurn is not a member of any of its camp members.");
		}
		// Try to increment playerId. If it goes higher than the number of players per camp, increment the camp id and set player id to 0.
		if(++playerId >= Game.PLAYERS_PER_CAMP)
		{
			campId++;
			playerId = 0;
		}
		// If camp id is now greater than the actual number of camps, then we've gone past the final player of the final camp and need to go back to the beginning.
		if(campId >= Game.NUMBER_CAMPS)
			campId = 0;
		this.PlayerTurn = this.Camps[campId].TeamPlayers[playerId];
    }

	/**
	 * Getters for width and height (in units of Tiles and pixels respectively)
	 */
	public uint GetWidthInTiles{get; private set;}
	public uint GetHeightInTiles{get; private set;}
	public float GetWidthInPixels{get{return this.GetWidthInTiles * Game.TILE_SIZE;}}
	public float GetHeightInPixels{get{return this.GetHeightInTiles * Game.TILE_SIZE;}}
    public Player PlayerTurn { get; private set; }
}