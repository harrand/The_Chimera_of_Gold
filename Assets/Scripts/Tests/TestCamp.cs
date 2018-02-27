using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This performs tests that are relevant to the camps
 * @author Ciara O'Brien, Harry Hollands
 */
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

	/**
	 * this checks if the player has correctly been spawned 
	 * @author Harry Hollands, Ciara O'Brien
	 */
    public bool TestSpawnPlayer(Player expected)
    {
        return this.campScript.SpawnPlayer() == expected;
    }

	/**
	 * This checks that the correct number of players has been returned
	 * @author Harry Hollands, Ciara O'Brien
	 */
    public bool TestGetNumberPlayers(uint expected)
    {
        return this.campScript.GetNumberOfPlayers() == expected;
    }
}
