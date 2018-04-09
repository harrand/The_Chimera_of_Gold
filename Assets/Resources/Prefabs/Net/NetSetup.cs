using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetSetup : NetworkBehaviour {

    [SyncVar]
    public Color playerColor = Color.black;

    [SyncVar]
    public int playerPosition = 0;

    [SyncVar]
    public int numberOfPlayers = 0;

    public bool rolled = false;

    void Start()
    {
        if (isLocalPlayer)
        {
            NetBehaviour[] cons = GetComponentsInChildren<NetBehaviour>();
            foreach (NetBehaviour n in cons)
            {
                n.enabled = true;
            }
            this.gameObject.tag = "LocalMultiplayer";
            Debug.Log("Setting tag");

            NetBoard netBoard = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetBoard>();
            netBoard.AssignPlayers();
            Debug.Log("Number of players: " + numberOfPlayers);
            if (netBoard.GameTurn != playerPosition)
                rolled = true;
        }
        Debug.Log(playerPosition);
        if(playerPosition == 1 && isServer)
        {
            Debug.Log("Becoming Self-Aware " + playerPosition);
            GameObject SkyNet = Instantiate(Resources.Load("Prefabs/SkyNet")) as GameObject;
            SkyNet.AddComponent<SkyRecon>();
            //ClientScene.RegisterPrefab(SkyNet);
            NetworkServer.Spawn(SkyNet);
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
                Debug.Log("De-Activating " + i);
            }
        }

        //Color for each pawn depends on what was chosen at the lobby
        Renderer[] rend = this.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rend) {
            r.material.color = playerColor;
            
        }

        
    }
    
}
