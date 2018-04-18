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

    [SyncVar]
    public string playerName = "";
    //public bool rolled = false;

    private NetBoard parent;
    private NetTile tile;

    public NetPlayer[] TeamPlayers;
    
    void Start()
    {
        parent = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetBoard>();
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
            
            parent.AssignPlayers();
            //parent = netBoard;
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

        TeamPlayers = new NetPlayer[5];
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
                    models[pawn].GetComponentInChildren<TextMesh>().text = playerName;
                    models[pawn].GetComponentInChildren<TextMesh>().color = playerColor;

                    TeamPlayers[i] = models[pawn].GetComponent<NetPlayer>();
                    //Debug.Log("TeamPlayer i = " + TeamPlayers[i]);
                }
            }
        }
        //Color for each pawn depends on what was chosen at the lobby
        Renderer[] rend = this.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rend) {
            r.material.color = playerColor;
            
        }

        //NetBoard netBoard1 = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetBoard>();
        //parent.AssignCamps(numberOfPlayers);
    }
    
    //Handles next turn. Commands can only be send by objects you have authority over... hence, function to call a function
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

    public NetTile GetOccupiedTile()
    {
        foreach (NetTile tile in this.parent.Tiles)
            if (tile.gameObject.transform.position == this.gameObject.transform.position + new Vector3(0, 0, Board.ExpectedTileSize(parent.gameObject, parent.GetWidthInTiles, parent.GetHeightInTiles).magnitude / 3))
                return tile;
        return null;
    }
    public NetBoard GetParent()
    {
        return this.parent;
    }
}
