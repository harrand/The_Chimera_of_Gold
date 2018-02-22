using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour

{
    private bool playerSel = false, tileSel = false; //Helps movement of players. playerSel set when first player is selected. tileSel true if current is a tile;   
    private GameObject[] menus;
    private Canvas Menu;

    private Transform current = null;   //Currently selected will be held here
    public GameObject Ethan;            //Test Doll
    //Speed multiplier for camera movement
    private float speed = 15.0f;
    //Default distance and Distance limits. So you can't zoom out forever...
    private float distance = 5.0f;
    private float minDist = 1.0f, maxDist = 35.0f;

    //Needed to Limit rotation. Limits clipping with terrain somewhat. 
    private float minY = 10.0f, maxY = 80.0f;
    //!(minY doesn't do anything. I don't know why. I've temporarily hardcoded the value in setCameraPosition and commented out the variable line.)!

    //Used for angles
    private float x =0, y=0;
    
    // Use this for initialization
	void Start ()
    {
        Ethan = GameObject.FindGameObjectWithTag("Player");             //The test dummy
        //angles to be used for the rotation
        Vector3 angles = transform.eulerAngles;
        y = angles.y;
        x = angles.x;


        menus = GameObject.FindGameObjectsWithTag("Menu");
        for(int i = 0; i < menus.Length; i++)
            menus[i].GetComponent<Canvas>().enabled = false;

        Menu = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>();
        Menu.enabled = false;
    }

    /**
     * getLastClicked
     * @author Aswin
     * Returns the object that was last clicked if it can be focused on. Otherwise returns null
     */
    private Transform getLastClicked()
    {
		// If no GameObjects with the tag exists, return null or this will throw during unit-tests where there is nothing tagged "GameBoard". - Harry
		if(GameObject.FindGameObjectWithTag("GameBoard") == null)
		{
			return null;
		}
        //Searchs for the board, and checks what was clicked last. Then puts it into current
        if (GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().CurrentSelected == 1)
        {
            current = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedTile.transform;
            tileSel = true;
            //Debug.Log("Tile");
        }
        else if (GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().CurrentSelected == 2)
        {
            current = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedPlayer.transform;
            playerSel = true;
            tileSel = false;
            //Debug.Log("player");
        }
        else if (GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().CurrentSelected == 3)
        {
            current = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedObstacle.transform;
            tileSel = false;
            //Debug.Log("obs");
        }
        else
        {
            current = null;
            tileSel = false;
            //Debug.Log("Other/back");
        }

        return current;
    }

    /**
     * ClampAngle
     * Clamps the angle at between the limits (20 is high enough to stop ground clip).
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
            
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3.0f);
        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 1.5f);
        
    }
    // Update is called once per frame
    void LateUpdate ()
    {
        current = getLastClicked();

        if (current != null && Input.GetKey(KeyCode.LeftShift) && playerSel && tileSel)
        {
            //offset from tile
            GameObject boardObject = GameObject.FindGameObjectWithTag("GameBoard");
            setCameraPosition(current);
            // after moving the player, remove all board highlights.
			boardObject.GetComponent<Board>().Event.OnPlayerMove(boardObject.GetComponent<InputController>().LastClickedPlayer, current.position);
            //Debug.Log(tileSel);
        }
        else if (current != null)
        { 
            setCameraPosition(current);
            //Debug.Log("Should rotate around selected");
        }
        else
        {
            setCameraPosition(Ethan.transform);
            //Debug.Log("Should Follow Ethan ");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu.enabled = !Menu.enabled;
            for (int i = 0; i < menus.Length; i++)
                menus[i].GetComponent<Canvas>().enabled = false;
        }

	}
}
