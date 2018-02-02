using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour 
{

	private Board parent;
	private Tile occupiedTile;


	// Use this for initialization
	void Start (Board board) 
	{
		parent = board;

	}
	
	// Update is called once per frame
	public Tile GetOccupiedTile()
	{
		return occupiedTile;
	}

	public void SetOccupiedTile(Tile newLocation)
	{
		occupiedTile = newLocation;
	}
}
