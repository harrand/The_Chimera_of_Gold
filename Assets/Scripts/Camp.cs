using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour
{

	private Board parent;
	private uint numberPlayers;
	private Player[] teamPlayers;

    // Use this for initialization
    void Start()
    {
        
    }
    /*
	void Start (Board board, uint noPlayers) 
	{
		parent = board;
		numberPlayers = noPlayers;
		teamPlayers = new Player[numberPlayers];
	}
    */

    public Player SpawnPlayer()
	{
        return null;
	}

	public uint GetNumberPlayers()
	{
		return numberPlayers;
	}

}
