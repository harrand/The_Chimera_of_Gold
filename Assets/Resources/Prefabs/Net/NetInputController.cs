using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetInputController : NetworkBehaviour {
/**
 * @author Aswin Mathew, Harry Hollands
 * The Input controller works with users selections
 */
    private NetBoard boardScript;
    /**
     * LastClickedX represents the object that was last clicked in 3D space. 
     */
    public Tile LastClickedTile { get; set; }
    public NetPlayer LastClickedPlayer { get; set; }
    public Obstacle LastClickedObstacle { get; set; }
    //public Camp LastClickedCamp{get; private set;}

    /**
     * Decides what to focus the camera on. Basically last clicked object
     */
    public int CurrentSelected { get; set; }
    /**
    * SelectedX represents the Player/Obstacle which should be able to move.
    */
    public NetPlayer SelectedPlayer { get; set; }
    public Obstacle SelectedObstacle { get; set; }

    void Start()
    {
        this.boardScript = this.GetComponent<NetBoard>();

        Debug.Log(boardScript.Tiles);

        this.LastClickedTile = null;
        this.LastClickedPlayer = null;
        this.LastClickedObstacle = null;
        this.SelectedPlayer = null;
        this.SelectedObstacle = null;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.UpdateLastClickedObjects();
        }

        if (Input.GetKeyDown("c") && this.LastClickedPlayer != null)
        {
            //this.LastClickedPlayer.transform.position = this.boardScript.GetGoalTile().transform.position + Player.POSITION_OFFSET;
            //this.boardScript.Event.OnPlayerGoalEvent(this.LastClickedPlayer);
            //this.LastClickedPlayer = null;
            //this.CurrentSelected = 1;
        }

        if (Input.GetKeyDown("m") && this.LastClickedPlayer != null)
        {
            // highlight possible moves
            NetBoard board = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetBoard>();
            //board.RemoveTileHighlights();
            /*if (this.LastClickedPlayer.HasControlledObstacle())
            {
                foreach (Tile tile in this.boardScript.Tiles)
                {
                    if (!tile.HasOccupant() && tile.PositionTileSpace.y > 2 && !tile.BlockedByObstacle() && tile != this.boardScript.GetGoalTile())
                        tile.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
            }
            else
                new PlayerControl(this.LastClickedPlayer).HighlightPossibleMoves(board.GetDice.NumberFaceUp(), Color.green, Color.blue, Color.red);
        */
        }
    }

    /**
	* Update selected Player, Camp etc. Also updates the tags on the objects. Useful for controlling camera
    *  Remember to update getLastClicked() in cameraControl.cs if anything new is added here. e.g: Camps...
    * @author Aswin Mathew, Harry Hollands
	*/
    private void UpdateLastClickedObjects()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))
            return;
        Tile currentTile = this.GetMousedTile();
        if (currentTile != null)
        {
            if (this.LastClickedTile != null)
                this.LastClickedTile.tag = "Tiles";
            this.LastClickedTile = currentTile;
            currentTile.tag = "CurrentTile";
            this.CurrentSelected = 1;
        }
        NetPlayer currentPlayer = this.GetMousedPlayer();
        if (currentPlayer != null)
        {
            if (this.LastClickedPlayer != null)
                this.LastClickedPlayer.tag = "Players";
            this.LastClickedPlayer = currentPlayer;
            currentPlayer.tag = "CurrentPlayer";
            this.CurrentSelected = 2;
        }
        /*
        Obstacle currentObstacle = this.GetMousedObstacle();
        if (currentObstacle != null)
        {
            if (this.LastClickedObstacle != null)
                this.LastClickedObstacle.tag = "Obstacles";
            this.LastClickedObstacle = currentObstacle;
            currentObstacle.tag = "CurrentObstacle";
            this.CurrentSelected = 3;
        }*/
    }

    /**
     * Returns the gameobject hit by a ray-cast from the camera transform.
     * @author Harry Hollands, Aswin Mathew
     * @return the gameobject currently moused over
     */
    private GameObject GetMousedGameObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit target = new RaycastHit();
        if (Physics.Raycast(ray, out target))
            return target.transform.gameObject;
        else
            return null;
    }

    /**
     * Returns the tile that is GetMousedGameObject. If that is not a tile, returns null.
     * @author Harry Hollands, Aswin Mathew
     * @return the tile that is currently moused over
     */
    private Tile GetMousedTile()
    {

        foreach (Tile tile in this.boardScript.Tiles)
            if (tile.gameObject == this.GetMousedGameObject())
                return tile;
        return null;
    }

    /**
     * Returns the player that is GetMousedGameObject. If that is not a player, returns null.
     * @author Harry Hollands, Aswin Mathew
     * @return the player that is currently moused over
     */
    private NetPlayer GetMousedPlayer()
    {
        foreach (NetPlayer player in this.boardScript.NetPlayers)
        {
            if (player != null && player.gameObject == this.GetMousedGameObject())
                return player;
        }
        return null;
    }
    /**
     * Returns the obstacle that is GetMousedGameObject. If that is not a obstacle, returns null.
     * @author Harry Hollands, Aswin Mathew
     * @return the obstacle that is currently moused over
     */
    //private Obstacle GetMousedObstacle()
    //{
    //    foreach (Obstacle obstacle in this.boardScript.Obstacles)
    //        if (obstacle.gameObject == this.GetMousedGameObject())
    //            return obstacle;
    //    return null;
    //}

}
