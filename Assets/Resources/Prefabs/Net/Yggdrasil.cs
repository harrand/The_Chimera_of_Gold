using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yggdrasil : MonoBehaviour {

    private NetBoard parent;
    public int LocalTurn;
    public bool Rolled;
    public GlobalNet SkyNet;
    public NetGame NetGame; 
	// Use this for initialization
	void Start () {
        Rolled = false;
        SkyNet = null;
        while (SkyNet == null)
            SkyNet = GameObject.FindGameObjectWithTag("SkyNet").GetComponent<GlobalNet>();

        NetGame = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<NetGame>();
	}
    /**
     * BoardEvent Constructor - sets the inputed board to be the board currently running
     * @author Harry Hollands
     * @param board the board that is currently being played
     */
    public Yggdrasil(NetBoard board)
    {
        this.parent = board;
    }


    public void OnPlayerMove(NetPlayer player, Vector3 moveTarget)
    {
        if (player.isLocalPlayer)
        {
            Debug.Log("It is not this Pawn's turn!");
            return;
        }
        if (player == null)
            return;
        if (player.transform.position == moveTarget)
            return;
        if (player.HasControlledObstacle())
        {
            bool shouldMove = false;
            foreach (NetTile tile in this.parent.Tiles)
                if (tile.transform.position == moveTarget && !tile.BlockedByObstacle())
                    shouldMove = true;
            if (shouldMove)
                //this.OnObstacleMove(player.GetControlledObstacle(), player, moveTarget + Obstacle.POSITION_OFFSET);
            return;
        }
        uint diceRoll = this.parent.GetDice.NumberFaceUp();
        // stop if the last clicked player is trying to move to a tile that it should not be able to move to.
        bool validMove = false;
        foreach (NetTile allowedDestination in new NetPlayerControl(this.parent.gameObject.GetComponent<NetInputController>().LastClickedPlayer).PossibleMoves(diceRoll))
        {
            if (allowedDestination.transform.position == moveTarget)
                validMove = true;
        }

        if (!validMove)
            return;
        player.gameObject.transform.position = moveTarget + NetPlayer.POSITION_OFFSET;
        this.parent.obstacleControlFlag = true;
        //this.parent.RemoveTileHighlights();
        this.HandleTakeovers(player);
        // Disable the dice so the same roll cannot be used twice.
        this.parent.GetDice.gameObject.SetActive(false);
        if (moveTarget == this.parent.GetGoalTile().transform.position)
            this.OnPlayerGoalEvent(player);
        this.parent.gameObject.GetComponent<NetGame>().endTurn.enabled = true;
    }

    public void OnPlayerGoalEvent(NetPlayer player)
    {
        Debug.Log("A player has reached the goal!");
        /*
        if (player.GetCamp().GetNumberOfPlayers() <= 1)
            OnWinEvent(player.GetCamp());
        player.Kill();
        this.parent.GetComponent<InputController>().CurrentSelected = 0;
        */
    }


    public void HandleTakeovers(NetPlayer player)
    {
        Debug.Log("You have landed on another pawn. Move it to the origin");
        /*
        Camp friendlyCamp = player.GetCamp();
        foreach (Camp camp in this.parent.Camps)
        {
            if (camp == friendlyCamp)
                continue;
            foreach (Player enemy in camp.TeamPlayers)
                if (enemy != null && enemy.GetOccupiedTile() == player.GetOccupiedTile()) // Player landed on another player and enemy should be sent back to their camp.
                {
                    enemy.transform.position = camp.GetOccupiedTile().gameObject.transform.position + Player.POSITION_OFFSET;
                }
        }
        */
    }
    // Update is called once per frame
    void Update () {
        Debug.Log("Global turn = " + SkyNet.GlobalTurn);
		if(SkyNet.GlobalTurn == LocalTurn && !Rolled)
        {
            NetGame.roll.enabled = true;
            //Debug.Log("Your turn!, Roll should be active");
        }
        else
        {
            NetGame.roll.enabled = false;
            //Debug.Log("Already Rolled!, Roll should be deactivated");
        }
	}
}
