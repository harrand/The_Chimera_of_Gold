using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class NetBoard : Board {
    /// width and height use number of Tiles as units.
    private uint numberCamps, numberObstacles;
    private float width, height;
    public bool netObstacleControlFlag;
    new public Tile[] Tiles { get; private set; }
    new public Obstacle[] Obstacles { get; private set; }
    public NetPlayer[] NetPlayers { get; private set; }
    new public Dice GetDice { get; private set; }
    new public BoardEvent Event { get; private set; }

    /**
    * This is to be used to create a new Board when the root GameObject has a terrain component (interpolating instead of taking dimensions as parameters).
    * This is currently used for the network game board. However, it is not used for the Board unit test.
    * @author Harry Hollands/Aswin
    * @param root - GameObject which the Board should be attached to.
    * @param tilesWidth - Number of Tiles per row.
    * @param tilesHeight - Number of Tiles per column.
    * @return - Reference to the Board created.
    */
    public static NetBoard NetCreate(GameObject root, uint tilesWidth, uint tilesHeight)
    {
        if (tilesWidth < 5 && tilesHeight < 5 && (tilesWidth * tilesHeight) < 13)
        {
            Debug.LogError("Board has invalid width/height (tile-space). One of width or height must be at least 5 AND width * height MUST be greater than 13.");
        }
        NetBoard netBoard = root.AddComponent<NetBoard>();
        root.tag = "GameBoard";
        root.name += " (Board)";
        netBoard.GetDice = Dice.Create(netBoard.gameObject.transform.position, new Vector3(), new Vector3(1, 1, 1));
        Debug.Log(netBoard.GetDice);
        netBoard.Event = new BoardEvent(netBoard);
        netBoard.GetWidthInTiles = Convert.ToUInt32(tilesWidth);
        netBoard.GetHeightInTiles = Convert.ToUInt32(tilesHeight);

        // The following code block should probably belong in Board::Start() or Board::Awake...
        // The reason is cannot be in Board::Start() is because Board::Start() is ran once the script is initialised and ready to execute, by the time multiple other scripts would have needed references to these camps etc...
        // The reason Board::Awake() cannot have this code block is because that will execute directly after "root.AddComponent<Board>()" which means before board.GetWidthInTiles is assigned.
        // Thus, the initialisation code MUST happen right here, despite being ugly.

        /// Allocate and assign Tiles (before culling).
        netBoard.Tiles = new Tile[netBoard.GetWidthInTiles * netBoard.GetHeightInTiles];
        for (uint i = 0; i < netBoard.Tiles.Length; i++)
        {
            float xTile = i % netBoard.GetWidthInTiles;
            float zTile = i / netBoard.GetWidthInTiles;
            netBoard.Tiles[i] = Tile.Create(netBoard, xTile, zTile);
            GameObject tileObject = netBoard.Tiles[i].gameObject;
            Vector2 tileSize = Board.ExpectedTileSize(root, netBoard.GetWidthInTiles, netBoard.GetHeightInTiles);
            tileObject.transform.position = Game.MinWorldSpace(root) + (new Vector3(xTile * tileSize.x, 0, zTile * tileSize.y)) + new Vector3(6, 0, 6);
            tileObject.transform.position = new Vector3(tileObject.transform.position.x, Game.InterpolateYWorldSpace(root, tileObject.transform.position), tileObject.transform.position.z);
            tileObject.transform.localScale = new Vector3(tileSize.x, 1, tileSize.y);
            tileObject.name = "Tile " + (i + 1);
        }

        /// Hardcode these; design changes did not allow these to vary.
        //board.numberCamps = 5;
        netBoard.numberObstacles = 13;
        
        /*/// Allocate and assign Obstacles.
        netBoard.Obstacles = new Obstacle[netBoard.numberObstacles];
        for (uint i = 0; i < netBoard.numberObstacles; i++)
            netBoard.Obstacles[i] = Obstacle.Create(netBoard, netBoard.Tiles[i + 30], i);
        */
        netBoard.Cull();
        return netBoard;
    }

    /**Can't put this in Create because the tags aren't set until after that function runs which causes errors and this is the awful fix ...I'm so sorry*/
    public void AssignPlayers()
    {
        //Assign Clickable Players for each client. (They can click their own pawns)
        GameObject go = GameObject.FindGameObjectWithTag("LocalMultiplayer");
        GetComponent<NetBoard>().NetPlayers = go.GetComponentsInChildren<NetPlayer>();

        Debug.Log(GetComponent<NetBoard>().NetPlayers);
    }
    /**
     * This removes all of the Tiles that do not actually belong to the game board. 
     * This transforms a grid of Tiles to the actual Chimera of Gold game board.
     * @author Aswin Mathew
     */
    new public void Cull()
    {
        /// Packs the Board::Tiles array into the new gameTiles array.
        Tile[] gameTiles = new Tile[151];
        Tile goalTile = null;
        int j = 0;
        for (int i = 0; i < this.Tiles.Length; i++)
        {
            if (CheckTile(this.Tiles[i].PositionTileSpace.x, this.Tiles[i].PositionTileSpace.y))
            {
                gameTiles[j] = this.Tiles[i];
                j++;
            }
            if (this.Tiles[i].PositionTileSpace.y == 18 && this.Tiles[i].PositionTileSpace.x == 10)
            {
                goalTile = Tiles[i];
            }
        }
        foreach (Tile t in Tiles)
        {
            if (t != null)
                t.gameObject.SetActive(false);
        }
        foreach (Tile t in gameTiles)
        {
            if (t != null)
            {
                t.gameObject.SetActive(true);
            }
        }
        goalTile.gameObject.SetActive(true);
        goalTile.GetComponent<Renderer>().material.color = Tile.goalColour;

    }

     /**
     * Gets the Tile at a specific Vector2 position. Returns null if there is no such Tile.
     * @author Harry Hollands
     * @param positionTileSpace - The position of the desired Tile.
     * @return - Reference to the Tile at the position given.
     */
    new public Tile GetTileByTileSpace(Vector2 positionTileSpace)
    {
        foreach (Tile t in this.Tiles)
            if (t.PositionTileSpace == positionTileSpace)
                return t;
        return null;
    }
    new public uint GetWidthInTiles { get; private set; }
    new public uint GetHeightInTiles { get; private set; }
    new public float GetWidthInPixels { get { return this.GetWidthInTiles * Board.ExpectedTileSize(this.gameObject, this.GetWidthInTiles, this.GetHeightInTiles).x; } }
    new public float GetHeightInPixels { get { return this.GetHeightInTiles * Board.ExpectedTileSize(this.gameObject, this.GetWidthInTiles, this.GetHeightInTiles).y; } }
    new public Camp CampTurn { get; private set; }

}
