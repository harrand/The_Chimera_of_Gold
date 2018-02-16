using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl 
{
    public Player GetPlayer { get; private set; }

    public PlayerControl(Player player)
    {
        this.GetPlayer = player;
    }

    public Tile[] PossibleMoves(uint diceRoll)
    {
        // uint diceRoll = this.GetPlayer.parent.parent.GetDice.NumberFaceUp()
        Tile previousTile = this.GetPlayer.GetOccupiedTile();
        Board parentBoard = this.GetPlayer.GetCamp().GetParent();
        Obstacle[] obstacles = parentBoard.Obstacles;
        List<Tile> possibleMoves = new List<Tile>();
        foreach(Tile adjacentTile in previousTile.AdjacentTiles())
        {
            bool available = true;
            Vector2 aPos = adjacentTile.PositionTileSpace;
            Vector2 pPos = previousTile.PositionTileSpace;
            foreach (Obstacle obstacle in obstacles)
                if (obstacle.GetOccupiedTile() == adjacentTile) // adjacent tile has an obstacle on it
                    available = false;
            if (!available)
                continue;
            possibleMoves.Add(adjacentTile);
        }
        return possibleMoves.ToArray();
    }

    public void HighlightPossibleMoves(uint diceRoll, Color highlightColour)
    {
        foreach (Tile tile in this.PossibleMoves(diceRoll))
            tile.GetComponent<Renderer>().material.color = highlightColour;
    }
}
