using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoard : TestBase
{
	/**
	 * Instantiates a new GameObject, attaches the Board script and ensures that the Board constructor modifies the name.
	 */
	public TestBoard()
	{
		GameObject boardObject = new GameObject();
		boardObject.name = "Test Object";
		boardObject.AddComponent<Board>();
		this.success = boardObject.name == "Board";
	}
}