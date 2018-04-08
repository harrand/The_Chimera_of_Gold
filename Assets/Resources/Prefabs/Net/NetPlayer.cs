using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class NetPlayer : NetworkBehaviour {
    public static Vector3 POSITION_OFFSET = new Vector3(0, 0.6f, 0);
    private NetBoard parent;

    private void Start()
    {
        parent= GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetBoard>();
    }
    /**
     * Destroys the player's gameobject, effectively removing it.
     * @author Harry Hollands
     */
    public void Kill()
    {
        //this.GetCamp().TeamPlayers[Array.IndexOf(this.GetCamp().TeamPlayers, this)] = null;
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
            if (obst.GetOccupiedTile() == this.GetOccupiedTile() && this.parent.netObstacleControlFlag)
                return obst;
        return null;
    }
}
