using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class NetGame : Game {

    
    private NetBoard board;

    //[SyncVar]
    //public int NumberOfPlayers = 0;

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
        this.roll.enabled = false;
        /*
        if (ishH)
        {
            NumberOfPlayers = Network.connections.Length;
            //NetworkServer.Spawn(this.board.gameObject);
        }
        Debug.Log(NumberOfPlayers);
        */
    }
    public void NetDiceRoll()
    {
        Debug.Log("Rolling");
        NetPlayer last = this.board.gameObject.GetComponent<NetInputController>().LastClickedPlayer;

        NetSetup currentCamp = GameObject.FindGameObjectWithTag("LocalMultiplayer").GetComponent<NetSetup>();

        if (!currentCamp.rolled)
        {
            GameObject GO = GameObject.FindGameObjectWithTag("SkyNet");
            //GO.GetComponent<GlobalNet>().CmdNextTurn();
            Debug.Log("ROLL " + GO.GetComponent<GlobalNet>().GlobalTurn);

            currentCamp.rolled = true;
            if (last != null && last.parent.CampTurn== currentCamp)
            {
                this.board.GetDice.Roll(last.transform.position + new Vector3(0, 20, 0));
            }
            else
            {
                this.board.GetDice.Roll(Camera.main.transform.position + new Vector3(0, 20, 0));

            }
            roll.enabled = false;
            endTurn.enabled = true;///////TEMPORARY DO NOT LEAVE THIS IN
        }
    }
    public void NetBoardNextTurn()
    {
        if (board.GameTurn == GameObject.FindGameObjectWithTag("LocalMultiplayer").GetComponent<NetSetup>().playerPosition)
        {
            this.board.NetNextTurn();
            roll.enabled = true;
            endTurn.enabled = false;
        }
    }
}
