using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform current = null;
    public GameObject Ethan;

    //Speed multiplier for camera movement
    public float speed = 5.0f;
    //Default distance and Distance limits. So you can't zoom out forever...
    public float distance = 10.0f;
    public float minDist = 5.0f, maxDist = 20.0f;

    //Needed to Limit rotation
    public float minY = -20, maxY = 80;

    //Used for angles
    float x, y;
    
    // Use this for initialization
	void Start ()
    {
        Ethan = GameObject.FindGameObjectWithTag("Player");             //The test dummy

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    /**
     * getLastClicked
     * @author Aswin
     * Returns the object that was last clicked if it can be focused on. Otherwise returns null
     */
    private Transform getLastClicked()
    {
        
        if (GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().CurrentSelected == 1)
        {
            current = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedTile.transform;
            //Debug.Log("Tile");
        }
        else if (GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().CurrentSelected == 2)
        {
            current = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedPlayer.transform;
            //Debug.Log("obs");
        }
        else if (GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().CurrentSelected == 3)
        {
            current = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedObstacle.transform;
            //Debug.Log("player");
        }
        else
        {
            current = null;
            //Debug.Log("Other/back");
        }
        
        return current;
        
        
    }

    /**
     * setCameraPostion
     * @author Aswin
     * rotates the camera around the currently selected object
     */
	private void setCameraPosition(Transform currentTarget)
    {
        if (Input.GetMouseButtonDown(1))
        {
            //  transform.LookAt(currentTarget);
            //transform.RotateAround(currentTarget.position, Vector3.up, Input.GetAxis("Mouse X") * speed);

        }
    }
    // Update is called once per frame
    void LateUpdate ()
    {
        current = getLastClicked();
        if (current != null)
        {

            setCameraPosition(current);
            Debug.Log("Hello : ");
        }
        else
        {
            setCameraPosition(Ethan.transform);
            Debug.Log("Else ");
        }
	}
}
