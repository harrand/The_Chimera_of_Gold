using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTile : TestBase
{
    private Tile tileScript;
    private GameObject tileObject;
	// Use this for initialization
	public TestTile()
    {
        var pair = Tile.Create(null, 0, 0);
        this.tileScript = pair.Key;
        this.tileObject = pair.Value;
	}
}
