using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : TestBase
{
    private Player playerScript;

	public TestPlayer() 
	{
        this.success = this.TestOccupiedTile();
	}

    bool TestOccupiedTile()
    {
        return false;
    }
}
