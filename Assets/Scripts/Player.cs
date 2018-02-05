using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	private Board parent;
    public Tile CurrentTile { get; private set; }

    /**
     * Pseudo-constructor which uses the Prefabs/Player prefab in the project tree.
     */
    public static Player Create(Board parent, Tile tilePosition)
    {
        GameObject playerObject = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
        Player playerScript = playerObject.AddComponent<Player>();
        playerScript.parent = parent;
        playerScript.CurrentTile = tilePosition;
        return playerScript;
    }

    // Use this for initialization
    void Start () 
	{

	}

}
