using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	/**
     * Returns an array of all Tiles which are directly adjacent to this one.
     */
	private Tile[] AdjacentTiles()
	{
		Vector2 pos = this.PositionTileSpace;
		Board board = this.parent;
		List<Tile> tiles = new List<Tile>();
		if (pos.x > 0)
			tiles.Add(board.GetTileByTileSpace(new Vector2(pos.x - 1, pos.y)));
		if (pos.x < (board.GetWidthInTiles - 1))
			tiles.Add(board.GetTileByTileSpace(new Vector2(pos.x + 1, pos.y)));
		if (pos.y > 0)
			tiles.Add(board.GetTileByTileSpace(new Vector2(pos.x, pos.y - 1)));
		if (pos.y < (board.GetHeightInTiles - 1))
			tiles.Add(board.GetTileByTileSpace(new Vector2(pos.x, pos.y + 1)));
		return tiles.ToArray();
	}

	/**
	 * Returns an array of all Tiles which are adjacent to all Tiles which are also adjacent to this one (within the range).
	 */
	public Tile[] AdjacentTiles(uint range)
	{
		Tile[] total = this.AdjacentTiles();
		for(uint i = 1; i < range; i++)
			foreach(Tile tile in total)
				if(total.Contains(tile))
					total = total.Union(tile.AdjacentTiles()).ToArray();
		return total.ToArray();
	}
}
