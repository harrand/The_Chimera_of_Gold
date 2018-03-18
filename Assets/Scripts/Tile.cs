using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

/**
* Tile contains all the data and necessary functions which need be applied to a game tile.
* @author Harry Hollands, Ciara O'Brien, Aswin Mathew
*/
public class Tile : MonoBehaviour
{
	private Board parent;
    public Vector2 PositionTileSpace { get; private set; }
	public static Color defaultColour = ((Material)Instantiate(Resources.Load("Prefabs/Translucent"))).color;
	public static Color goalColour = ((Material)Instantiate(Resources.Load("Prefabs/Translucent 1"))).color;
    /**
     * Pseudo-constructor which uses the Prefabs/Tile prefab in the project tree.
     * @author Harry Hollands
     * @param parent - Reference to the Board which is the parent of this Tile.
     * @param xTile - Row of the Tile.
     * @param zTile - Column of the Tile. The reason that this parameter is labeled 'zTile' and not 'yTile' is because of the fact that the 2D board will be laid down in the 3D world.
     * @return - Reference to the created Tile. 
     */
    public static Tile Create(Board parent, float xTile, float zTile)
    {
        GameObject tileObject = Instantiate(Resources.Load("Prefabs/Tile")) as GameObject;
        Tile tileScript = tileObject.AddComponent<Tile>();
        // is assigned in this::Start()
        if (parent != null)
        {
            tileObject.transform.parent = parent.gameObject.transform;
            tileScript.parent = parent;
        }
        tileScript.PositionTileSpace = new Vector2(xTile, zTile);
        return tileScript;
    }

    /**
     * Sets the underlying GameObject's transform-parent to the Board's transform.
     * @author Harry Hollands
     */
    void Start()
    {
        if(this.parent != null)
            gameObject.transform.parent = this.parent.gameObject.transform;
	}

    /**
    * If any Player, Obstacle or Camp is on this Tile, returns true. Else false.
    * @author Harry Hollands
    * @return - True if the Tile has a Player or Obstacle or Camp on it, else false.
    */
    public bool HasOccupant()
    {
        return this.GetOccupant() != null;
    }

    /**
     * Returns the underlying GameObject of the Player or Obstacle on the tile. Does not include Camps.
     * @author Harry Hollands
     * @return - Reference to the underlying GameObject of the occupant of this Tile.
     */
    public GameObject GetOccupant()
    {
        foreach (Camp camp in this.parent.Camps)
        {
            if (camp.gameObject.transform.position == this.gameObject.transform.position)
                return camp.gameObject;
            foreach (Player player in camp.TeamPlayers)
                if (player != null && player.gameObject.transform.position - Player.POSITION_OFFSET == this.gameObject.transform.position)
                    return player.gameObject;
        }
		foreach(Obstacle obst in this.parent.Obstacles)
			if(obst.gameObject.transform.position == this.gameObject.transform.position)
				return obst.gameObject;
		return null;
    }

    /**
    * Returns true if this Tile has an occupant and the occupant is an Obstacle.
    * @author Harry Hollands
    * @return - True if an Obstacle is on this Tile, else false.
    */
	public bool BlockedByObstacle()
	{
		foreach(Obstacle obst in this.parent.Obstacles)
			if(obst.GetOccupiedTile() == this)
				return true;
		return false;
	}

    /**
    * Returns displacement between two Tiles in tile-space.
    * @author Harry Hollands
    * @param otherTile - The other Tile used for this calculation.
    * @return - Vector2 of the difference between this tile and the other tile in tile-space.
    */
    public Vector2 DisplacementFrom(Tile otherTile)
    {
        return this.PositionTileSpace - otherTile.PositionTileSpace;
    }

    /**
    * Returns geometric distance between two Tiles in tile-space.
    * Does not take diagonal displacement into account.
    * @author Harry Hollands
    * @param otherTile - The other TIle used for this calculation.
    * @return - Unsigned int representing the magnitude of the displacement between this Tile and the other Tile.
    */
    public uint DistanceFrom(Tile otherTile)
    {
        return Convert.ToUInt32(Mathf.Abs(this.DisplacementFrom(otherTile).x) + Mathf.Abs(this.DisplacementFrom(otherTile).y));
    }

