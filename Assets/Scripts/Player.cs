using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour 
{
	public static Vector3 POSITION_OFFSET = new Vector3(0, 3, 0);
	private Board parent;
    
    /**
     * Pseudo-constructor which uses the Prefabs/Player prefab in the project tree.
     */
    public static Player Create(Board parent, Tile tilePosition)
    {
        GameObject playerObject = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
        Player playerScript = playerObject.AddComponent<Player>();
        playerScript.parent = parent;
		playerObject.transform.position = tilePosition.gameObject.transform.position + Player.POSITION_OFFSET;
        playerScript.gameObject.AddComponent<NetworkIdentity>();
        playerScript.gameObject.AddComponent<NetworkTransform>();
        //playerScript.gameObject.GetComponent<NetworkIdentity>()
        return playerScript;
    }

    /**
    * Returns a reference to the Camp which holds this Player.
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
     */
    public void Kill()
    {
        Destroy(this.gameObject);
    }

    /**
    * Returns the Tile that the player is sitting upon.
    */
    public Tile GetOccupiedTile()
    {
        Vector3 noOffset = this.gameObject.transform.position - Player.POSITION_OFFSET;
        foreach (Tile tile in this.parent.Tiles)
            if (tile.gameObject.transform.position == noOffset)
                return tile;
        return null;
    }

    public bool HasControlledObstacle()
    {
        return this.GetControlledObstacle() != null;
    }

    /**
    * Returns the obstacle that this player is standing on and therefore would be controlling.
    * Returns null if there is no such obstacle.
    */
    public Obstacle GetControlledObstacle()
    {
        foreach (Obstacle obst in this.parent.Obstacles)
            if (obst.GetOccupiedTile() == this.GetOccupiedTile() && this.parent.obstacleControlFlag)
                return obst;
        return null;
    }
}
