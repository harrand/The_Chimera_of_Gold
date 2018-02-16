using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardEvent
{
	private Board parent;

	public BoardEvent(Board board)
	{
		this.parent = board;
	}

	/**
	 * Invoked whenever a player attempts to move.
	 */
	public void OnPlayerMove(Player player, Vector3 moveTarget)
	{
		uint diceRoll = this.parent.GetDice.NumberFaceUp();
		// stop if the last clicked player is trying to move to a tile that it should not be able to move to.
		bool validMove = false;
		foreach (Tile allowedDestination in new PlayerControl(this.parent.gameObject.GetComponent<InputController>().LastClickedPlayer).PossibleMoves(diceRoll))
		{
			if (allowedDestination.transform.position == moveTarget)
				validMove = true;
		}
		if (!validMove)
		{
			// Would normally log this but it spams it due to the nature of Input.GetKeyDown being really spammy.
			//Debug.Log("You cannot move here!");
			return;
		}
		player.gameObject.transform.position = moveTarget + Player.POSITION_OFFSET;
		this.parent.RemoveTileHighlights();
		this.HandleTakeovers(player);
	}

	/**
	 * Called by OnPlayerMove. Ensures that if a player has landed on any others, it sets that player back to the camp spawn.
	 */
	private void HandleTakeovers(Player player)
	{
		Camp friendlyCamp = player.GetCamp();
		foreach(Camp camp in this.parent.Camps)
		{
			if(camp == friendlyCamp)
				continue;
			foreach(Player enemy in camp.TeamPlayers)
				if(enemy.GetOccupiedTile() == player.GetOccupiedTile())
				{
					Debug.Log("enemy camp is: " + camp.GetOccupiedTile());
					enemy.transform.position = camp.GetOccupiedTile().gameObject.transform.position + new Vector3(0, 3, 0);
					Debug.Log("You have sent an enemy player back their camp!");
				}
		}
	}

	public void OnPlayerMove(Player player, Tile desiredTile)
	{
		this.OnPlayerMove(player, desiredTile.gameObject.transform.position);
	}
}
