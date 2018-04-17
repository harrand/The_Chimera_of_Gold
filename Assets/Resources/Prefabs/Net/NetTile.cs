using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class NetTile : MonoBehaviour {
    private NetBoard parent;
    public Vector2 PositionTileSpace { get; private set; }
    public static Color defaultColour = ((Material)Instantiate(Resources.Load("Prefabs/Translucent"))).color;
    public static Color goalColour = ((Material)Instantiate(Resources.Load("Prefabs/Translucent 1"))).color;

    public static NetTile Create(NetBoard parent, float xTile, float zTile)
    {
        GameObject tileObject = Instantiate(Resources.Load("Prefabs/Tile")) as GameObject;
        NetTile tileScript = tileObject.AddComponent<NetTile>();
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
        if (this.parent != null)
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

        //Search for multiplayer clones and iterate through positions... Might help to set up a camp thing in netsetup when you activate the valid ones. 
        return null;
        /*
        foreach (Camp camp in this.parent.Camps)
        {
            if (camp.gameObject.transform.position == this.gameObject.transform.position)
                return camp.gameObject;
            foreach (Player player in camp.TeamPlayers)
                if (player != null && player.gameObject.transform.position - Player.POSITION_OFFSET == this.gameObject.transform.position)
                    return player.gameObject;
        }
        foreach (Obstacle obst in this.parent.Obstacles)
            if (obst.gameObject.transform.position == this.gameObject.transform.position)
                return obst.gameObject;
        return null;
        */
    }

    /**
    * Returns true if this Tile has an occupant and the occupant is an Obstacle.
    * @author Harry Hollands
    * @return - True if an Obstacle is on this Tile, else false.
    */
    public bool BlockedByObstacle()
    {
        if (parent.Obstacles == null)
            return false;

        foreach (NetObstacle obst in this.parent.Obstacles)
            if (obst.GetOccupiedTile() == this)
                return true;
        return false;
    }

    /**
    * Returns displacement between two Tiles in tile-space.
    * @author Harry Hollands
    * @param otherTile - The other Tile used for this calculation.
    * @return - Vector2 of the difference between this tile and the other tile in tile-space.
    */
    public Vector2 DisplacementFrom(NetTile otherTile)
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
    public uint DistanceFrom(NetTile otherTile)
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
    private NetTile[] AdjacentTiles(NetTile original, uint range, bool ignoreObstacles)
    {
        Vector2 pos = this.PositionTileSpace;
        NetBoard board = this.parent;
        List<NetTile> tiles = new List<NetTile>();
        NetTile tile = null;
        if (pos.x > 0)
        {
            tile = board.GetNetTileByTileSpace(new Vector2(pos.x - 1, pos.y));
            if (tile.gameObject.activeSelf)
            {
                if (ignoreObstacles || !tile.BlockedByObstacle())
                    tiles.Add(tile);
                else
                {
                    NetObstacle blockedBy = null;
                    foreach (NetObstacle obst in board.Obstacles)
                        if (obst.GetOccupiedTile() == tile)
                            blockedBy = obst;
                    if (blockedBy.GetOccupiedTile().DistanceFrom(original) == range)
                        tiles.Add(tile);
                }
            }
        }
        if (pos.x < (board.GetWidthInTiles - 1))
        {
            tile = board.GetNetTileByTileSpace(new Vector2(pos.x + 1, pos.y));
            if (tile.gameObject.activeSelf)
            {
                if (ignoreObstacles || !tile.BlockedByObstacle())
                    tiles.Add(tile);
                else
                {
                    NetObstacle blockedBy = null;
                    foreach (NetObstacle obst in board.Obstacles)
                        if (obst.GetOccupiedTile() == tile)
                            blockedBy = obst;
                    if (blockedBy.GetOccupiedTile().DistanceFrom(original) == range)
                        tiles.Add(tile);
                }
            }
        }
        if (pos.y > 0)
        {
            tile = board.GetNetTileByTileSpace(new Vector2(pos.x, pos.y - 1));
            if (tile.gameObject.activeSelf)
            {
                if (ignoreObstacles || !tile.BlockedByObstacle())
                    tiles.Add(tile);
                else
                {
                    NetObstacle blockedBy = null;
                    foreach (NetObstacle obst in board.Obstacles)
                        if (obst.GetOccupiedTile() == tile)
                            blockedBy = obst;
                    if (blockedBy.GetOccupiedTile().DistanceFrom(original) == range)
                        tiles.Add(tile);
                }
            }
        }
        if (pos.y < (board.GetHeightInTiles - 1))
        {
            tile = board.GetNetTileByTileSpace(new Vector2(pos.x, pos.y + 1));
            if (tile.gameObject.activeSelf)
            {
                if (ignoreObstacles || !tile.BlockedByObstacle())
                    tiles.Add(tile);
                else
                {
                    NetObstacle blockedBy = null;
                    foreach (NetObstacle obst in board.Obstacles)
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
    public NetTile[] AdjacentTilesNoObstacles(uint range)
    {
        if (range == 0)
            return new NetTile[0];
        NetTile[] total = this.AdjacentTiles(this, range, true);
        for (uint i = 1; i < range; i++)
            foreach (NetTile tile in total)
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
    public NetTile[] AdjacentTiles(uint range)
    {
        if (range == 0)
            return new NetTile[0];
        NetTile[] total = this.AdjacentTiles(this, range, false);
        for (uint i = 1; i < range; i++)
            foreach (NetTile tile in total)
            {
                if (tile != null)
                {
                    total = total.Union(tile.AdjacentTiles(this, range, false)).ToArray();
                }
            }
        return total.ToArray();
    }
}
