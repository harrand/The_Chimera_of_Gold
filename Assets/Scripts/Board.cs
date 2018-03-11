using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/**
* Board is a container for Camps (and their respective Players), all Tiles and Obstacles. Also handles turns, tile-highlighting and game rule enforcement.
* @author Harry Hollands, Ciara O'Brien, Aswin Mathew
*/
public class Board : MonoBehaviour
{
    /// width and height use number of Tiles as units.
    private uint numberCamps, numberObstacles;
    private float width, height;
    public bool obstacleControlFlag;
    public Tile[] Tiles { get; private set; }
	public Obstacle[] Obstacles { get; private set; }
	public Camp[] Camps { get; private set; }
    public Dice GetDice { get; private set; }
	public BoardEvent Event{ get; private set; }

    /**
    * This is to be used to create a new Board when the root GameObject has a terrain component (interpolating instead of taking dimensions as parameters).
    * This is currently used for the main game board. However, it is not used for the Board unit test.
    * @author Harry Hollands
    * @param root - GameObject which the Board should be attached to.
    * @param tilesWidth - Number of Tiles per row.
    * @param tilesHeight - Number of Tiles per column.
    * @return - Reference to the Board created.
    */
    public static Board Create(GameObject root, uint tilesWidth, uint tilesHeight)
	{
		if(tilesWidth < 5 && tilesHeight < 5 && (tilesWidth * tilesHeight) < 13)
		{
			Debug.LogError("Board has invalid width/height (tile-space). One of width or height must be at least 5 AND width * height MUST be greater than 13.");
		}
		Board board = root.AddComponent<Board>();
        root.tag = "GameBoard";
        root.name += " (Board)";
        board.GetDice = Dice.Create(board.gameObject.transform.position, new Vector3(), new Vector3(1, 1, 1));
		board.Event = new BoardEvent(board);
		board.GetWidthInTiles = Convert.ToUInt32(tilesWidth);
		board.GetHeightInTiles = Convert.ToUInt32(tilesHeight);

		// The following code block should probably belong in Board::Start() or Board::Awake...
		// The reason is cannot be in Board::Start() is because Board::Start() is ran once the script is initialised and ready to execute, by the time multiple other scripts would have needed references to these camps etc...
		// The reason Board::Awake() cannot have this code block is because that will execute directly after "root.AddComponent<Board>()" which means before board.GetWidthInTiles is assigned.
		// Thus, the initialisation code MUST happen right here, despite being ugly.

        /// Allocate and assign Tiles (before culling).
		board.Tiles = new Tile[board.GetWidthInTiles * board.GetHeightInTiles];
		for(uint i = 0; i < board.Tiles.Length; i++)
		{
			float xTile = i % board.GetWidthInTiles;
			float zTile = i / board.GetWidthInTiles;
			board.Tiles[i] = Tile.Create(board, xTile, zTile);
            board.Tiles[i].InitialMaterial = Tile.PrefabMaterial();
			GameObject tileObject = board.Tiles[i].gameObject;
			Vector2 tileSize = Board.ExpectedTileSize(root, board.GetWidthInTiles, board.GetHeightInTiles);
			tileObject.transform.position = Game.MinWorldSpace(root) + (new Vector3(xTile * tileSize.x, 0, zTile * tileSize.y)) + new Vector3(6,0,6);
			tileObject.transform.position = new Vector3(tileObject.transform.position.x, Game.InterpolateYWorldSpace(root, tileObject.transform.position), tileObject.transform.position.z);
			tileObject.transform.localScale = new Vector3(tileSize.x, 1, tileSize.y);
			tileObject.name = "Tile " + (i + 1);
		}
        /// Hardcode these; design changes did not allow these to vary.
        board.numberCamps = 5;
		board.numberObstacles = 13;

        /// Allocate and assign Camps.
		board.Camps = new Camp[PlayerData.numberOfPlayers];
        for (uint i = 0; i < PlayerData.numberOfPlayers; i++)
        {
            Color color = Color.black;
            switch (i % 5)
            {
                case 0:
                    color = Color.red;
                    break;
                case 1:
                    color = Color.green;
                    break;
                case 2:
                    color = Color.blue;
                    break;
                case 3:
                    color = Color.yellow;
                    break;
                case 4:
                    color = Color.magenta;
                    break;
            }
            if (PlayerData.isAIPlayer[i])
            {
                board.Camps[i] = Camp.CreateAICamp(board, board.Tiles[(4 * i) + 2], color, PlayerData.isHardAI[i]);
            }
            else
			{
                // we want Tile 4*i + 2
                board.Camps[i] = Camp.Create(board, board.Tiles[(4 * i) + 2], color);
			}
            board.ResetTurns();
        }
        /// Allocate and assign Obstacles.
		board.Obstacles = new Obstacle[board.numberObstacles];
		for (uint i = 0; i < board.numberObstacles; i++)
			board.Obstacles[i] = Obstacle.Create(board, board.Tiles[i + 30]);
		board.Cull();
		return board;
	}

