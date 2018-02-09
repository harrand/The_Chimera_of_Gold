using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamp : TestBase
{
	private Camp campScript;

	public TestCamp()
	{
        //this.campScript = Camp.Create(null, null);

        this.success = this.TestSpawnPlayer();
        this.success &= this.TestGetNumberPlayers();
	}

    public bool TestSpawnPlayer()
    {
        return false;
    }

    public bool TestGetNumberPlayers()
    {
        return false;
    }
}
