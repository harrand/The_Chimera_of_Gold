using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraButtons : MonoBehaviour {

    public Dropdown playerOne;
    public GameObject player1;
    public Dropdown playerTwo;
    public GameObject player2;
    public Dropdown playerThree;
    public GameObject player3;
    public Dropdown playerFour;
    public GameObject player4;
    public Dropdown playerFive;
    public GameObject player5;
    public Dropdown[] playerButtons = new Dropdown[5];
    public GameObject[] players = new GameObject[5];

    // Use this for initialization
    void Start () {
        playerButtons[0] = playerOne;
        playerButtons[1] = playerTwo;
        playerButtons[2] = playerThree;
        playerButtons[3] = playerFour;
        playerButtons[4] = playerFive;
        players[0] = player1;
        players[1] = player2;
        players[2] = player3;
        players[3] = player4;
        players[4] = player5;
        DisableSome();
    }

    // Update is called once per frame
    void Update () 
	{
		if (GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected == 2) {
			Player currentPlayer = GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().LastClickedPlayer;
			int indexOfPlayer = currentPlayer.GetPlayerIndex ();
			int indexOfCamp = currentPlayer.GetCamp ().GetCampIndex ();
			switch (indexOfCamp)
			{
			case 0:
				playerOne.value = indexOfPlayer;
				break;
			case 1:
				playerTwo.value = indexOfPlayer;
				break;
			case 2:
				playerThree.value = indexOfPlayer;
				break;
			case 3:
				playerFour.value = indexOfPlayer;
				break;
			case 4:
				playerFive.value = indexOfPlayer;
				break;
			}
		}
	}

    /**
    * Disable unnecessary buttons
    * @author Zibo Zhang and Yutian Xue
    */
    public void DisableSome()
    {
        //int playerNumbers = boardObject.GetComponents<Board>().
        for (int i = 0; i < 5 - PlayerData.numberOfPlayers; i++)
        {
            playerButtons[4 - i].enabled = false;
            players[4 - i].SetActive(false);
        }
    }

    /**
    * Set the camera to player-one's pawns
    * @author Zibo Zhang and Yutian Xue
    */
    public void SetCameraForPlayer1(){
		if (GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().Camps [0].TeamPlayers [playerOne.value] == null) {
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().LastClickedTile = GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().GetGoalTile ();
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 1;
		} else {
			GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedPlayer = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<Board>().Camps[0].TeamPlayers[playerOne.value];
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 2;
		}
	}

    /**
    * Set the camera to player-two's pawns
    * @author Zibo Zhang and Yutian Xue
    */
    public void SetCameraForPlayer2(){
		if (GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().Camps [1].TeamPlayers [playerTwo.value] == null) {
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().LastClickedTile = GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().GetGoalTile ();
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 1;
		} else {
			GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedPlayer = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<Board>().Camps[1].TeamPlayers[playerTwo.value];
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 2;
		}
	}

    /**
    * Set the camera to player-three's pawns
    * @author Zibo Zhang and Yutian Xue
    */
    public void SetCameraForPlayer3(){
		if (GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().Camps [2].TeamPlayers [playerThree.value] == null) {
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().LastClickedTile = GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().GetGoalTile ();
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 1;
		} else {
			GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedPlayer = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<Board>().Camps[2].TeamPlayers[playerThree.value];
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 2;
		}
	}

    /**
    * Set the camera to player-four's pawns
    * @author Zibo Zhang and Yutian Xue
    */
    public void SetCameraForPlayer4(){
		if (GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().Camps [3].TeamPlayers [playerFour.value] == null) {
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().LastClickedTile = GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().GetGoalTile ();
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 1;
		} else {
			GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedPlayer = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<Board>().Camps[3].TeamPlayers[playerFour.value];
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 2;
		}
	}

    /**
    * Set the camera to player-five's pawns
    * @author Zibo Zhang and Yutian Xue
    */
    public void SetCameraForPlayer5(){
		if (GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().Camps [4].TeamPlayers [playerFive.value] == null) {
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().LastClickedTile = GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().GetGoalTile ();
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 1;
		} else {
			GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedPlayer = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<Board>().Camps[4].TeamPlayers[playerFive.value];
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 2;
		}
	}
}