    /**
    * This is to be used to create a new Board when the root GameObject has no terrain component (taking dimensions as parameters instead of interpolating).
    * This is currently used for TestBoard (The unit test for this class).
    * @author Harry Hollands
    * @param root - GameObject which the Board should be attached to.
    * @param width - Geometric width of the Board.
    * @param height - Geometric height of the Board.
    * @param tilesWidth - Number of Tiles per row.
    * @param tilesHeight - Number of Tiles per column.
    * @return - Reference to the Board created.
    */
    public static Board CreateNoTerrain(GameObject root, float width, float height, uint tilesWidth, uint tilesHeight)
	{
		if(tilesWidth < 5 && tilesHeight < 5 && (tilesWidth * tilesHeight) < 13)
		{
			Debug.LogError("Board has invalid width/height (tile-space). One of width or height must be at least 5 AND width * height MUST be greater than 13.");
		}
		Board board = root.AddComponent<Board>();
		root.name = "Board";
		root.transform.localScale = new Vector3(width, 1, height);
		board.GetWidthInTiles = Convert.ToUInt32(tilesWidth);
		board.GetHeightInTiles = Convert.ToUInt32(tilesHeight);

		// The following code block should probably belong in Board::Start() or Board::Awake...
		// The reason is cannot be in Board::Start() is because Board::Start() is ran once the script is initialised and ready to execute, by the time multiple other scripts would have needed references to these camps etc...
		// The reason Board::Awake() cannot have this code block is because that will execute directly after "root.AddComponent<Board>()" which means before board.GetWidthInTiles is assigned.
		// Thus, the initialisation code MUST happen right here, despite being ugly.

		board.Tiles = new Tile[board.GetWidthInTiles * board.GetHeightInTiles];
        Vector2 tileSize = new Vector2(width / tilesWidth, height / tilesHeight);
		for(uint i = 0; i < board.Tiles.Length; i++)
		{
			float xTile = i % board.GetWidthInTiles;
			float zTile = i / board.GetWidthInTiles;
			board.Tiles[i] = Tile.Create(board, xTile, zTile);
			GameObject tileObject = board.Tiles[i].gameObject;
            tileObject.transform.position = new Vector3(xTile * tileSize.x, 0, zTile * tileSize.y);
            tileObject.transform.localScale = new Vector3(tileSize.x, 1, tileSize.y);
			tileObject.name = "Tile " + (i + 1);
		}
		board.numberCamps = 5;
		board.numberObstacles = 13;

		board.Camps = new Camp[board.numberCamps];
        for (uint i = 0; i < board.numberCamps; i++)
        {
            Color color = Color.black;
            switch (i % 4)
            {
                case 0:
                    color = Color.red;
                    break;
                case 1:
                    color = Color.green;
                    break;
                case 2:
                    color = Color.blue;
                    break;
                case 3:
                    color = Color.yellow;
                    break;
                default:
                    color = Color.magenta;
                    break;
            }
            board.Camps[i] = Camp.Create(board, board.Tiles[i], color);
        }
        board.Obstacles = new Obstacle[board.numberObstacles];
		for (uint i = 0; i < board.numberObstacles; i++)
			board.Obstacles[i] = Obstacle.Create(board, board.Tiles[i]);
		return board;
	}

    /**
     * @author ?
     */
	void Start()
    {

    }

    /**
    * If CampTurn is somehow broken and null (or at the beginning of the game), reset the turns.
    */
    void Update()
    {
        if (this.CampTurn == null && this.Camps != null)
            this.ResetTurns();
    }

    /**
    * Returns the Goal Tile, which will be the tile with the greatest y-coordinate (as it is at the top of the mountain).
    * @author Harry Hollands, Ciara O'Brien
    * @return - Reference to the goal Tile.
    */
    public Tile GetGoalTile()
    {
        Tile max = this.Tiles[0];
        foreach(Tile t in this.Tiles)
        {
            if (max.gameObject.transform.position.y < t.gameObject.transform.position.y)
                max = t;
        }
        return max;
    }

