using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	private Board parent;

    public static KeyValuePair<Tile, GameObject> Create(Board parent)
    {
        GameObject tileObject = Instantiate(Resources.Load("Prefabs/Tile")) as GameObject;
        Tile tileScript = tileObject.AddComponent<Tile>();
        tileScript.parent = parent;
        return new KeyValuePair<Tile, GameObject>(tileScript, tileObject);
    }

	void Start ()
    {
        gameObject.transform.parent = this.parent.gameObject.transform;
	}
	
	void Update ()
    {
		
	}
}
