using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GlobalNet : NetworkBehaviour {

    [SyncVar]
    public int GlobalTurn = 1;

    public int NumberOfPlayers;

}