    /**
     * Checks which tiles should be spwaned on the terrain to form the board.
     * x and y are the tile coordinates in a 2D plane. Currently taken from their tileSpace
     * @author Aswin Mathew
     * @param x - x-coordinate of the theoretical Tile.
     * @param y - y-coordinate of the theoretical Tile.
     * @return - boolean which returns whether the theoretical Tile should exist on the game board or not.
     */
    private bool CheckTile(float x, float y)
    {
        if(y == 0)
        {
            if (x == 2 || x == 6 || x == 10 || x == 14 || x == 18)
                return true;
            else
                return false;
        }
        else if (y == 1)
        {
            return true;
        }
        else if (y == 2)
        {
            if (x == 0 || x == 4 || x == 8 || x == 12 || x == 16 || x == 20)
                return true;
            else
                return false;
        }
        else if (y == 3)
        {
            return true;
        }
        else if (y == 4 || y == 5)
        {
            if (x == 2 || x == 6 || x == 10 || x == 14 || x == 18)
                return true;
            else
                return false;
        }
        else if (y == 6)
        {
            if (x >= 2 && x <= 18)
                return true;
            else
                return false;
        }
        else if (y == 7)
        {
            if (x == 4 || x == 8 || x == 12 || x == 16)
                return true;
            else
                return false;
        }
        else if (y == 8)
        {
            if (x >= 4 && x <= 16)
                return true;
            else
                return false;
        }
        else if (y == 9 || y == 10)
        {
            if (x == 6 || x == 14)
                return true;
            else
                return false;
        }
        else if (y == 11)
        {
            if (x >= 6 && x <= 14)
                return true;
            else
                return false;
        }
        else if (y == 12)
        {
            if (x == 8 || x == 12)
                return true;
            else
                return false;
        }
        else if (y == 13)
        {
            if (x >= 8 && x <= 12)
                return true;
            else
                return false;
        }
        else if (y == 14)
        {
            if (x == 10)
                return true;
            else
                return false;
        }
        else if (y == 15)
        {
            if (x >= 4 && x <= 16)
                return true;
            else
                return false;
        }
        else if (y == 16 || y == 17 || y == 18)
        {
            if (x == 4 || x == 16)
                return true;
            else
                return false;
        }
        else if (y == 19)
        {
            if (x >= 4 && x <= 16)
                return true;
            else
                return false;
        }
        else
            return false;
    }
    /**
     * This removes all of the Tiles that do not actually belong to the game board. 
     * This transforms a grid of Tiles to the actual Chimera of Gold game board. Well, it will when it is fully implemented.
     * @author Aswin Mathew
     */
    public void Cull()
    {
        /// Packs the Board::Tiles array into the new gameTiles array.
        Tile[] gameTiles = new Tile[151];
		Tile goalTile = null;
        int j = 0;
        for (int i = 0; i < this.Tiles.Length; i++)
        {
            if (CheckTile(this.Tiles[i].PositionTileSpace.x, this.Tiles[i].PositionTileSpace.y))
            {
                gameTiles[j] = this.Tiles[i];
                j++;

            }
            if (this.Tiles[i].PositionTileSpace.y == 17 && this.Tiles[i].PositionTileSpace.x == 10)
            {
                goalTile = Tiles[i];
            }
        }
        foreach (Tile t in Tiles)
        {
            if (t != null)
                t.gameObject.SetActive(false);
        }
        foreach (Tile t in gameTiles)
        {
            if (t != null)
            {
                t.GetComponent<Renderer>().material = t.InitialMaterial;
                t.gameObject.SetActive(true);
            }
        }
        goalTile.gameObject.SetActive(true);
        goalTile.GetComponent<Renderer>().material.color = Color.yellow / 1.2f;
        goalTile.InitialMaterial = goalTile.GetComponent<Renderer>().material;
		this.Obstacles[0].gameObject.transform.position = this.GetTileByTileSpace(new Vector2(0, 3)).gameObject.transform.position;
		this.Obstacles[1].gameObject.transform.position = this.GetTileByTileSpace(new Vector2(4, 3)).gameObject.transform.position;
		this.Obstacles[2].gameObject.transform.position = this.GetTileByTileSpace(new Vector2(8, 3)).gameObject.transform.position;
		this.Obstacles[3].gameObject.transform.position = this.GetTileByTileSpace(new Vector2(12, 3)).gameObject.transform.position;
		this.Obstacles[4].gameObject.transform.position = this.GetTileByTileSpace(new Vector2(16, 3)).gameObject.transform.position;
		this.Obstacles[5].gameObject.transform.position = this.GetTileByTileSpace(new Vector2(20, 3)).gameObject.transform.position;

		this.Obstacles[6].gameObject.transform.position = this.GetTileByTileSpace(new Vector2(8, 8)).gameObject.transform.position;
		this.Obstacles[7].gameObject.transform.position = this.GetTileByTileSpace(new Vector2(12, 8)).gameObject.transform.position;

		this.Obstacles[8].gameObject.transform.position = this.GetTileByTileSpace(new Vector2(8, 11)).gameObject.transform.position;
		this.Obstacles[9].gameObject.transform.position = this.GetTileByTileSpace(new Vector2(12, 11)).gameObject.transform.position;

		this.Obstacles[10].gameObject.transform.position = this.GetTileByTileSpace(new Vector2(10, 13)).gameObject.transform.position;

		this.Obstacles[11].gameObject.transform.position = this.GetTileByTileSpace(new Vector2(10, 15)).gameObject.transform.position;

		this.Obstacles[12].gameObject.transform.position = this.GetTileByTileSpace(new Vector2(10, 19)).gameObject.transform.position;
    }

