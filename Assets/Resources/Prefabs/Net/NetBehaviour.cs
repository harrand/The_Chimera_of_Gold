using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetBehaviour : NetworkBehaviour{

    private Canvas menu;
	// Use this for initialization
	void Start () {
        int Position = this.GetComponentInParent<NetSetup>().playerPosition;
        NetBoard netBoard = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetBoard>();
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
        //this.transform.position = new Vector3(Position * 10 + 10,5,0);


        menu = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>();
        menu.enabled = false;
	}

	// Update is called once per frame
	void Update () {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //rb.AddForce(movement * speed);
    }
}
