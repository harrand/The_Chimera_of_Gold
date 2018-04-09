using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

/**
* Player contains all the data and functionality for a Pawn in the Chimera of Gold.
* @author Harry Hollands, Ciara O'Brien, Aswin Mathew
*/
public class Player :  NetworkBehaviour

{
	public static Vector3 POSITION_OFFSET = new Vector3(0, 0.6f, 0);
	private Board parent;
    //Aswin- Soz, plaster to get a smooth pawn movement transition
    public Vector3 origin, current;

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

        playerScript.origin = playerObject.transform.position;
        playerScript.current = playerScript.origin;

        return playerScript;
    }
    private void Update()
    {
        if(current != origin)
            Move();
    }
    public void Move()
    {
        this.gameObject.transform.position = Vector3.Lerp(current, origin, Time.deltaTime * 1.5f);
        current = this.gameObject.transform.position;
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
        this.GetCamp().TeamPlayers[Array.IndexOf(this.GetCamp().TeamPlayers, this)] = null;
        Destroy(this.gameObject);
    }

    public uint GoalScore()
    {
        return Convert.ToUInt32(this.GetOccupiedTile().PositionTileSpace.y);
    }

    /**
    * Returns the Tile that the player is sitting upon.
    * @author Harry Hollands
    * @return - Reference to the Tile on which this Player rests.
    */
    public Tile GetOccupiedTile()
    {
        Vector3 noOffset = this.origin - Player.POSITION_OFFSET;
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

	/**
    * Returns an index of this Player in its camp.
    * @author Zibo Zhang
    * @return - index of the player to the parent Camp.
    */
	public int GetPlayerIndex()
	{
		int count = 0;
		foreach (Player player in this.GetCamp().TeamPlayers) 
		{
			if (player == this)
				return count;
			count++;
		}
		return -1;
	}
}