    /**
     * Gets the Tile at a specific Vector2 position. Returns null if there is no such Tile.
     * @author Harry Hollands
     * @param positionTileSpace - The position of the desired Tile.
     * @return - Reference to the Tile at the position given.
     */
    public Tile GetTileByTileSpace(Vector2 positionTileSpace)
    {
        foreach (Tile t in this.Tiles)
            if (t.PositionTileSpace == positionTileSpace)
                return t;
        return null;
    }

    /**
    * Returns true if the Tile has an obstacle ontop of it.
    * @author Harry Hollands
    * @param tile - Reference to the Tile which need be checked for an Obstacle.
    * @return - True if the Tile has an Obstacle on it, else false.
    */
    public bool TileOccupiedByObstacle(Tile tile)
    {
        return this.TileOccupiedByObstacle(tile.PositionTileSpace);
    }

    /**
     * See Board::TileOccupiedByObstacle.
     */
    public bool TileOccupiedByObstacle(Vector2 tilePosition)
    {
        foreach (Obstacle obstacle in this.Obstacles)
            if (obstacle.CurrentTile.PositionTileSpace == tilePosition)
                return true;
        return false;
    }

    /**
    * Returns true if the Tile has a player pawn ontop of it.
    * @author Yutian Xue and Zibo Zhang
    * @param tilePosition - Position of the Tile which need be checked for a player pawn.
    * @return - True if the Tile has a player pawn on it, else false.
    */
    public bool TileOccupiedByPlayerPawn(Vector2 tilePosition)
    {
        Camp tempCamp = null;
        foreach (Camp myCamp in this.Camps)
        {
            tempCamp = myCamp;
            foreach (Player myPlayer in tempCamp.TeamPlayers)
            {
                if (myPlayer.GetOccupiedTile().transform.position == GetTileByTileSpace(tilePosition).transform.position)
                    return true;
            }
        }
        return false;
    }

    /**
     * Resets the... well... turns.
     * @author Harry Hollands
     */
    public void ResetTurns()
    {
        this.CampTurn = this.Camps[0];
    }

	/**
	 * Dice is already being rolled when this is invoked. Causes the AI Player to perform its movement functionality approximately one second after the dice is predicted to hit the ground.
	 */
	private IEnumerator DelayAIMove(float seconds, Tile previousLocation, Player aiPlayer)
	{
		// s = ut + 0.5a(t^2)
		// v^2 = u^2 + 2as
		// v = u + at
		// at = v - u
		// t = (v - u) / a
		// a = -9.81 units per second^2
		// u = 0
		// s = distance between ground and dice position
		// v= sqrt(u^2 + 2 * a * s)
		float s = (this.GetDice.transform.position - previousLocation.transform.position).magnitude;
		float v = Mathf.Sqrt(2 * s * 9.81f);
		float t = v / 9.81f;
		yield return new WaitForSeconds(t + 1);
		int roll = (int) this.GetDice.NumberFaceUp();
        Tile tileDestination = this.CampTurn.ai.MovementTo(aiPlayer.GetOccupiedTile(), roll);
        aiPlayer.gameObject.transform.position = tileDestination.gameObject.transform.position + Player.POSITION_OFFSET;
		this.CampTurn.ai.MoveObstacle (tileDestination, aiPlayer);
		this.NextTurn();
	}

