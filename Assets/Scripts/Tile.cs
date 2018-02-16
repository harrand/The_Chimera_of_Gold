using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	private Board parent;
    public Vector2 PositionTileSpace { get; private set; }

    /**
     * Pseudo-constructor which uses the Prefabs/Tile prefab in the project tree.
     */
    public static Tile Create(Board parent, float xTile, float zTile)
    {
        GameObject tileObject = Instantiate(Resources.Load("Prefabs/Tile")) as GameObject;
        Tile tileScript = tileObject.AddComponent<Tile>();
        if (parent != null)
        {
            tileObject.transform.parent = parent.gameObject.transform;
            tileScript.parent = parent;
        }
        tileScript.PositionTileSpace = new Vector2(xTile, zTile);
        return tileScript;
    }

	void Start()
    {
        if(this.parent != null)
            gameObject.transform.parent = this.parent.gameObject.transform;
	}

    /**
    * If any Player, Obstacle or Camp is on this Tile, returns true.
    */
    public bool HasOccupant()
    {
        return this.GetOccupant() != null;
    }

    public GameObject GetOccupant()
    {
        if (this.parent == null)
            return null;
        return null;
    }
}
