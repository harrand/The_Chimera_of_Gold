using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour
{
    private Board parent;
    private Tile tile;
    public Vector2 PositionTileSpace { get; private set; }
    private uint numberPlayers;
	public Player[] TeamPlayers{get; private set;}

    /**
     * Pseudo-constructor which uses the Prefabs/Camp prefab in the project tree.
     */
    public static Camp Create(Board parent, Tile tile)
    {
        GameObject campObject = Instantiate(Resources.Load("Prefabs/Camp")) as GameObject;
        Camp campScript = campObject.AddComponent<Camp>();
        campScript.parent = parent;
        campScript.tile = tile;
        campScript.PositionTileSpace = tile.PositionTileSpace;
		campObject.transform.position = tile.gameObject.transform.position;
        return campScript;
    }

    void Awake()
    {
		// Define Player array size.
        this.numberPlayers = 5;
        this.TeamPlayers = new Player[this.numberPlayers];
    }

	void Start()
	{
		// Instantiate all Players in the camp.
		for(uint i = 0; i < this.numberPlayers; i++)
		{
			this.TeamPlayers[i] = Player.Create(this.parent, this.tile);
		}
	}

	// Do not implement until test is written.
    public Player SpawnPlayer()
	{
        return null;
	}

	public uint GetNumberPlayers()
	{
		return numberPlayers;
	}

}
