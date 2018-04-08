using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetSetup : NetworkBehaviour {

    [SyncVar]
    public Color playerColor = Color.black;

    [SyncVar]
    public int playerPosition = 0;

    private Camp parent;
    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            

            NetBehaviour[] cons = GetComponentsInChildren<NetBehaviour>();
            foreach(NetBehaviour n in cons)
                n.enabled = true;

        
        }

        BoxCollider[] models = this.GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < models.Length / 5; i++)
        {
            if (playerPosition - 1 != i)
            {
                for (int j = 0; j < 5; j++)
                {
                    int pawn = j + (i*5);
                    models[pawn].gameObject.SetActive(false);
                }
                Debug.Log("Activating " + i);
            }
        }

        //Color for each pawn depends on what was chosen at the lobby
        Renderer[] rend = this.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rend) {
            r.material.color = playerColor;
            
        }

        
    }
    
}
