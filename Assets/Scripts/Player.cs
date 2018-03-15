using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/**
* Player contains all the data and functionality for a Pawn in the Chimera of Gold.
* @author Harry Hollands, Ciara O'Brien, Aswin Mathew
*/
public class Player :  NetworkBehaviour

{
	public static Vector3 POSITION_OFFSET = new Vector3(0, 3, 0);
	private Board parent;

    /**
     * Pseudo-constructor which uses the Prefabs/Player prefab in the project tree.
     * @author Harry Hollands
     * @param parent - Reference to the parent Board.
     * @param tilePosition - Reference to the Tile to create the Player unto.
     * @return - Reference to the Player created.
     */
    public static Player Create(Board parent, Tile tilePosition, uint playerId)
    {
        Debug.Log(playerId);
        GameObject playerObject = Instantiate(Resources.Load("Prefabs/pawn"+ playerId)) as GameObject;
        Player playerScript = playerObject.AddComponent<Player>();
        playerScript.parent = parent;
		playerObject.transform.position = tilePosition.gameObject.transform.position + Player.POSITION_OFFSET;
        
        return playerScript;
    }

    /**
    * Returns a reference to the Camp which holds this Player.
    * @author Harry Hollands
    * @return - Reference to the parent Camp.
    */
    public Camp GetCamp()
	{
		foreach(Camp camp in this.parent.Camps)
		{
			foreach(Player player in camp.TeamPlayers)
				if(player == this)
					return camp;
		}
		return null;
	}

    /**
     * Destroys the player's gameobject, effectively removing it.
     * @author Harry Hollands
     */
    public void Kill()
    {
        uint thisIndex = 0;
        for(uint index = 0; index < 5; index++)
        {
            if (this.GetCamp().TeamPlayers[index] == this)
                thisIndex = index;
        }
        Destroy(this.gameObject);
        this.GetCamp().TeamPlayers[thisIndex] = null;
    }

    /**
    * Returns the Tile that the player is sitting upon.
    * @author Harry Hollands
    * @return - Reference to the Tile on which this Player rests.
    */
    public Tile GetOccupiedTile()
    {
        Vector3 noOffset = this.gameObject.transform.position - Player.POSITION_OFFSET;
        foreach (Tile tile in this.parent.Tiles)
            if (tile.gameObject.transform.position == noOffset)
                return tile;
        return null;
    }

    /**
    * @author Harry Hollands
    * @return - True if the Player is currently controlling an Obstacle, else false.
    */
    public bool HasControlledObstacle()
    {
        return this.GetControlledObstacle() != null;
    }

    /**
    * Returns the obstacle that this player is standing on and therefore would be controlling.
    * @author Harry Hollands
    * @return - Reference to the Obstacle that the player is controlling, or null if there is no such obstacle.
    */
    public Obstacle GetControlledObstacle()
    {
        foreach (Obstacle obst in this.parent.Obstacles)
            if (obst.GetOccupiedTile() == this.GetOccupiedTile() && this.parent.obstacleControlFlag)
                return obst;
        return null;
    }
}
