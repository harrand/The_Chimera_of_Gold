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

	public static Board Create(GameObject root, uint tilesWidth, uint tilesHeight)
	{
		if(tilesWidth < 5 && tilesHeight < 5 && (tilesWidth * tilesHeight) < 13)
		{
			Debug.LogError("Board has invalid width/height. One of width or height must be at least 5 AND width * height MUST be greater than 13.");
		}
		Board board = root.AddComponent<Board>();
        root.tag = "GameBoard";
		//root.transform.localScale = new Vector3(width, 1, height);
		board.GetWidthInTiles = Convert.ToUInt32(tilesWidth);
		board.GetHeightInTiles = Convert.ToUInt32(tilesHeight);

		// The following code block should probably belong in Board::Start() or Board::Awake...
		// The reason is cannot be in Board::Start() is because Board::Start() is ran once the script is initialised and ready to execute, by the time multiple other scripts would have needed references to these camps etc...
		// The reason Board::Awake() cannot have this code block is because that will execute directly after "root.AddComponent<Board>()" which means before board.GetWidthInTiles is assigned.
		// Thus, the initialisation code MUST happen right here, despite being ugly.

		board.Tiles = new Tile[board.GetWidthInTiles * board.GetHeightInTiles];
		for(uint i = 0; i < board.Tiles.Length; i++)
		{
			float xTile = i % board.GetWidthInTiles;
			float zTile = i / board.GetWidthInTiles;
			board.Tiles[i] = Tile.Create(board, xTile, zTile);
			GameObject tileObject = board.Tiles[i].gameObject;
			Vector2 tileSize = Board.ExpectedTileSize(root, board.GetWidthInTiles, board.GetHeightInTiles);
			tileObject.transform.position = Game.MinWorldSpace(root) + (new Vector3(xTile * tileSize.x, 0, zTile * tileSize.y));
			tileObject.transform.position = new Vector3(tileObject.transform.position.x, Game.InterpolateYWorldSpace(root, tileObject.transform.position), tileObject.transform.position.z);
			tileObject.transform.localScale = new Vector3(tileSize.x, 1, tileSize.y);
			tileObject.name = "Tile " + (i + 1);
		}
		board.numberCamps = 5;
		board.numberObstacles = 13;

		board.Camps = new Camp[board.numberCamps];
		for (uint i = 0; i < board.numberCamps; i++)
			board.Camps[i] = Camp.Create(board, board.Tiles[i]);
		board.Obstacles = new Obstacle[board.numberObstacles];
		for (uint i = 0; i < board.numberObstacles; i++)
			board.Obstacles[i] = Obstacle.Create(board, board.Tiles[i]);
		return board;
	}

	public static Board CreateNoTerrain(GameObject root, float width, float height, uint tilesWidth, uint tilesHeight)
	{
		if(width < 5 && height < 5 && (width * height) < 13)
		{
			Debug.LogError("Board has invalid width/height. One of width or height must be at least 5 AND width * height MUST be greater than 13.");
		}
		Board board = root.AddComponent<Board>();
		//root.tag = "GameBoard";
		root.name = "Board";
		root.transform.localScale = new Vector3(width, 1, height);
		board.GetWidthInTiles = Convert.ToUInt32(tilesWidth);
		board.GetHeightInTiles = Convert.ToUInt32(tilesHeight);

		// The following code block should probably belong in Board::Start() or Board::Awake...
		// The reason is cannot be in Board::Start() is because Board::Start() is ran once the script is initialised and ready to execute, by the time multiple other scripts would have needed references to these camps etc...
		// The reason Board::Awake() cannot have this code block is because that will execute directly after "root.AddComponent<Board>()" which means before board.GetWidthInTiles is assigned.
		// Thus, the initialisation code MUST happen right here, despite being ugly.

		board.Tiles = new Tile[board.GetWidthInTiles * board.GetHeightInTiles];
		for(uint i = 0; i < board.Tiles.Length; i++)
		{
			float xTile = i % board.GetWidthInTiles;
			float zTile = i / board.GetWidthInTiles;
			board.Tiles[i] = Tile.Create(board, xTile, zTile);
			GameObject tileObject = board.Tiles[i].gameObject;
			tileObject.transform.position = new Vector3(xTile, 0, zTile) * Game.TILE_SIZE;
			tileObject.transform.localScale *= Game.TILE_SIZE;
			tileObject.name = "Tile " + (i + 1);
		}
		board.numberCamps = 5;
		board.numberObstacles = 13;

		board.Camps = new Camp[board.numberCamps];
		for (uint i = 0; i < board.numberCamps; i++)
			board.Camps[i] = Camp.Create(board, board.Tiles[i]);
		board.Obstacles = new Obstacle[board.numberObstacles];
		for (uint i = 0; i < board.numberObstacles; i++)
			board.Obstacles[i] = Obstacle.Create(board, board.Tiles[i]);
		return board;
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

	public static Vector2 ExpectedTileSize(GameObject root, float widthTiles, float heightTiles)
	{
		Vector3 min = Game.MinWorldSpace(root), max = Game.MaxWorldSpace(root);
		float deltaX = max.x - min.x, deltaZ = max.z - min.z;
		return new Vector2(deltaX / widthTiles, deltaZ / heightTiles);
	}
}