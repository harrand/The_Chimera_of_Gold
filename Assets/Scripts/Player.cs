using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	private Board parent;
    
    /**
     * Pseudo-constructor which uses the Prefabs/Player prefab in the project tree.
     */
    public static Player Create(Board parent, Tile tilePosition)
    {
        Vector3 offset = new Vector3(0, 3, 0);
        GameObject playerObject = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
        Player playerScript = playerObject.AddComponent<Player>();
        playerScript.parent = parent;
		playerObject.transform.position = tilePosition.gameObject.transform.position + offset;
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
    * Returns the Tile that the player is sitting upon.
    */
    public Tile GetOccupiedTile()
    {
        Vector3 noOffset = this.gameObject.transform.position - new Vector3(0, 3, 0);
        foreach (Tile tile in this.parent.Tiles)
            if (tile.gameObject.transform.position == noOffset)
                return tile;
        return null;
    }
}
