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

        // Define Player array size.
        campScript.numberPlayers = 5;
        campScript.TeamPlayers = new Player[campScript.numberPlayers];
        // Instantiate all Players in the camp.
        for (uint i = 0; i < campScript.numberPlayers; i++)
        {
            campScript.TeamPlayers[i] = Player.Create(campScript.parent, campScript.tile);
        }
        return campScript;
    }

    void Awake()
    {
		
    }

	void Start()
	{
		
	}

    public uint GetNumberOfPlayers()
    {
        uint counter = 0;
        foreach (Player p in this.TeamPlayers)
            if (p != null)
                counter++;
        return counter;
    }

	// Do not implement until test is written.
    public Player SpawnPlayer()
	{
        if(this.GetNumberOfPlayers() < this.numberPlayers)
        {
            this.TeamPlayers[this.GetNumberOfPlayers()] = Player.Create(this.parent, this.tile);
        }
        Debug.Log("Camp tried to spawn Player but already has the maximum number of spawned players active.");
        return null;
	}

    public Tile GetOccupiedTile()
    {
        foreach(Tile tile in this.parent.Tiles)
        {
            if (this.gameObject == tile.GetOccupant())
                return tile;
        }
        return null;
    }

    public Board GetParent()
    {
        return this.parent;
    }
}
