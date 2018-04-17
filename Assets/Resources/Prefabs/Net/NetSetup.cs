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

    //public bool rolled = false;

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

            //Disable non-player pawns here
            NetBoard netBoard = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetBoard>();
            netBoard.AssignPlayers();

            //Grow the world tree for each player
            GameObject WorldTree = Instantiate(Resources.Load("Prefabs/WorldTree")) as GameObject;
            WorldTree.transform.position = new Vector3(110,35,114);
            WorldTree.AddComponent<Yggdrasil>();
            WorldTree.GetComponent<Yggdrasil>().LocalTurn = playerPosition;
            Debug.Log("Tree grown");

            Debug.Log("Number of players: " + numberOfPlayers);
           //if (netBoard.GameTurn != playerPosition)
           //     rolled = true;
        }

        if(isServer && isLocalPlayer)
        {
            Debug.Log("Becoming Self-Aware " + playerPosition);
            GameObject SkyNet = Instantiate(Resources.Load("Prefabs/SkyNet")) as GameObject;
            SkyNet.AddComponent<SkyRecon>();
            SkyNet.GetComponent<GlobalNet>().NumberOfPlayers = numberOfPlayers;
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
                    int pawn = j + (i * 5);
                    models[pawn].gameObject.SetActive(false);
                    models[pawn].GetComponent<NetBehaviour>().enabled = false;
                    models[pawn].GetComponent<NetPlayer>().enabled = false;
                }
            }
            else
            {
                for(int j = 0; j < 5; j++)
                {
                    int pawn = j + (i * 5);
                    models[pawn].GetComponent<NetPlayer>().parent = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetBoard>();
                    models[pawn].gameObject.transform.position = new Vector3(0, 0, 0);
                }
            }
        }

        //Color for each pawn depends on what was chosen at the lobby
        Renderer[] rend = this.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rend) {
            r.material.color = playerColor;
            
        }

        
    }
    
    public void NextTurn()
    {
        Debug.Log("Next Turn...");
        
        CmdNextPlayerTurn();
    }
    [Command]
    public void CmdNextPlayerTurn()
    {
        GlobalNet sky = GameObject.FindGameObjectWithTag("SkyNet").GetComponent<GlobalNet>();
        sky.GlobalTurn++;
        if (sky.GlobalTurn > sky.NumberOfPlayers)
            sky.GlobalTurn = 1;
    }
}
