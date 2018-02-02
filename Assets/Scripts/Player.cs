using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{

	private Board parent;

	// Use this for initialization
	void Start (Board board) 
	{
		parent = board;
	}
	
	public Tile GetOccupiedTile()
	{
		
	}

}
