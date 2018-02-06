using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public Tile tile;
    public Player player;
    public Obstacle obstacle;
    public Transform current = null;

    public Transform startPos;
    public Transform endPos;
    public Transform defaultPos;

    public float speed = 1.0f;
    public float startT;
    public float distance;
    // Use this for initialization
	void Start ()
    {
        defaultPos = Camera.main.transform;
        Debug.Log("Pos: "+ defaultPos.position);

        startT = Time.time;
        distance = Vector3.Distance(startPos.position, endPos.position);

    }

    private Transform getLastClickedPosition()
    {
        if (GetComponent<InputController>().CurrentSelected == 1)
        {
            current = this.GetComponent<InputController>().LastClickedTile.transform;
            Debug.Log("Tile");
        }
        else if (GetComponent<InputController>().CurrentSelected == 2)
        {
            current = this.GetComponent<InputController>().LastClickedObstacle.transform;
            Debug.Log("obs");
        }
        else if (GetComponent<InputController>().CurrentSelected == 3)
        {
            current = this.GetComponent<InputController>().LastClickedPlayer.transform;
            Debug.Log("player");
        }
        else
        {
            current = null;
            Debug.Log("Other/back");
        }
        return current;
    }
	private void setCameraPosition(GameObject current)
    {
        transform.LookAt(current.transform);

    }
    // Update is called once per frame
    void Update ()
    {
        current = getLastClickedPosition();
        if (current != null)
        {
            //setCameraPosition(current);
            Debug.Log("Hello : " + current.position);
        }
        else
        {
            current = defaultPos;
            Debug.Log("Else " + current.position);
            //transform.TransformVector(0, 13, -9);
        }
	}
}
