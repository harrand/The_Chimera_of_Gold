using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/**
* Board Event handles any changes that take place on the board - for example if a player makes a move
* @author Harry Hollands, Ciara O'Brien
*/
public class BoardEvent
{
	private Board parent;
    private Vector3 Pfrom, Pto;
	/**
	 * BoardEvent Constructor - sets the inputed board to be the board currently running
	 * @author Harry Hollands
	 * @param board the board that is currently being played
	 */
	public BoardEvent(Board board)
	{
		this.parent = board;
	}

	/**
	 * Invoked whenever a player attempts to move. Handles the complete movement of the player, checking if valid before making it.
	 * @author Harry Hollands
	 * @param player holds the current player to move
	 * @param moveTarget holds the location that the player wants to move to
	 */
	public void OnPlayerMove(Player player, Vector3 moveTarget)
	{
        if(player.GetCamp().GetParent().CampTurn != player.GetCamp())
        {
            Debug.Log("It is not this Pawn's turn!");
            return;
        }
        if (player == null)
            return;
		if(player.transform.position == moveTarget)
			return;
        if(player.HasControlledObstacle())
        {
            bool shouldMove = false;
            foreach (Tile tile in this.parent.Tiles)
                if (tile.transform.position == moveTarget && !tile.BlockedByObstacle())
                    shouldMove = true;
            if(shouldMove)
                this.OnObstacleMove(player.GetControlledObstacle(), player, moveTarget + Obstacle.POSITION_OFFSET);
            return;
        }
		uint diceRoll = this.parent.GetDice.NumberFaceUp();
		// stop if the last clicked player is trying to move to a tile that it should not be able to move to.
		bool validMove = false;
        foreach (Tile allowedDestination in new PlayerControl(this.parent.gameObject.GetComponent<InputController>().LastClickedPlayer).PossibleMoves(diceRoll))
            if (allowedDestination.transform.position == moveTarget)
                validMove = true;
		if (!validMove)
			return;
        player.origin = moveTarget + Player.POSITION_OFFSET;
        //player.target = player.origin;
        //player.gameObject.transform.position = player.origin;// Vector3.Lerp(player.gameObject.transform.position, moveTarget + Player.POSITION_OFFSET, Time.deltaTime * 1.5f);

        player.GetCamp().GetParent().obstacleControlFlag = true;
		//this.parent.RemoveTileHighlights();
		this.HandleTakeovers(player);
		// Disable the dice so the same roll cannot be used twice.
		this.parent.GetDice.gameObject.SetActive(false);
		if (moveTarget == this.parent.GetGoalTile().transform.position)
			this.OnPlayerGoalEvent(player);
        this.parent.gameObject.GetComponent<Game>().endTurn.enabled = true;
        
	}

    /**
    * Invoked when the player attempts to move an obstacle.
    * @author Harry Hollands
    * @param obstacle holds the obstacle to move
    * @param controller holds the player that has control over the obstacle being moved
    * @param moveTarget holds the location that the player wants to move the obstacle to
    */
    public void OnObstacleMove(Obstacle obstacle, Player controller, Vector3 moveTarget)
    {
        Vector3 previousLocation = obstacle.gameObject.transform.position;
        obstacle.gameObject.transform.position = moveTarget;
        if (obstacle.GetOccupiedTile() == controller.GetCamp().GetParent().GetGoalTile() || obstacle.GetOccupiedTile().PositionTileSpace.y <= 2)
        {
            obstacle.gameObject.transform.position = previousLocation;
            return;
        }
        this.parent.RemoveTileHighlights();
        this.parent.GetDice.gameObject.SetActive(false);
    }

	/**
	 * When a player reaches to goal node - the player is killed so that no longer usable - no longer has a turn
	 * @author Harry Hollands, Ciara O'Brien
	 * @param player the player that has won the game
	 */
    public void OnPlayerGoalEvent(Player player)
    {
        Debug.Log("A player has reached the goal!");
        if(player.GetCamp().GetNumberOfPlayers() <= 1)
            OnWinEvent(player.GetCamp());
        player.Kill();
        this.parent.GetComponent<InputController>().CurrentSelected = 0;
    }

