using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BoardEvent : NetworkBehaviour
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
        if (player == null)
            return;
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
            if (allowedDestination.transform.position == moveTarget)
            {
                validMove = true;
                if (allowedDestination == this.parent.GetGoalTile())
                    this.OnPlayerGoalEvent(player);
            }
		if (!validMove)
			return;
		player.gameObject.transform.position = moveTarget + Player.POSITION_OFFSET;
        player.GetCamp().GetParent().obstacleControlFlag = true;
		//this.parent.RemoveTileHighlights();
		this.HandleTakeovers(player);
		// Disable the dice so the same roll cannot be used twice.
		this.parent.GetDice.gameObject.SetActive(false);
	}

    /**
    * Invoked when the player attempts to move an obstacle.
    */
    public void OnObstacleMove(Obstacle obstacle, Player controller, Vector3 moveTarget)
    {
        Vector3 previousLocation = obstacle.gameObject.transform.position;
        obstacle.gameObject.transform.position = moveTarget;
        if (obstacle.GetOccupiedTile() == controller.GetCamp().GetParent().GetGoalTile())
        {
            obstacle.gameObject.transform.position = previousLocation;
            return;
        }
        this.parent.RemoveTileHighlights();
        this.parent.GetDice.gameObject.SetActive(false);
    }

    public void OnPlayerGoalEvent(Player player)
    {
        Debug.Log("A player has reached the goal!");
        if(player.GetCamp().GetNumberOfPlayers() <= 1)
            OnWinEvent(player.GetCamp());
        player.Kill();
    }

    public void OnWinEvent(Camp winner)
    {
        Debug.Log(winner + " has reached the Chimera of Gold and won the game! Congratulations!");
        winner.TeamColor = Color.yellow / 1.2f;
        foreach (Camp mongrelLoser in winner.GetParent().Camps)
            if (mongrelLoser != winner)
            {
                mongrelLoser.GetComponent<Renderer>().material.color = Color.gray;
            }
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
				if(enemy.GetOccupiedTile() == player.GetOccupiedTile()) // Player landed on another player and enemy should be sent back to their camp.
				{
					enemy.transform.position = camp.GetOccupiedTile().gameObject.transform.position + Player.POSITION_OFFSET;
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
