using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * TestBoard - Completely tests the Board Functionalities.
 */

public class TestTile : TestBase
{
    private Tile tileScript;
    /**
    * TestBoard
    * @Author Harry Hollands
    * Instantiates a new GameObject, attaches the Board script and ensures that the Board constructor modifies the name.
    */
    public TestTile()
    {
        GameObject boardObject = new GameObject();
        Board testBoard = Board.CreateNoTerrain(boardObject, 800, 800, 10, 10);
        this.tileScript = testBoard.Tiles[0];

        this.success = this.TestHasOccupant(false);
        this.success &= this.TestGetOccupant(null);
	}


    // This cannot be implemented until we know both where the players are on the board and also where the obstacles are.
    bool TestHasOccupant(bool expected)
    {
        if (this.tileScript.HasOccupant() == expected)
            return true;
        Debug.Log("Test failed -- Tile::HasOccupant() did not return " + expected + " as expected; it returned " + this.tileScript.HasOccupant());
        return false;
    }

    bool TestGetOccupant(GameObject expected)
    {
        if (this.tileScript.GetOccupant() == expected)
            return true;
        Debug.Log("Test failed -- Tile::GetOccupant() did not return " + expected + " as expected; it returned " + this.tileScript.GetOccupant());
        return false;
    }
}
