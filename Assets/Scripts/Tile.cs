using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	private Board parent;
    public Vector2 PositionTileSpace { get; private set; }

    public static KeyValuePair<Tile, GameObject> Create(Board parent, float xTile, float zTile)
    {
        GameObject tileObject = Instantiate(Resources.Load("Prefabs/Tile")) as GameObject;
        Tile tileScript = tileObject.AddComponent<Tile>();
        tileScript.parent = parent;
        tileScript.PositionTileSpace = new Vector2(xTile, zTile);
        Debug.Log("Tile = [" + xTile + ", " + zTile + "]");
        return new KeyValuePair<Tile, GameObject>(tileScript, tileObject);
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
