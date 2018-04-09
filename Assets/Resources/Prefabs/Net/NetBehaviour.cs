using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetBehaviour : NetworkBehaviour{

    private Canvas menu;
    private int debugPos = -1;
    private Color dbc;
    NetSetup local = null;
    // Use this for initialization
    void Start () {
        int Position = this.GetComponentInParent<NetSetup>().playerPosition;
        NetBoard netBoard = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetBoard>();
        GlobalNet sky = GameObject.FindGameObjectWithTag("SkyNet").GetComponent<GlobalNet>();
        //rb = this.GetComponent<Rigidbody>();
        switch(Position)
        {
            case 1:
                transform.position = netBoard.Tiles[2].transform.position;
                break;
            case 2:
                transform.position = netBoard.Tiles[6].transform.position;
                break;
            case 3:
                transform.position = netBoard.Tiles[10].transform.position;
                break;
            case 4:
                transform.position = netBoard.Tiles[14].transform.position;
                break;
            case 5:
                transform.position = netBoard.Tiles[18].transform.position;
                break;
        }
        Debug.Log("Pos: "+ Position + "Turn: "+ sky.GlobalTurn );
        
        if(this.GetComponentInParent<NetSetup>().gameObject.tag.Equals("LocalMultiplayer"))
            local = this.GetComponentInParent<NetSetup>();


        if (local != null && local.playerPosition == sky.GlobalTurn)
            GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetGame>().roll.enabled = true;
        else
            GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetGame>().roll.enabled = false;

        menu = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>();
        menu.enabled = false;


        debugPos = Position;
        dbc = this.gameObject.GetComponent<Renderer>().material.color;
	}

	// Update is called once per frame
	void Update () {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //////Each NetworkPlayer clone prints out so it looks like The player2 (StandAlones) are printing to console. 
        //////This is such bullshit I don't even remember what the fuck I was trying to fix in the first place...
        //rb.AddForce(movement * speed);
    }
}
