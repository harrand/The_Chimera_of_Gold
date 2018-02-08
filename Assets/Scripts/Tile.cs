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
		tileObject.transform.parent = parent.gameObject.transform;
        Tile tileScript = tileObject.AddComponent<Tile>();
        tileScript.parent = parent;
        tileScript.PositionTileSpace = new Vector2(xTile, zTile);
        return tileScript;
    }

	void Start ()
    {
        if(this.parent != null)
            gameObject.transform.parent = this.parent.gameObject.transform;
	}
	
	void Update ()
    {

	}
}
