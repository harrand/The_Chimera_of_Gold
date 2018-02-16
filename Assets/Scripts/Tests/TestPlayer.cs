using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : TestBase
{
    private Player playerScript;

	public TestPlayer() 
	{
        GameObject boardObject = new GameObject();
        Board testBoard = Board.CreateNoTerrain(boardObject, 800, 800, 10, 10);
        this.playerScript = testBoard.Camps[0].TeamPlayers[0];
        this.success = this.TestGetCamp(testBoard.Camps[0]);
	}

    public bool TestGetCamp(Camp expected)
    {
        if (this.playerScript.GetCamp() == expected)
            return true;
        Debug.Log("Test failed -- GetCamp did not equal the expected parameter. Expected " + expected + " but the parent is " + this.playerScript.GetCamp());
        return false;
    }
}
