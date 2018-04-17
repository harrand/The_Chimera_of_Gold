using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
/**
 * This controls the player - checking and showing movement available 
 * @author Harry Hollands
 */
public class NetPlayerControl
{
    public NetPlayer GetPlayer { get; private set; }

    public NetPlayerControl(NetPlayer player)
    {
        this.GetPlayer = player;
    }

    /**
    * Given a rolled dice value, produces an array of all the Tiles that the game-logic would deem legal to move to.
    * @author Harry Hollands, Ciara O'Brien
    * @param diceRoll the move that has been done, therefore checking any possible tiles that can be visited within that number
    * @return a list of the tiles that can possibly be moved to
    */
    public NetTile[] PossibleMoves(uint diceRoll)
    {
        NetTile previousTile = this.GetPlayer.GetOccupiedTile();
        Debug.Log("ITle = " + previousTile);
        Board parentBoard = this.GetPlayer.parent;
        Obstacle[] obstacles = parentBoard.Obstacles;
        List<NetTile> possibleMoves = new List<NetTile>();
        NetTile[] adjacents = previousTile.AdjacentTiles(diceRoll);
        NetTile[] adjacentsIgnoreRules = previousTile.AdjacentTilesNoObstacles(diceRoll);
        foreach (NetTile adjacentTile in adjacents)
        {
            bool isOnCamp = false;
            //foreach (Camp camp in parentBoard.Camps)
              //  if (camp.GetOccupiedTile() == adjacentTile)
                //    isOnCamp = true;
            bool allNeighboursAccountedFor = true;
            foreach (NetTile neighbour in adjacentTile.AdjacentTilesNoObstacles(1))
                if (Array.IndexOf(adjacentsIgnoreRules, neighbour) == -1)
                    allNeighboursAccountedFor = false;
            if (!allNeighboursAccountedFor && !isOnCamp)
                possibleMoves.Add(adjacentTile);
        }
        return possibleMoves.ToArray();
    }

    /**
     * Paints every Tile from this.PossibleMoves(uint) the colour of highlightColour.
     * Use this to signify to the player which tiles can be moved to.
     * @param diceRoll the move that has been done, therefore checking any possible tiles that can be visited within that number
     * @param highlightColour changes the colour of all of the available tiles to move to under the current move
     */
    public void HighlightPossibleMoves(uint diceRoll, Color moveHighlightColour, Color obstacleHighlightColour, Color takeoverHighlightColour)
    {
        this.GetPlayer.parent.RemoveTileHighlights();
        foreach (NetTile tile in this.PossibleMoves(diceRoll))
        {
            Debug.Log("Tile: " + tile);
            if (tile.BlockedByObstacle())
                tile.GetComponent<Renderer>().material.color = obstacleHighlightColour;
            else if (tile.GetOccupant() != null && tile.GetOccupant().GetComponent<Player>() != null)
                tile.GetComponent<Renderer>().material.color = takeoverHighlightColour;
            else
                tile.GetComponent<Renderer>().material.color = moveHighlightColour;
        }
    }
}
