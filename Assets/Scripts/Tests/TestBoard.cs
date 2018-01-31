using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoard : TestBase
{
	private GameObject boardObject;
	private Board boardScript;
	/**
	 * Instantiates a new GameObject, attaches the Board script and ensures that the Board constructor modifies the name.
	 */
	public TestBoard()
	{
		this.boardObject = new GameObject();
		this.boardObject.name = "Test Object";
		// A successful Board will be renamed to "Board" by its script.
		this.boardScript = this.boardObject.AddComponent<Board>();
		this.success = this.TestName() && this.TestDimensions() && this.TestTiles();
	}

	/**
	 * Passes if the boardObject was renamed to "Board"
	 */
	public bool TestName()
	{
		return this.boardObject.name == "Board";
	}

	/**
	 * Passes if the boardObject has the same dimensions as the Board script expects.
	 */
	public bool TestDimensions()
	{
		return (this.boardObject.transform.localScale.x == this.boardScript.GetWidthInTiles) && (this.boardObject.transform.localScale.z == this.boardScript.GetHeightInTiles);
	}

	/**
	 * Passes if the Board has the expected number of Tiles and none of them are null references.
	 */
	public bool TestTiles()
	{
		if(this.boardScript.GetTiles.Length != (this.boardScript.GetWidthInTiles * this.boardScript.GetHeightInTiles))
			return false;
		foreach(Tile tile in this.boardScript.GetTiles)
		{
			if(tile == null)
				return false;
		}
		return true;
	}
}