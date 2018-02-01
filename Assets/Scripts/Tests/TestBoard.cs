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
		this.boardObject.transform.localScale = new Vector3(3, 3, 3);
		this.boardScript = this.boardObject.AddComponent<Board>();


		this.TestDimensions ();
		this.TestTiles ();
		this.TestCamps ();
		this.TestObstacles ();

		this.success = this.TestDimensions() && this.TestTiles() && this.TestCamps() && this.TestObstacles();

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
	 * The Tiles must all also be in the correct positions for a grid.
	 */
	public bool TestTiles()
	{
		uint expectedLength = this.boardScript.GetWidthInTiles * this.boardScript.GetHeightInTiles;
		if (this.boardScript.GetTiles.Length != expectedLength) 
		{
			Debug.Log ("Test Failed -- Incorrect number of tiles (should be "+expectedLength+" )");
			return false;
		}
		for(uint x = 0; x < this.boardScript.GetWidthInTiles; x++)
		{
			for(uint z = 0; z < this.boardScript.GetHeightInTiles; z++)
			{
				if (this.boardScript.GetTiles [x + (z * this.boardScript.GetWidthInTiles)].gameObject.transform.position != (new Vector3 (x, 0, z) * Game.TILE_SIZE)) 
				{
					Debug.Log ("Test Failed -- Tiles do not represent a grid format");
					return false;
				}
			}
		}
		return true;
	}

	public bool TestCamps()
	{
		if (this.boardScript.GetCamps.Length != 5) 
		{
			Debug.Log ("Test Failed -- Incorrect number of camps (should be 5)");
			return false;
		}

		foreach (Camp camp in this.boardScript.GetCamps)
			if (camp == null) 
			{
				Debug.Log ("Test Failed -- NULL Camp entry");
				return false;
			}

		return true;	
	}

	public bool TestObstacles()
	{
		if (this.boardScript.GetObstacles.Length != 13) 
		{
			Debug.Log ("Test Failed -- Incorrect number of obstacles (should be 13)");
			return false;
		}

		foreach (Obstacle obstacle in this.boardScript.GetObstacles)
			if (obstacle == null) 
			{
				Debug.Log ("Test Failed -- NULL obstacle entry");
				return false;
			}

		return true;	
	}
}