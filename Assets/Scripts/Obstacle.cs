using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        return obstacleScript;
    }

    // Use this for initialization
    void Start () 
	{

	}
}