	/**
     * Simulates the end of the current turn and sets Board::PlayerTurn to the "next" player accordingly.
     * Also handles the AI Player's turn instantaneously and then passes over to the next player's turn.
     */
    public void NextTurn()
    {
        this.obstacleControlFlag = false;
        int campId = -1;
        // Set campId and playerId to the corresponding indices for the current Player
        for (int campCounter = 0; campCounter < this.Camps.Length; campCounter++)
        {
            if (this.CampTurn == this.Camps[campCounter])
            {
                campId = campCounter;
            }
        }
        if(campId == -1)
        {
            Debug.Log("NextTurn failed -- CampTurn is not a valid reference to a Board camp.");
            return;
        }
        if (++campId >= this.Camps.Length)
            campId = 0;
        this.CampTurn = this.Camps[campId];

        if(this.CampTurn.isAI())
        {
            Player aiPlayer = null;
            uint count = 0;
            do
            {
                if (++count > 5)
                    break;
                int index = new System.Random().Next() % 5;
                aiPlayer = this.CampTurn.TeamPlayers[index++];
                if (index > 5)
                    index = 0;
            } while (aiPlayer == null);
            Tile previousLocation = aiPlayer.GetOccupiedTile();
			this.GetDice.Roll(aiPlayer.gameObject.transform.position + new Vector3(0, 20, 0));
			StartCoroutine(DelayAIMove(2, previousLocation, aiPlayer));
        }
        this.RemoveTileHighlights();
        //consider highlighting something in some way to display the colour of the current camp turn
        //this.GetDice.GetComponent<Renderer>().material.color = this.CampTurn.TeamColor;
    }

	public Obstacle GetObstacleByTile(Tile para)
	{
		Obstacle result = null;
		foreach (Obstacle obst in this.Obstacles)
			if (obst.GetOccupiedTile () == para)
				result = obst;
		return result;
	}
	public Obstacle GetObstacleByTileSpace(Vector2 para)
	{
		Obstacle result = null;
		foreach (Obstacle obst in this.Obstacles)
			if (obst.GetOccupiedTile () == this.GetTileByTileSpace(para))
				result = obst;
		return result;
	}

	/**
    * Highlights the Tiles with the given colour.
    * @author Harry Hollands, Ciara O'Brien
    * @param color - Color of which to highlight all the active Board Tiles.
    */

    public void HighlightTiles(Color color)
    {
        foreach (Tile tile in this.Tiles)
			if(tile != null && tile.gameObject.activeSelf)
            tile.GetComponent<Renderer>().material.color = color;
    }

    /**
    * Sets the materials of all the Tiles to be equal to the Tile prefab's material.
    * @author Harry Hollands, Ciara O'Brien
    */
    public void RemoveTileHighlights()
    {
        foreach (Tile tile in this.Tiles)
            if (tile != null && tile.gameObject.activeSelf)
                tile.GetComponent<Renderer>().material = tile.InitialMaterial;
    }

	/// Getters for width and height (in units of Tiles and pixels respectively)
    public uint GetWidthInTiles{get; private set;}
	public uint GetHeightInTiles{get; private set;}
	public float GetWidthInPixels{get{return this.GetWidthInTiles * Board.ExpectedTileSize(this.gameObject, this.GetWidthInTiles, this.GetHeightInTiles).x;}}
	public float GetHeightInPixels{get{return this.GetHeightInTiles * Board.ExpectedTileSize(this.gameObject, this.GetWidthInTiles, this.GetHeightInTiles).y;}}
    public Camp CampTurn {get; private set;}

    /**
    * Uses the dimensions of the GameObject root parameter and the number of tiles to discern the size that each tile should be.
    * Note: Game.MinWorldSpace and Game.MaxWorldSpace require a Terrain component. Thus, invoking ExpectedTileSize with a root GameObject with no Terrain component WILL throw.
    * @author Harry Hollands
    * @param root - Reference to the GameObject of the root object (i.e the terrain).
    * @param widthTiles - Number of Tiles per row.
    * @param heightTiles - Number of Tiles per column.
    */
    public static Vector2 ExpectedTileSize(GameObject root, float widthTiles, float heightTiles)
	{
		Vector3 min = Game.MinWorldSpace(root), max = Game.MaxWorldSpace(root);
		float deltaX = max.x - min.x, deltaZ = max.z - min.z;
		return new Vector2(deltaX / widthTiles, deltaZ / heightTiles);
	}
}