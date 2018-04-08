using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class NetGame : Game {

    
    private NetBoard board;

    void Start()
    {
        /*
        new TestBoard(50, 50, 5, 5); // Perform Board Unit Test
        new TestTile();
        new TestCamp();
        new TestPlayer();
        */


        // Create a normal Board with Input attached. Both Board and InputController are attached to the root GameObject (this).
        this.board = NetBoard.NetCreate(this.gameObject, tileWidth, tileHeight);
        this.board.gameObject.AddComponent<NetInputController>();
        this.endTurn.enabled = false;
        /*if(isServer)
        {
            NetworkServer.Spawn(this.board.gameObject);
        }*/
    }
    public void NetDiceRoll()
    {
        Debug.Log("Rolling");
        NetPlayer last = this.board.gameObject.GetComponent<NetInputController>().LastClickedPlayer;

        Camp currentCamp = this.board.CampTurn;

        //if (!currentCamp.rolled)
        //{
        //    currentCamp.rolled = true;
        if (last != null )//&& last.GetCamp().GetParent().CampTurn == currentCamp)
        {
            this.board.GetDice.Roll(last.transform.position + new Vector3(0, 20, 0));
        }
        else
        {
            this.board.GetDice.Roll(Camera.main.transform.position + new Vector3(0, 20, 0));

        }
        //    roll.enabled = false;
        //}*/
    }
}
