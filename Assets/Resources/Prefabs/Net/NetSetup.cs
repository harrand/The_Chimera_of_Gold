using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetSetup : NetworkBehaviour {

    [SyncVar]
    public Color playerColor = Color.black;

    [SyncVar]
    public int playerPosition = 0;
    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            NetBehaviour[] cons = GetComponentsInChildren<NetBehaviour>();
            foreach(NetBehaviour n in cons)
                n.enabled = true;
        }
        //Color for each pawn depends on what was chosen at the lobby
        Renderer[] rend = this.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rend) {
            r.material.color = playerColor;
        }
    }
    
}
