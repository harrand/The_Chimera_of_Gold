using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoard : TestBase
{
	private GameObject boardObject;
	/**
	 * Instantiates a new GameObject, attaches the Board script and ensures that the Board constructor modifies the name.
	 */
	public TestBoard()
	{
		this.boardObject = new GameObject();
		this.boardObject.name = "Test Object";
		// A successful Board will be renamed to "Board" by its script.
		this.boardObject.AddComponent<Board>();
		this.success = (this.boardObject.name == "Board");
	}
}