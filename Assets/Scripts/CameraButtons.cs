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
    //GameObject boardObject = GameObject.FindGameObjectWithTag("GameBoard");

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

    public void DisableSome()
    {
        //int playerNumbers = boardObject.GetComponents<Board>().
        for (int i = 0; i < 5 - PlayerData.numberOfPlayers; i++)
        {
            playerButtons[4 - i].enabled = false;
            players[4 - i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

	public void SetCameraForPlayer1(){
		if (GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().Camps [0].TeamPlayers [playerOne.value] == null) {
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().LastClickedTile = GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().GetGoalTile ();
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 2;
		} else {
			GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedPlayer = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<Board>().Camps[0].TeamPlayers[playerOne.value];
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 2;
		}
	}

	public void SetCameraForPlayer2(){
		if (GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().Camps [1].TeamPlayers [playerOne.value] == null) {
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().LastClickedTile = GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().GetGoalTile ();
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 2;
		} else {
			GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedPlayer = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<Board>().Camps[1].TeamPlayers[playerOne.value];
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 2;
		}
	}

	public void SetCameraForPlayer3(){
		if (GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().Camps [2].TeamPlayers [playerOne.value] == null) {
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().LastClickedTile = GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().GetGoalTile ();
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 2;
		} else {
			GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedPlayer = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<Board>().Camps[2].TeamPlayers[playerOne.value];
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 2;
		}
	}

	public void SetCameraForPlayer4(){
		if (GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().Camps [3].TeamPlayers [playerOne.value] == null) {
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().LastClickedTile = GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().GetGoalTile ();
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 2;
		} else {
			GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedPlayer = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<Board>().Camps[3].TeamPlayers[playerOne.value];
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 2;
		}
	}

	public void SetCameraForPlayer5(){
		if (GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().Camps [4].TeamPlayers [playerOne.value] == null) {
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().LastClickedTile = GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<Board> ().GetGoalTile ();
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 2;
		} else {
			GameObject.FindGameObjectWithTag("GameBoard").GetComponent<InputController>().LastClickedPlayer = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<Board>().Camps[4].TeamPlayers[playerOne.value];
			GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<InputController> ().CurrentSelected = 2;
		}
	}
}
