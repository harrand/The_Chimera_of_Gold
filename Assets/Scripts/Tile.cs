using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
	private Board parent;
    public Vector2 PositionTileSpace { get; private set; }

    /**
     * Pseudo-constructor which uses the Prefabs/Tile prefab in the project tree.
     */
    public static Tile Create(Board parent, float xTile, float zTile)
    {
        GameObject tileObject = Instantiate(Resources.Load("Prefabs/Tile")) as GameObject;
        Tile tileScript = tileObject.AddComponent<Tile>();
        if (parent != null)
        {
            tileObject.transform.parent = parent.gameObject.transform;
            tileScript.parent = parent;
        }
        tileScript.PositionTileSpace = new Vector2(xTile, zTile);
        return tileScript;
    }

	void Start()
    {
        if(this.parent != null)
            gameObject.transform.parent = this.parent.gameObject.transform;
	}

    /**
    * If any Player, Obstacle or Camp is on this Tile, returns true.
    */
    public bool HasOccupant()
    {
        return this.GetOccupant() != null;
    }

    /**
     * Returns the gameobject of the player or obstacle on the tile. Does not return gameobjects for camps.
     */
    public GameObject GetOccupant()
    {
		foreach(Camp camp in this.parent.Camps)
			foreach(Player player in camp.TeamPlayers)
				if(player.gameObject.transform.position == this.gameObject.transform.position)
					return player.gameObject;
		foreach(Obstacle obst in this.parent.Obstacles)
			if(obst.gameObject.transform.position == this.gameObject.transform.position)
				return obst.gameObject;
		return null;
    }

	private bool BlockedByObstacle()
	{
		foreach(Obstacle obst in this.parent.Obstacles)
			if(obst.GetOccupiedTile() == this)
				return true;
		return false;
	}

    public Vector2 DisplacementFrom(Tile otherTile)
    {
        return this.PositionTileSpace - otherTile.PositionTileSpace;
    }

    public uint DistanceFrom(Tile otherTile)
    {
        return Convert.ToUInt32(Mathf.Abs(this.DisplacementFrom(otherTile).x) + Mathf.Abs(this.DisplacementFrom(otherTile).y));
    }

	/**
     * Returns an array of all Tiles which are directly adjacent to this one.
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
            if (ignoreObstacles || !tile.BlockedByObstacle())
            {
                tiles.Add(tile);
            }
            else
            {
                Obstacle blockedBy = null;
                foreach (Obstacle obst in board.Obstacles)
                    if (obst.GetOccupiedTile() == tile)
                        blockedBy = obst;
                Debug.Log("blocked by = " + blockedBy + " on tile " + blockedBy.GetOccupiedTile());
                Debug.Log("original = " + original);
                if (blockedBy.GetOccupiedTile().DistanceFrom(original) == range)
                {
                    Debug.Log("you can move an obstacle if you go ontop of it!");
                    tiles.Add(tile);
                }
            }
		}
		if (pos.x < (board.GetWidthInTiles - 1))
		{
			tile = board.GetTileByTileSpace(new Vector2(pos.x + 1, pos.y));
            if (ignoreObstacles || !tile.BlockedByObstacle())
            {
                tiles.Add(tile);
            }
            else
            {
                Obstacle blockedBy = null;
                foreach (Obstacle obst in board.Obstacles)
                    if (obst.GetOccupiedTile() == tile)
                        blockedBy = obst;
                Debug.Log("blocked by = " + blockedBy + " on tile " + blockedBy.GetOccupiedTile());
                Debug.Log("original = " + original);
                if (blockedBy.GetOccupiedTile().DistanceFrom(original) == range)
                {
                    Debug.Log("you can move an obstacle if you go ontop of it!");
                    tiles.Add(tile);
                }
            }
        }
		if (pos.y > 0)
		{
			tile = board.GetTileByTileSpace(new Vector2(pos.x, pos.y - 1));
            if (ignoreObstacles || !tile.BlockedByObstacle())
            {
                tiles.Add(tile);
            }
            else
            {
                Obstacle blockedBy = null;
                foreach (Obstacle obst in board.Obstacles)
                    if (obst.GetOccupiedTile() == tile)
                        blockedBy = obst;
                Debug.Log("blocked by = " + blockedBy + " on tile " + blockedBy.GetOccupiedTile());
                Debug.Log("original = " + original);
                if (blockedBy.GetOccupiedTile().DistanceFrom(original) == range)
                {
                    Debug.Log("you can move an obstacle if you go ontop of it!");
                    tiles.Add(tile);
                }
            }
        }
		if (pos.y < (board.GetHeightInTiles - 1))
		{
			tile = board.GetTileByTileSpace(new Vector2(pos.x, pos.y + 1));
            if (ignoreObstacles || !tile.BlockedByObstacle())
            {
                tiles.Add(tile);
            }
            else
            {
                Obstacle blockedBy = null;
                foreach (Obstacle obst in board.Obstacles)
                    if (obst.GetOccupiedTile() == tile)
                        blockedBy = obst;
                Debug.Log("blocked by = " + blockedBy + " on tile " + blockedBy.GetOccupiedTile());
                Debug.Log("original = " + original);
                if (blockedBy.GetOccupiedTile().DistanceFrom(original) == range)
                {
                    Debug.Log("you can move an obstacle if you go ontop of it!");
                    tiles.Add(tile);
                }
            }
        }
		return tiles.ToArray();
	}

	/**
	 * Returns an array of all Tiles which are adjacent to all Tiles which are also adjacent to this one (within the range).
	 * Does not take obstacles into account.
	 */
	public Tile[] AdjacentTilesSimple(Tile original, uint range)
	{
		if(range == 0)
			return new Tile[0];
		Tile[] total = this.AdjacentTiles(original, range, true);
		for(uint i = 1; i < range; i++)
			foreach(Tile tile in total)
				total = total.Union(tile.AdjacentTiles(original, range, true)).ToArray();
		return total.ToArray();
	}

	/**
	 * Returns an array of all Tiles which are adjacent to all Tiles which are also adjacent to this one (within the range).
	 * DOES take obstacles into account.
	 */
	public Tile[] AdjacentTiles(uint range)
	{
		if(range == 0)
			return new Tile[0];
		Tile[] total = this.AdjacentTiles(this, range, false);
		for(uint i = 1; i < range; i++)
			foreach(Tile tile in total)
			{
				total = total.Union(tile.AdjacentTiles(this, range, false)).ToArray();
			}
		return total.ToArray();
	}
}
