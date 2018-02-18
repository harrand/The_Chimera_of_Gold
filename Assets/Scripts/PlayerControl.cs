using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerControl 
{
    public Player GetPlayer { get; private set; }

    public PlayerControl(Player player)
    {
        this.GetPlayer = player;
    }

    /**
    * Given a rolled dice value, produces an array of all the Tiles that the game-logic would deem legal to move to.
    */
    public Tile[] PossibleMoves(uint diceRoll)
    {
        // uint diceRoll = this.GetPlayer.parent.parent.GetDice.NumberFaceUp()
        Debug.Log(diceRoll);
        Tile previousTile = this.GetPlayer.GetOccupiedTile();
        Board parentBoard = this.GetPlayer.GetCamp().GetParent();
        Obstacle[] obstacles = parentBoard.Obstacles;
        List<Tile> possibleMoves = new List<Tile>();
        foreach(Tile adjacentTile in previousTile.AdjacentTiles(diceRoll))
        {
            bool isOnCamp = false;
            foreach (Camp camp in parentBoard.Camps)
                if (camp.GetOccupiedTile() == adjacentTile)
                    isOnCamp = true;
            if (adjacentTile.DistanceFrom(previousTile) == diceRoll && !isOnCamp)
            {
                possibleMoves.Add(adjacentTile);
            }
        }
        return possibleMoves.ToArray();
    }

    /**
     * Paints every Tile from this.PossibleMoves(uint) the colour of highlightColour.
     * Use this to signify to the player which tiles can be moved to.
     */
    public void HighlightPossibleMoves(uint diceRoll, Color highlightColour)
    {
        foreach (Tile tile in this.PossibleMoves(diceRoll))
            tile.GetComponent<Renderer>().material.color = new Color(highlightColour.r, highlightColour.g, highlightColour.b, highlightColour.a);
    }
}
