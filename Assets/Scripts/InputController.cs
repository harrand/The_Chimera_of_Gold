using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Board boardScript;
	// Use this for initialization
	void Start ()
    {
        this.boardScript = this.GetComponent<Board>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
            GetMousedTile();
	}

    Tile GetMousedTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit target = new RaycastHit();
        if (Physics.Raycast(ray, out target, 100))
        {
            foreach (Tile tile in this.boardScript.Tiles)
                if (tile.gameObject == target.transform.gameObject)
                {
                    Debug.Log("Moused over a Tile! It was called " + tile.gameObject.name);
                    return tile;
                }
            Debug.Log("Something is moused over, but it is not a Tile of the current Board.");
            return null;
        }
        else
        {
            Debug.LogError("Mouse is not over any collider.");
            return null;
        }
    }
}
