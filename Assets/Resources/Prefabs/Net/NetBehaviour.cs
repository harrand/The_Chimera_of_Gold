using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetBehaviour : NetworkBehaviour{

    public float speed = 30.0F;
    private Rigidbody rb;
    
	// Use this for initialization
	void Start () {
        int Position = this.GetComponentInParent<NetSetup>().playerPosition;

        rb = this.GetComponent<Rigidbody>();
        //this.transform.position = new Vector3(15 * (Position-1) ,10,5);
        
	}

	// Update is called once per frame
	void Update () {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }
}
