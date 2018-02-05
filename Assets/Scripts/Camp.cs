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
        return campScript;
    }

    // Use this for initialization
    void Awake()
    {
        this.numberPlayers = 5;
        this.TeamPlayers = new Player[this.numberPlayers];
        for(uint i = 0; i < this.numberPlayers; i++)
        {
            this.TeamPlayers[i] = Player.Create(this.parent, this.tile);
        }
    }
    /*
	void Start (Board board, uint noPlayers) 
	{
		parent = board;
		numberPlayers = noPlayers;
		teamPlayers = new Player[numberPlayers];
	}
    */

    public Player SpawnPlayer()
	{
        return null;
	}

	public uint GetNumberPlayers()
	{
		return numberPlayers;
	}

}