    /**
     * Returns an array of all Tiles which are DIRECTLY adjacent to this one.
     * The range used is always 1. The only reason a range parameter is needed is because 
     * @author Harry Hollands
     * @param original - The Tile that will have its adjacent Tiles retrieved.
     * @param range - The range in which you would like to check for adjacent tiles (For example, a dice roll).
     * @param ignoreObstacles  
     * @return - Array of all adjacent Tiles.
     */
    private Tile[] AdjacentTiles(Tile original, uint range, bool ignoreObstacles)
	{
		Vector2 pos = this.PositionTileSpace;
		Board board = this.parent;
		List<Tile> tiles = new List<Tile>();
		Tile tile = null;
		if (pos.x > 0)
		{
			tile = board.GetTileByTileSpace(new Vector2(pos.x - 1, pos.y));
			if(tile.gameObject.activeSelf)
			{
	            if (ignoreObstacles || !tile.BlockedByObstacle())
	                tiles.Add(tile);
	            else
	            {
	                Obstacle blockedBy = null;
	                foreach (Obstacle obst in board.Obstacles)
	                    if (obst.GetOccupiedTile() == tile)
	                        blockedBy = obst;
	                if (blockedBy.GetOccupiedTile().DistanceFrom(original) == range)
	                    tiles.Add(tile);
	            }
			}
		}
		if (pos.x < (board.GetWidthInTiles - 1))
		{
			tile = board.GetTileByTileSpace(new Vector2(pos.x + 1, pos.y));
			if(tile.gameObject.activeSelf)
			{
	            if (ignoreObstacles || !tile.BlockedByObstacle())
	                tiles.Add(tile);
	            else
	            {
	                Obstacle blockedBy = null;
	                foreach (Obstacle obst in board.Obstacles)
	                    if (obst.GetOccupiedTile() == tile)
	                        blockedBy = obst;
	                if (blockedBy.GetOccupiedTile().DistanceFrom(original) == range)
	                    tiles.Add(tile);
	            }
			}
        }
		if (pos.y > 0)
		{
			tile = board.GetTileByTileSpace(new Vector2(pos.x, pos.y - 1));
			if(tile.gameObject.activeSelf)
			{
	            if (ignoreObstacles || !tile.BlockedByObstacle())
	                tiles.Add(tile);
	            else
	            {
	                Obstacle blockedBy = null;
	                foreach (Obstacle obst in board.Obstacles)
	                    if (obst.GetOccupiedTile() == tile)
	                        blockedBy = obst;
	                if (blockedBy.GetOccupiedTile().DistanceFrom(original) == range)
	                    tiles.Add(tile);
	            }
			}
        }
		if (pos.y < (board.GetHeightInTiles - 1))
		{
			tile = board.GetTileByTileSpace(new Vector2(pos.x, pos.y + 1));
			if(tile.gameObject.activeSelf)
			{
	            if (ignoreObstacles || !tile.BlockedByObstacle())
	                tiles.Add(tile);
	            else
	            {
	                Obstacle blockedBy = null;
	                foreach (Obstacle obst in board.Obstacles)
	                    if (obst.GetOccupiedTile() == tile)
	                        blockedBy = obst;
	                if (blockedBy.GetOccupiedTile().DistanceFrom(original) == range)
	                    tiles.Add(tile);
	            }
			}
        }
		return tiles.ToArray();
	}

    /**
	 * Returns an array of all Tiles which are adjacent to all Tiles which are also adjacent to this one (within the range).
	 * Does not take obstacles into account.
     * @author Harry Hollands
     * @param original - The Tile that will have its adjacent Tiles retrieved.
     * @param range - The range in which you would like to check for adjacent tiles (For example, a dice roll). 
	 * @return - Array of Tiles adjacent to the original within the specified range, including all Tiles which would normally be blocked by Obstacles etc.
     */
    public Tile[] AdjacentTilesNoObstacles(uint range)
	{
		if(range == 0)
			return new Tile[0];
		Tile[] total = this.AdjacentTiles(this, range, true);
		for(uint i = 1; i < range; i++)
			foreach(Tile tile in total)
				total = total.Union(tile.AdjacentTiles(this, range, true)).ToArray();
		return total.ToArray();
	}

    /**
	 * Returns an array of all Tiles which are adjacent to all Tiles which are also adjacent to this one (within the range).
	 * DOES take obstacles into account.
     * @author Harry Hollands, Ciara O'Brien
     * @param range - The range in which you would like to check for adjacent tiles (For example, a dice roll).
	 * @return - Array of Tiles adjacent to the original within the specified range.
     */
    public Tile[] AdjacentTiles(uint range)
	{
		if(range == 0)
			return new Tile[0];
		Tile[] total = this.AdjacentTiles(this, range, false);
		for(uint i = 1; i < range; i++)
			foreach(Tile tile in total)
			{
				if(tile != null)
				{
					total = total.Union(tile.AdjacentTiles(this, range, false)).ToArray();
				}
			}
		return total.ToArray();
	}
}
