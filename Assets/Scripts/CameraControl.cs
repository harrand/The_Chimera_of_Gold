using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* The controls for the camera - after input has been processed this moves the camera accordingly
* @author Aswin Mathew
*/
public class CameraControl : MonoBehaviour
{
    public bool playerSel = false, tileSel = false; //Helps movement of players. playerSel set when first player is selected. tileSel true if current is a tile;   
    private GameObject[] menus;
    private Canvas Menu;

    private Transform current = null;   //Currently selected will be held here
    public GameObject Ethan;            //Test Doll
    //Speed multiplier for camera movement
    private float speed = 15.0f;
    //Default distance and Distance limits. So you can't zoom out forever...
    private float distance = 100.0f;
    private float minDist = 10.0f, maxDist = 150.0f;

    //Needed to Limit rotation. Limits clipping with terrain somewhat. 
    private float minY = 10.0f, maxY = 100.0f;

    //Used for angles
    private float x = 0, y = 0;
    
    // Use this for initialization
	void Start()
    {
        Ethan = GameObject.FindGameObjectWithTag("Player");//The test dummy
        //angles to be used for the rotation
        Vector3 angles = transform.eulerAngles;
        //y = angles.y;
        //x = angles.x;
        y = 10;
        x = 0;

        menus = GameObject.FindGameObjectsWithTag("Menu");
        for(uint i = 0; i < menus.Length; i++)
            menus[i].GetComponent<Canvas>().enabled = false;

        Menu = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>();
        Menu.enabled = false;
    }

    /**
     * GetLastClicked
     * @author Aswin Mathew
     * @return the object that was last clicked if it can be focused on. Otherwise returns null
     */
    private Transform GetLastClicked()
    {
		// If no GameObjects with the tag exists, return null or this will throw during unit-tests where there is nothing tagged "GameBoard". - Harry
		if(GameObject.FindGameObjectWithTag("GameBoard") == null)
		{
			return null;
		}
        //Searchs for the board, and checks what was clicked last. Then puts it into current
        if(GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().CurrentSelected == 1)
        {
            if (GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedTile != null)
            {
                current = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedTile.transform;
                tileSel = true;
                //Debug.Log("Tile");
            }
        }
        else if(GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().CurrentSelected == 2)
        {
            current = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedPlayer.transform;
            playerSel = true;
            tileSel = false;
            //Debug.Log("player");
        }
        else if(GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().CurrentSelected == 3)
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
     * Clamps the angle at between the limits (20 is high enough to stop ground clip).
     * @author Aswin Mathew
     * @param angle the limits of how far you can rotate the camera
     * @param min the minimum limit on how far you can rotate the camera
     * @param max the maximum limit on how far you can rotate the camera
     * @return the float representing the angle 
     */
    private float ClampAngle(float angle, float min, float max)
    {
        if(angle < -360f)
            angle += 360f;
        if(angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    /**
     * Rotates the camera around the currently selected object. 
     * @author Aswin Mathew
     * @param currentTarget the thing that is currently being looked at
     */
    public void SetCameraPosition(Transform currentTarget)
    {   
        //Only allows rotation if right mouse is held down. (It was difficult to click on other objects when the camera kept moving)
        if(Input.GetMouseButton(1))
        {
            //Debug.Log("DOWN");
            x += Input.GetAxis("Mouse X") * speed;
            y -= Input.GetAxis("Mouse Y") * speed;
        }
        y = ClampAngle(y, minY, maxY);              //y rotation limit
       
        Quaternion rotation = Quaternion.Euler(y, x, 0);
            
        //Travel distance from object. Scroll in and out to move the camera back and forth. (Also clamped between limits)
        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*50, minDist,maxDist);
                        
        //Keeps camera behind the object it follows. Useful for the test doll.
        Vector3 negativeDist = new Vector3(0f, 0f, -distance);
        Vector3 position = currentTarget.position + (rotation * negativeDist);

        if(position.x <= -90)
            position.x = -90;
        else if(position.x > 290)
            position.x = 290;

        if (position.z >= 290)
            position.z = 290;
        else if(position.z <= -90)
            position.z = -90;
            
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3.0f);
        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 1.5f);
        
    }

    /**
     * Invokes every tick
     * @author Aswin Mathew
     */
    void LateUpdate()
    {
        GameObject boardObject = GameObject.FindGameObjectWithTag("GameBoard");
        if (boardObject.GetComponent<InputController>().CurrentSelected == 0)
        {
            Player randomTeamPawn = null;
            int i = 0;
            while (randomTeamPawn == null)
            {
                Debug.Log("choosing first non null pawn");
                i++;
                randomTeamPawn = boardObject.GetComponent<Board>().CampTurn.TeamPlayers[i];
                if (i > 5)
                    break;
            }
            SetCameraPosition(randomTeamPawn.transform);
            boardObject.GetComponent<InputController>().LastClickedPlayer = randomTeamPawn;
            return;
        }

        current = GetLastClicked();

        if (current != null && Input.GetKey(KeyCode.LeftShift) && playerSel && tileSel)
        {
            //offset from tile

            SetCameraPosition(current);
            // after moving the player, remove all board highlights.
			boardObject.GetComponent<Board>().Event.OnPlayerMove(boardObject.GetComponent<InputController>().LastClickedPlayer, current.position);
            //Debug.Log(tileSel);
        }
        else if(current != null)
        { 
            SetCameraPosition(current);
            //Debug.Log("Should rotate around selected");
        }
        else
        {
            //Ethan no longer exists, should throw an error if this ever runs
            SetCameraPosition(Ethan.transform);
            //Debug.Log("Should Follow Ethan ");
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Menu.enabled = !Menu.enabled;
            for(int i = 0; i < menus.Length; i++)
                menus[i].GetComponent<Canvas>().enabled = false;
        }

	}
}
