using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/**
 * This class holds all of the information for the camps
* @author Harry Hollands
*/
public class Camp : NetworkBehaviour
{
    private Board parent;
    private Tile tile;
    public Vector2 PositionTileSpace { get; private set; }
    private uint numberPlayers;
	public Player[] TeamPlayers{get; private set;}
    public Color TeamColor { get; set; }
    public DecisionTree ai { get; private set; }
    public bool rolled = false;

    /**
     * Pseudo-constructor which uses the Prefabs/Camp prefab in the project tree.
     * @author Harry Hollands
     * @param parent holds the parent board to be edited
     * @param tile holds the current tile to create a camp and players for that camp
     * @param teamColour The colour to set the camp and its players to initially
     * @param playerId Used to figure out which player the camp belongs to
     * @return the camp created 
     */
    public static Camp Create(Board parent, Tile tile, Color teamColour, uint playerId)
    {
        GameObject campObject = Instantiate(Resources.Load("Prefabs/camp1")) as GameObject;
        Camp campScript = campObject.AddComponent<Camp>();
        campScript.TeamColor = teamColour;
        campScript.gameObject.GetComponent<Renderer>().material.color = campScript.TeamColor;
        campScript.parent = parent;
        campScript.tile = tile;
        campScript.PositionTileSpace = tile.PositionTileSpace;
		campObject.transform.position = tile.gameObject.transform.position;
        campScript.ai = null;
        // Define Player array size.
        campScript.numberPlayers = 5;
        campScript.TeamPlayers = new Player[campScript.numberPlayers];
        // Instantiate all Players in the camp.
        for (uint i = 0; i < campScript.numberPlayers; i++)
        {
            campScript.SpawnPlayer(playerId);
        }
        return campScript;
    }

    public static Camp CreateAICamp(Board parent, Tile tile, Color teamColour, bool hardMode, uint playerId)
    {
        Camp camp = Camp.Create(parent, tile, teamColour, playerId);
        //camp.ai = camp.gameObject.AddComponent<DecisionTree>();
        camp.ai = DecisionTree.Create(parent, camp, hardMode);
        camp.ai.board = camp.GetParent();
        Debug.Log("created " + (hardMode ? "hard" : "easy") + " mode AI player.");
        return camp;
    }

    public bool isAI()
    {
        return this.ai != null;
    }

    private void Start()
    {
        foreach(Player player in this.TeamPlayers)
            if (player != null)
                player.gameObject.GetComponent<Renderer>().material.color = this.TeamColor;
    }

    /**
	 * Returns number of active players belonging to the camp.
	 * @author Harry Hollands
	 * @return the unit representing how many players there are
	 */
    public uint GetNumberOfPlayers()
    {
        uint counter = 0;
        foreach (Player p in this.TeamPlayers)
            if (p != null)
                counter++;
        return counter;
    }

	/**
	 * This spawn a player under the current camp, checking if the max number has been exceeded
	 * @author Harry Hollands
	 * @return the player that has been created
	 */
    public Player SpawnPlayer(uint playerId)
	{
        if(this.GetNumberOfPlayers() < this.numberPlayers)
        {
            this.TeamPlayers[this.GetNumberOfPlayers()] = Player.Create(this.parent, this.tile, playerId);
            return this.TeamPlayers[this.GetNumberOfPlayers() - 1];
        }
        Debug.Log("Camp tried to spawn Player but already has the maximum number of spawned players active.");
        return null;
	}

    /**
     * Returns a reference to the Tile that the camp is ontop of. Returns null if there is no such tile.
     * @author Harry Hollands
     * @return the tile that the camp is on top of
     */
    public Tile GetOccupiedTile()
    {
		foreach(Tile tile in this.parent.Tiles)
			if(tile.gameObject.transform.position == this.gameObject.transform.position)
				return tile;
		return null;
    }

	/**
	 * This returns the parent board of the camp
	 * @author Harry Hollands
	 * @return the parent board of this camp
	 */
    public Board GetParent()
    {
        return this.parent;
    }

	/**
	 * This returns the index of the camp in its parent board
	 * @author Zibo Zhang
	 * @return the camp index of the board
	 */
	public int GetCampIndex()
	{
		int count = 0;
		foreach(Camp camp in this.parent.Camps)
		{
			if(camp == this)
				return count;
			
			count++;
		}
		return -1;
	}
}
