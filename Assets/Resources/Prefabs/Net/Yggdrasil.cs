using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yggdrasil : MonoBehaviour {

    public int LocalTurn;
    public bool Rolled;
    public GlobalNet SkyNet;
    public NetGame NetGame; 
	// Use this for initialization
	void Start () {
        Rolled = false;
        SkyNet = null;
        while (SkyNet == null)
            SkyNet = GameObject.FindGameObjectWithTag("SkyNet").GetComponent<GlobalNet>();

        NetGame = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetGame>();
	}
    
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Global turn = " + SkyNet.GlobalTurn);
		if(SkyNet.GlobalTurn == LocalTurn && !Rolled)
        {
            NetGame.roll.enabled = true;
            Debug.Log("Your turn!, Roll should be active");
        }
        else
        {
            NetGame.roll.enabled = false;
            Debug.Log("Already Rolled!, Roll should be deactivated");
        }
	}
}
