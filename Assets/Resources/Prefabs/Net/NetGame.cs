using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetGame : Game {

    
    private Board board;

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
}
