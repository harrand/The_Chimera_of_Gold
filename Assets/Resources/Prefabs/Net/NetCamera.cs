using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetCamera : CameraControl {
    Transform current = null;
    // Use this for initialization
    /**
     * GetLastClicked
     * @author Aswin Mathew
     * @return the object that was last clicked if it can be focused on. Otherwise returns null
     */
    private Transform GetLastClicked()
    {
        // If no GameObjects with the tag exists, return null or this will throw during unit-tests where there is nothing tagged "GameBoard". - Harry
        if (GameObject.FindGameObjectWithTag("GameBoard") == null)
        {
            return null;
        }
        //Searchs for the board, and checks what was clicked last. Then puts it into current
        if (GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetInputController>().CurrentSelected == 1)
        {
            if (GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetInputController>().LastClickedTile != null)
            {
                current = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetInputController>().LastClickedTile.transform;
                tileSel = true;
                //Debug.Log("Tile");
            }
        }
        else if (GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetInputController>().CurrentSelected == 2)
        {
            current = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetInputController>().LastClickedPlayer.transform;
            playerSel = true;
            tileSel = false;
            //Debug.Log("player");
        }
        else if (GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetInputController>().CurrentSelected == 3)
        {
            current = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetInputController>().LastClickedObstacle.transform;
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

    // Update is called once per frame
    void LateUpdate()
    {
        GameObject boardObject = GameObject.FindGameObjectWithTag("GameBoard");
        Debug.Log(boardObject);
        if (current == null)
        {
            NetPlayer[] nets = GameObject.FindGameObjectWithTag("LocalMultiplayer").GetComponentsInChildren<NetPlayer>();
            Debug.Log(nets);
            foreach (NetPlayer n in nets)
            {
                if(n.isActiveAndEnabled)
                {
                    current = n.gameObject.transform;
                    Debug.Log(current);
                    break;
                }
            }

        }
        current = GetLastClicked();
        Debug.Log("Current = " + current);
        if (current != null && Input.GetKey(KeyCode.LeftShift) && playerSel && tileSel)
        {
            //offset from tile

            SetCameraPosition(current);
            // after moving the player, remove all board highlights.
            boardObject.GetComponent<NetBoard>().Event.OnPlayerMove(boardObject.GetComponent<NetInputController>().LastClickedPlayer, current.position);
            //Debug.Log(tileSel);
        }
        else if (current != null)
        {
            SetCameraPosition(current);
            //Debug.Log("Should rotate around selected");
        }
        else
        {
            SetCameraPosition(boardObject.transform);
        }
    }
}
