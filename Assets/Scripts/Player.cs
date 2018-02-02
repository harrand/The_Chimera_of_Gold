using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	private Board parent;
    private Tile currentTile;

    public static Player Create(Board parent, Tile tilePosition)
    {
        GameObject playerObject = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
        Player playerScript = playerObject.AddComponent<Player>();
        playerScript.parent = parent;
        playerScript.currentTile = tilePosition;
        return playerScript;
    }

    // Use this for initialization
    void Start () 
	{

	}
	
	public Tile GetOccupiedTile()
	{
        return null;
	}

}
