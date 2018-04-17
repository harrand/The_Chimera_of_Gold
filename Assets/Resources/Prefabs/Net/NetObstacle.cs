using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetObstacle : MonoBehaviour {

    public static Vector3 POSITION_OFFSET = new Vector3(0, 1.35f, 0);
    private NetBoard parent;
    public NetTile CurrentTile { get; private set; }
    /**
     * Pseudo-constructor which uses the Prefabs/Player prefab in the project tree.
     * @author Harry Hollands
     * @param parent the parent board the obstacle is created on
     * @param tilePosition the position that the player should be placed at
     * @return the obstacle that has been created
     */
    public static NetObstacle Create(NetBoard parent, NetTile tilePosition, uint obstacleID)
    {
        GameObject obstacleObject = Instantiate(Resources.Load("Prefabs/obstacle" + (obstacleID % 5).ToString())) as GameObject;
        NetObstacle obstacleScript = obstacleObject.AddComponent<NetObstacle>();
        obstacleScript.parent = parent;
        obstacleScript.CurrentTile = tilePosition;
        // Unity transform parent system does not deliver the desired result. Therefore manually assigning the position to be equal to the parent.
        obstacleObject.transform.position = tilePosition.gameObject.transform.position + Obstacle.POSITION_OFFSET;
        return obstacleScript;
    }

    // Use this for initialization
    void Start()
    {

    }

    /**
	 * This returns which tile the obstacle is currently on
	 * @author Harry Hollands
	 * @return the tile that the obstacle is on
	 */
    public NetTile GetOccupiedTile()
    {
        foreach (NetTile tile in this.parent.Tiles)
            if (tile.gameObject.transform.position == this.gameObject.transform.position - NetObstacle.POSITION_OFFSET)
                return tile;
        return null;
    }
}
