using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * TestBoard - Completely tests the Board Functionalities.
 */

public class TestTile : TestBase
{
    private Tile tileScript;
    private GameObject tileObject;
    /**
    * TestBoard
    * @Author Harry Hollands
    * Instantiates a new GameObject, attaches the Board script and ensures that the Board constructor modifies the name.
    */
    public TestTile()
    {
        this.tileScript = Tile.Create(null, 0, 0);
        this.tileObject = this.tileScript.gameObject;
	}


    // This cannot be implemented until we know both where the players are on the board and also where the obstacles are.
    bool HasOccupant()
    {
        return false;
    }

    GameObject GetOccupant()
    {
        return null;
    }
}
