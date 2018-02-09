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
        this.tileScript = Tile.Create(null, 0, 0);

        this.success = this.TestHasOccupant();
        this.success &= this.TestGetOccupant();
	}


    // This cannot be implemented until we know both where the players are on the board and also where the obstacles are.
    bool TestHasOccupant()
    {
        return false;
    }

    bool TestGetOccupant()
    {
        return false;
    }
}
