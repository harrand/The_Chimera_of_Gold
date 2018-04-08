using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetBehaviour : NetworkBehaviour{

    private Canvas menu;
	// Use this for initialization
	void Start () {
        int Position = this.GetComponentInParent<NetSetup>().playerPosition;

        //rb = this.GetComponent<Rigidbody>();
        this.transform.position = new Vector3(Position * 10 + 10,5,0);


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
