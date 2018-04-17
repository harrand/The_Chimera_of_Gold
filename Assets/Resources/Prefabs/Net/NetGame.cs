using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class NetGame : Game {

    
    private NetBoard board;
    public Yggdrasil Yggdrasil;
    private GlobalNet SkyNet;
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
        Yggdrasil = null;
        SkyNet = null;

        // Create a normal Board with Input attached. Both Board and InputController are attached to the root GameObject (this).
        this.board = NetBoard.NetCreate(this.gameObject, tileWidth, tileHeight);
        this.board.gameObject.AddComponent<NetInputController>();
        this.endTurn.enabled = false;
        this.roll.enabled = false;

        //this.Yggdrasil = GameObject.FindGameObjectWithTag("WorldTree").GetComponent<Yggdrasil>();
        
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

        if (Yggdrasil == null)
            Yggdrasil = GameObject.FindGameObjectWithTag("WorldTree").GetComponent<Yggdrasil>();
        if (SkyNet == null)
            SkyNet = GameObject.FindGameObjectWithTag("SkyNet").GetComponent<GlobalNet>();

        if (!Yggdrasil.Rolled)
        {
            //GameObject GO = GameObject.FindGameObjectWithTag("SkyNet");
            //GO.GetComponent<GlobalNet>().CmdNextTurn()

            Yggdrasil.Rolled = true;
            if (last != null )
            {
                this.board.GetDice.Roll(last.transform.position + new Vector3(0, 20, 0));
            }
            else
            {
                this.board.GetDice.Roll(Camera.main.transform.position + new Vector3(0, 20, 0));

            }
            Yggdrasil.Rolled = true;
            endTurn.enabled = true;
        }
    }
    public void NetBoardNextTurn()
    {
        if (Yggdrasil.LocalTurn == SkyNet.GlobalTurn)//board.GameTurn == GameObject.FindGameObjectWithTag("LocalMultiplayer").GetComponent<NetSetup>().playerPosition)
        {
            this.board.NetNextTurn();
            //roll.enabled = true;
            //Yggdrasil.NetNextTurn();
            Yggdrasil.Rolled = false;
            endTurn.enabled = false;
        }
    }
}
