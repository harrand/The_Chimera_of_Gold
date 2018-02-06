using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Transform current = null;   //Currently selected will be held here
    public GameObject Ethan;            //Test Doll

    //Speed multiplier for camera movement
    private float speed = 15.0f;
    //Default distance and Distance limits. So you can't zoom out forever...
    private float distance = 5.0f;
    private float minDist = 1.0f, maxDist = 15.0f;

    //Needed to Limit rotation
    public float minY = -20f, maxY = 80f;

    //Used for angles
    float x=0, y=0;
    
    // Use this for initialization
	void Start ()
    {
        Ethan = GameObject.FindGameObjectWithTag("Player");             //The test dummy

        Vector3 angles = transform.eulerAngles;
        y = angles.y;
        x = angles.x;
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
     *@author Aswin
     * Clamps the angle at 360.
     */
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
    /**
     * setCameraPostion
     * @author Aswin
     * rotates the camera around the currently selected object. 
     */
    private void setCameraPosition(Transform currentTarget)
    {   
        //Only allows rotation if right mouse is held down. (It was difficult to click on other objects when the camera kept moving)
        if (Input.GetMouseButton(1))
        {
            //Debug.Log("DOWN");

            x += Input.GetAxis("Mouse X") * speed;
            y -= Input.GetAxis("Mouse Y") * speed;
        }
            y = ClampAngle(y, minY, maxY);              //y rotation limit

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            
            //Travel distance from object. Scroll in and out to move the camera back and forth. (Also clamped between limits)
            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, minDist,maxDist);

            //Keeps camera behind the object it follows. Useful for the test doll.
            Vector3 negativeDist = new Vector3(0f, 0f, -distance);
            Vector3 position = currentTarget.position + (rotation * negativeDist);
            
            transform.rotation = rotation;
            transform.position = position;
        
    }
    // Update is called once per frame
    void LateUpdate ()
    {
        current = getLastClicked();
       
        if (current != null)
        { 
            setCameraPosition(current);
            //Debug.Log("Should rotate around selected");
        }
        else
        {
            setCameraPosition(Ethan.transform);
            //Debug.Log("Should Follow Ethan ");
        }
        
	}
}
