using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	private Board parent;
    public Tile CurrentTile { get; set; }
    
    /**
     * Pseudo-constructor which uses the Prefabs/Player prefab in the project tree.
     */
    public static Player Create(Board parent, Tile tilePosition)
    {
        Vector3 offset = new Vector3(0, 3, 0);
        GameObject playerObject = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
        Player playerScript = playerObject.AddComponent<Player>();
        playerScript.parent = parent;
        playerScript.CurrentTile = tilePosition;
		playerObject.transform.position = tilePosition.gameObject.transform.position + offset;
        return playerScript;
    }

    // Use this for initialization
    void Start () 
	{

	}

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
}
