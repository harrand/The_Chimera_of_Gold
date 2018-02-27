using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * TestTile - Completely tests the Tiles functionalities
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

	/**
	 * This checks if there is an occupant on the tile, therefore checking if this has been done correctly
	 * @author Harry Hollands
	 * @param exepected the expected boolean value returned 
	 */
    bool TestHasOccupant(bool expected)
    {
        if (this.tileScript.HasOccupant() == expected)
            return true;
        Debug.Log("Test failed -- Tile::HasOccupant() did not return " + expected + " as expected; it returned " + this.tileScript.HasOccupant());
        return false;
    }

	/**
	 * This checks if the get ocupant function works, checking if it returns the correct value
	 * @author Harry Hollands
	 * @param expected the expected gameobject returned when checking for the occupant
	 */
    bool TestGetOccupant(GameObject expected)
    {
        if (this.tileScript.GetOccupant() == expected)
            return true;
        Debug.Log("Test failed -- Tile::GetOccupant() did not return " + expected + " as expected; it returned " + this.tileScript.GetOccupant());
        return false;
    }
}
