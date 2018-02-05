using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * TestBoard - Completely tests the Board Functionalities.
 */

public class TestBoard : TestBase
{
	private GameObject boardObject;
	private Board boardScript;
	/**
	 * TestBoard
	 * @Author Harry Hollands
	 * Instantiates a new GameObject, attaches the Board script and ensures that the Board constructor modifies the name.
	 */
	public TestBoard()
	{
		this.boardObject = new GameObject();
		this.boardObject.transform.localScale = new Vector3(9, 9, 9);
		this.boardScript = this.boardObject.AddComponent<Board>();

		this.TestDimensions ();
		this.TestTiles ();
		this.TestCamps ();
		this.TestObstacles ();
        this.TestTurns();

		this.success = this.TestDimensions() && this.TestTiles() && this.TestCamps() && this.TestObstacles() && this.TestTurns();

	}

	/**
	 * TestDimensions
	 * @Author Harry Hollands
	 * Passes if the boardObject has the same dimensions as the Board script expects.
	 */
	public bool TestDimensions()
	{
		return (this.boardObject.transform.localScale.x == this.boardScript.GetWidthInTiles) && (this.boardObject.transform.localScale.z == this.boardScript.GetHeightInTiles);
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
			Debug.Log ("Test Failed -- Incorrect number of tiles (should be "+expectedLength+" )");
			return false;
		}
		for(uint x = 0; x < this.boardScript.GetWidthInTiles; x++)
		{
			for(uint z = 0; z < this.boardScript.GetHeightInTiles; z++)
			{
				if (this.boardScript.Tiles [x + (z * this.boardScript.GetWidthInTiles)].gameObject.transform.position != (new Vector3 (x, 0, z) * Game.TILE_SIZE)) 
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
	 * @Author Harry Hollands, Lawrence Howes-Yarlett (https://www.facebook.com/lawrence.howesyarlett) & Ciara O'Brien
     * Ensures that the turn system of the Board is implemented correctly for every single turn state.
     */
    public bool TestTurns()
    {
        this.boardScript.ResetTurns();
        int numberCamps = this.boardScript.Camps.Length;
        int numberPlayersPerCamp = this.boardScript.Camps[0].TeamPlayers.Length;
        for(uint i = 0; i < (numberCamps * numberPlayersPerCamp); i++)
        {
            uint currentCamp = (uint)Math.Ceiling((double)i / numberPlayersPerCamp);
            int currentPlayerInCamp = (int) i % numberPlayersPerCamp;
            Player expectedPlayer = this.boardScript.Camps[currentCamp].TeamPlayers[currentPlayerInCamp];
            if(expectedPlayer != this.boardScript.PlayerTurn)
            {
                Debug.Log("Test Failed -- Player turn selection invalid.");
                return false;
            }
            this.boardScript.NextTurn();
        }
        return true;
    }
}