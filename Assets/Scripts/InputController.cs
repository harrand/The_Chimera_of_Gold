using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Board boardScript;
    /**
     * LastClickedX represents the object that was last clicked in 3D space. 
     */
	public Tile LastClickedTile{get; private set;}
	public Player LastClickedPlayer{get; private set;}
	public Obstacle LastClickedObstacle{get; private set;}
	//public Camp LastClickedCamp{get; private set;}

    /**
    * SelectedX represents the Player/Obstacle which should be able to move.
    */
    public Player SelectedPlayer{get; set;}
    public Obstacle SelectedObstacle{get; set;}

	void Start ()
    {
        this.boardScript = this.GetComponent<Board>();
		this.LastClickedTile = null;
		this.LastClickedPlayer = null;
		this.LastClickedObstacle = null;
        this.SelectedPlayer = null;
        this.SelectedObstacle = null;
	}
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
		{
			this.UpdateLastClickedObjects();
		}
	}

	/**
	* Update selected Player, Camp etc...
	*/
	private void UpdateLastClickedObjects()
	{
		Tile currentTile = this.GetMousedTile();
		if(currentTile != null)
			this.LastClickedTile = currentTile;
		Player currentPlayer = this.GetMousedPlayer();
		if(currentPlayer != null)
			this.LastClickedPlayer = currentPlayer;
		Obstacle currentObstacle = this.GetMousedObstacle();
		if(currentObstacle != null)
			this.LastClickedObstacle = currentObstacle;
	}

	private GameObject GetMousedGameObject()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit target = new RaycastHit();
		if (Physics.Raycast(ray, out target))
			return target.transform.gameObject;
		else
			return null;
	}

    private Tile GetMousedTile()
    {
		foreach(Tile tile in this.boardScript.Tiles)
			if(tile.gameObject == this.GetMousedGameObject())
				return tile;
		return null;
    }

	private Player GetMousedPlayer()
	{
		foreach(Camp camp in this.boardScript.Camps)
		{
			foreach(Player player in camp.TeamPlayers)
				if(player.gameObject == this.GetMousedGameObject())
					return player;
		}
		return null;
	}

	private Obstacle GetMousedObstacle()
	{
		foreach(Obstacle obstacle in this.boardScript.Obstacles)
			if(obstacle.gameObject == this.GetMousedGameObject())
				return obstacle;
		return null;
	}
}
