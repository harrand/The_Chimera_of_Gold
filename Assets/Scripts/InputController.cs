using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
	public const float MAX_CLICK_DISTANCE = 1000.0f;
    private Board boardScript;
	public Tile LastClickedTile{get; private set;}
	public Player LastClickedPlayer{get; private set;}
	public Obstacle LastClickedObstacle{get; private set;}
	//public Camp LastClickedCamp{get; private set;}

	void Start ()
    {
        this.boardScript = this.GetComponent<Board>();
		this.LastClickedTile = null;
		this.LastClickedPlayer = null;
		this.LastClickedObstacle = null;
	}
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
		{
			Tile currentTile = this.GetMousedTile();
			if(currentTile != null)
				this.LastClickedTile = currentTile;
			/* This code is ready for use.
			 * However, it should not be used until Player, Camp and Obstacle have been sufficiently unit tested.
			 * As of today (02/02/2018), Camp has a fault where TeamPlayers does not contain valid elements so would fail unit testing and break this.
			Player currentPlayer = this.GetMousedPlayer();
			if(currentPlayer != null)
				this.LastClickedPlayer = currentPlayer;
			Obstacle currentObstacle = this.GetMousedObstacle();
			if(currentObstacle != null)
				this.LastClickedObstacle = currentObstacle;
			*/
		}
	}

	private GameObject GetMousedGameObject()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit target = new RaycastHit();
		if (Physics.Raycast(ray, out target, InputController.MAX_CLICK_DISTANCE))
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
