using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class does not have its own unit-test because of the fact that all methods that Obstacle contains are already tested in Player due to identical code.
 * The purpose of this class, despite having no unique code, is to provide an explicit label to GameObjects which are Obstacles.
 */
public class Obstacle : MonoBehaviour 
{
	private Board parent;
    public Tile CurrentTile { get; private set; }

    /**
     * Pseudo-constructor which uses the Prefabs/Player prefab in the project tree.
     */
    public static Obstacle Create(Board parent, Tile tilePosition)
    {
        GameObject obstacleObject = Instantiate(Resources.Load("Prefabs/Obstacle")) as GameObject;
        Obstacle obstacleScript = obstacleObject.AddComponent<Obstacle>();
        obstacleScript.parent = parent;
        obstacleScript.CurrentTile = tilePosition;
        // Unity transform parent system does not deliver the desired result. Therefore manually assigning the position to be equal to the parent.
        obstacleObject.transform.position = tilePosition.gameObject.transform.position;
        return obstacleScript;
    }

    // Use this for initialization
    void Start () 
	{

	}
    public Tile GetOccupiedTile()
    {
		foreach(Tile tile in this.parent.Tiles)
			if(tile.gameObject.transform.position == this.gameObject.transform.position)
				return tile;
		return null;
    }
}
