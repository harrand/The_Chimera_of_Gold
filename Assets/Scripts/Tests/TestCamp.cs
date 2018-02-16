using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamp : TestBase
{
	private Camp campScript;

	public TestCamp()
	{
        GameObject boardObject = new GameObject();
        Board testBoard = Board.CreateNoTerrain(boardObject, 800, 800, 10, 10);
        this.campScript = testBoard.Camps[0];

        // spawnplayer should be null first
        this.success = this.TestSpawnPlayer(null);
        this.success &= this.TestGetNumberPlayers(5);
	}

    public bool TestSpawnPlayer(Player expected)
    {
        return this.campScript.SpawnPlayer() == expected;
    }

    public bool TestGetNumberPlayers(uint expected)
    {
        return this.campScript.GetNumberOfPlayers() == expected;
    }
}
