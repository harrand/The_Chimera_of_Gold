using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();

        NetSetup[] localPlayer = gamePlayer.GetComponentsInChildren<NetSetup>();
        int numberOfPlayers = manager.GetComponent<LobbyManager>()._playerNumber;

        foreach (NetSetup player in localPlayer)
        {
            player.playerColor = lobby.playerColor;
            player.playerPosition = lobby.playerPosition;
            player.numberOfPlayers = numberOfPlayers;
            player.playerName = lobby.playerName;
        }
        
    }
}
