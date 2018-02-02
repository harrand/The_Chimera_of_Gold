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
        var pair = Tile.Create(null, 0, 0);
        this.tileScript = pair.Key;
        this.tileObject = pair.Value;
	}
}