	/**
	 * This sets the teams colour to yellow to represent a win and sets all the other players' colours to grey
	 * @author Harry Hollands, Ciara O'Brien
	 * @param winner represents the winning camp
	 */
    public void OnWinEvent(Camp winner)
    {
        Debug.Log(winner + " has reached the Chimera of Gold and won the game! Congratulations!");
        PlayerData.winnerColour = winner.TeamColor;
        int winnerIndex = Array.IndexOf(this.parent.Camps, winner);
        PlayerData.winnerNumber = winnerIndex + 1;
        Camp[] winners = new Camp[5];
        winners = this.parent.GetOrderedCamps();
        Debug.Log(winners[0]);
        Debug.Log("scores");
        foreach (Camp camp in this.parent.Camps)
            Debug.Log(camp.GetTotalScore());
        PlayerData.secondColour = winners[1].TeamColor;
        PlayerData.secondNumber = Array.IndexOf(this.parent.Camps, winners[1]) + 1;
        if (PlayerData.numberOfPlayers >= 3)
        {
            PlayerData.thirdColour = winners[2].TeamColor;
            PlayerData.thirdNumber = Array.IndexOf(this.parent.Camps, winners[2]) + 1;
        }
        if (PlayerData.numberOfPlayers >= 4)
        {
            PlayerData.fourthColour = winners[3].TeamColor;
            PlayerData.fourthNumber = Array.IndexOf(this.parent.Camps, winners[3]) + 1;
        }
        if (PlayerData.numberOfPlayers >= 5)
        {
            PlayerData.fifthColour = winners[4].TeamColor;
            PlayerData.fifthNumber = Array.IndexOf(this.parent.Camps, winners[4]) + 1;
        }
        winner.TeamColor = Color.yellow / 1.2f;
        /*
         * 1st place is trivial
         * second place will have the fewest pawns and reminaing pawns closest to the goal tile.
         * and so forth
         * 
         * heuristics needed:
         * distance from pawn to goal
         * pawns remaining (which we have)
        */
        //PlayerData.winners = this.parent.GetOrderedCamps();
        foreach (Camp playerLost in winner.GetParent().Camps)
            if (playerLost != winner)
            {
                playerLost.GetComponent<Renderer>().material.color = Color.gray;
            }
        SceneManager.LoadScene("Finit");
    }

    /**
	 * Called by OnPlayerMove. Ensures that if a player has landed on any others, it sets that player back to the camp spawn.
	 * @author Harry Hollands
	 * @param player The player that is taking over another player
	 */
    public void HandleTakeovers(Player player)
	{
		Camp friendlyCamp = player.GetCamp();
		foreach(Camp camp in this.parent.Camps)
		{
			if(camp == friendlyCamp)
				continue;
			foreach(Player enemy in camp.TeamPlayers)
				if(enemy != null && enemy.GetOccupiedTile() == player.GetOccupiedTile()) // Player landed on another player and enemy should be sent back to their camp.
				{
                    //Debug.Log("Touching");
                    //enemy.transform.position = camp.GetOccupiedTile().gameObject.transform.position + Player.POSITION_OFFSET;
                    //OnPlayerMove(enemy, camp.GetOccupiedTile().gameObject.transform.position + Player.POSITION_OFFSET);
                    enemy.origin = camp.GetOccupiedTile().gameObject.transform.position + Player.POSITION_OFFSET;
                }
		}
	}

	/**
	 * Same as BoardEvent::OnPlayerMove(Player, Vector3)
	 * See above.
	 * @author Harry Hollands
	 * @param player the current player that wants to move
	 * @param desiredTile the tile that the player is moving onto
	 */
	public void OnPlayerMove(Player player, Tile desiredTile)
	{
		this.OnPlayerMove(player, desiredTile.gameObject.transform.position);
	}
}
