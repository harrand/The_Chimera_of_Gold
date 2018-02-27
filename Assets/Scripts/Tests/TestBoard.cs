using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * TestBoard - Completely tests the Board Functionalities.
 * Note: TestBoard does NOT have clickable Tiles.
 * @author Harry Hollands, Ciara O'Brien
 */
public class TestBoard : TestBase
{
	private Board boardScript;
	private float width, height;
	private uint tileWidth, tileHeight;
	/**
	 * TestBoard
	 * @Author Harry Hollands
	 * Instantiates a new GameObject, attaches the Board script and ensures that the Board constructor modifies the name.
	 */
	public TestBoard(float width, float height, uint tileWidth, uint tileHeight)
	{
        TerrainData data = new TerrainData();
        data.size = new Vector3(width, 1, height);
		this.boardScript = Board.CreateNoTerrain(Terrain.CreateTerrainGameObject(data), width, height, tileWidth, tileHeight);
		this.width = width;
		this.height = height;
		this.tileWidth = tileWidth;
		this.tileHeight = tileHeight;

		// Chain it like this to ensure that compiler does not optimise away any of the checks.
		this.success = this.TestDimensions();
		this.success &= this.TestTiles();
		this.success &= this.TestCamps();
		this.success &= this.TestObstacles();
		this.success &= this.TestTurns();
	}

	/**
	 * TestDimensions
	 * @Author Harry Hollands
	 * Passes if the boardObject has the same dimensions as the Board script expects.
	 */
	public bool TestDimensions()
	{
		Transform boardTransform = this.boardScript.gameObject.transform;
		bool geometricMatch = (boardTransform.localScale == new Vector3(this.width, 1, this.height));
		if(!geometricMatch)
		{
			Debug.Log("Test Failed -- Geometric width and height of Board's GameObject does not match the parameters of the Board.");
			return false;
		}
		bool tileMatch = this.boardScript.GetWidthInTiles == this.tileWidth && this.boardScript.GetHeightInTiles == this.tileHeight;
		if(!tileMatch)
		{
			Debug.Log("Test Failed -- Width and height (in tile-space) variables of the Board do not match the inputs it was given.");
			return false;
		}
		return true;
	}

	/**
	 * TestTiles
	 * @Author Harry Hollands
	 * Passes if the Board has the expected number of Tiles and none of them are null references.
	 * The Tiles must all also be in the correct positions for a grid.
	 */
	public bool TestTiles()
	{
		uint expectedLength = this.boardScript.GetWidthInTiles * this.boardScript.GetHeightInTiles;
		if (this.boardScript.Tiles.Length != expectedLength) 
		{
			Debug.Log ("Test Failed -- Incorrect number of tiles (should be " + expectedLength + " )");
			return false;
		}
		for(uint x = 0; x < this.boardScript.GetWidthInTiles; x++)
		{
			for(uint z = 0; z < this.boardScript.GetHeightInTiles; z++)
			{
                Tile currentTile = this.boardScript.Tiles[x + (z * this.boardScript.GetWidthInTiles)];
                Vector2 currentExpectedTileSize = Board.ExpectedTileSize(this.boardScript.gameObject, this.boardScript.GetWidthInTiles, this.boardScript.GetHeightInTiles);
                if (currentTile.gameObject.transform.position != (new Vector3 (x * currentExpectedTileSize.x, 0, z * currentExpectedTileSize.y))) 
				{
					Debug.Log ("Test Failed -- Tiles do not represent a grid format");
					return false;
				}
			}
		}
		return true;
	}

	/**
	 * TestCamps
	 * @Author Ciara O'Brien
	 * Checks the validity of the camps on the board - ensuring the correct number of camps and none are NULL
	 */
	public bool TestCamps()
	{
		if (this.boardScript.Camps.Length != Game.NUMBER_CAMPS) 
		{
			Debug.Log ("Test Failed -- Incorrect number of camps (should be " + Game.NUMBER_CAMPS + ")");
			return false;
		}

		foreach (Camp camp in this.boardScript.Camps)
			if (camp == null) 
			{
				Debug.Log ("Test Failed -- One or more null Camp entry");
				return false;
			}

		return true;	
	}

	/**
	 * 
	 * TestObstacles
	 * @Author Ciara O'Brien
	 * Checks the validity of the obstacles on the board - ensuring the correct number and none are NULL
	 * 
	 */
	public bool TestObstacles()
	{
		if (this.boardScript.Obstacles.Length != Game.NUMBER_OBSTACLES) 
		{
			Debug.Log ("Test Failed -- Incorrect number of obstacles (should be " + Game.NUMBER_OBSTACLES + ")");
			return false;
		}

		foreach (Obstacle obstacle in this.boardScript.Obstacles)
			if (obstacle == null) 
			{
				Debug.Log ("Test Failed -- One or more null obstacle entry");
				return false;
			}

		return true;	
	}

    /**
	 * TestObstacles
	 * @Author Harry Hollands, Lawrence Howes-Yarlett & Ciara O'Brien
     * Ensures that the turn system of the Board is implemented correctly for every single turn state.
     */
    public bool TestTurns()
    {
        this.boardScript.ResetTurns();
        int numberCamps = this.boardScript.Camps.Length;
        int numberPlayersPerCamp = this.boardScript.Camps[0].TeamPlayers.Length;
		for(uint i = 0; i < numberCamps; i++)
		{
            Camp expectedCurrentCamp = this.boardScript.Camps[i];
            if(expectedCurrentCamp != this.boardScript.CampTurn)
            {
                Debug.Log("Test Failed -- Camp turn selection invalid.");
                return false;
            }
            this.boardScript.NextTurn();
        }
        return true;
    }
}