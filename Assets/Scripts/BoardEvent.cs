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
		if(player.transform.position == moveTarget)
			return;
        if(player.HasControlledObstacle())
        {
            this.OnObstacleMove(player.GetControlledObstacle(), player, moveTarget);
            return;
        }
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
        player.GetCamp().GetParent().obstacleControlFlag = true;
		this.parent.RemoveTileHighlights();
		this.HandleTakeovers(player);
		// Disable the dice so the same roll cannot be used twice.
		this.parent.GetDice.gameObject.SetActive(false);
	}

    public void OnObstacleMove(Obstacle obstacle, Player controller, Vector3 moveTarget)
    {
        obstacle.gameObject.transform.position = moveTarget;
        this.parent.RemoveTileHighlights();
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
					enemy.transform.position = camp.GetOccupiedTile().gameObject.transform.position + new Vector3(0, 3, 0);
					Debug.Log("A player has been sent back to their camp!");
				}
		}
	}

	/**
	 * Same as BoardEvent::OnPlayerMove(Player, Vector3)
	 * See above.
	 */
	public void OnPlayerMove(Player player, Tile desiredTile)
	{
		this.OnPlayerMove(player, desiredTile.gameObject.transform.position);
	}
}
