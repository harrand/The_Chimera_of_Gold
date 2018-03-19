using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinnerScreen : MonoBehaviour
{
    public Text title;
    public Text firstPlayer;
    public Text secondPlayer;
    public Text thirdPlayer;
    public Text fourthPlayer;
    public Text fifthPlayer;

    // Use this for initialization
    void Start ()
    {
        //Debug.Log(PlayerData.winners.Length);
        //Debug.Log(PlayerData.winners[0]);
        //Board board = PlayerData.winners[0].GetParent();
        if (PlayerData.numberOfPlayers == 2)
        {
            firstPlayer.enabled = true;
            secondPlayer.enabled = true;
            thirdPlayer.enabled = false;
            fourthPlayer.enabled = false;
            fifthPlayer.enabled = false;
        }
        else if (PlayerData.numberOfPlayers == 3)
        {
            firstPlayer.enabled = true;
            secondPlayer.enabled = true;
            thirdPlayer.enabled = true;
            fourthPlayer.enabled = false;
            fifthPlayer.enabled = false;
        }
        else if (PlayerData.numberOfPlayers == 4)
        {
            firstPlayer.enabled = true;
            secondPlayer.enabled = true;
            thirdPlayer.enabled = true;
            fourthPlayer.enabled = true;
            fifthPlayer.enabled = false;
        }
        else
        {
            firstPlayer.enabled = true;
            secondPlayer.enabled = true;
            thirdPlayer.enabled = true;
            fourthPlayer.enabled = true;
            fifthPlayer.enabled = true;
        }
        title.text = ("Congratulations, player " + PlayerData.winnerNumber + ", you found the Chimera of Gold!");
        firstPlayer.text = ("1. Player " + PlayerData.winnerNumber);
        firstPlayer.color = PlayerData.winnerColour;
        /*
        int secondPos = Array.IndexOf(board.Camps, PlayerData.winners[1]) + 1;
        secondPlayer.text = ("Player " + secondPos);
        secondPlayer.color = PlayerData.winners[1].TeamColor;
        if (thirdPlayer.enabled)
        {
            int thirdPos = Array.IndexOf(board.Camps, PlayerData.winners[2]) + 1;
            thirdPlayer.text = ("Player " + thirdPos);
            thirdPlayer.color = PlayerData.winners[2].TeamColor;
        }
        if (fourthPlayer.enabled)
        {
            int fourthPos = Array.IndexOf(board.Camps, PlayerData.winners[3]) + 1;
            fourthPlayer.text = ("Player " + fourthPos);
            fourthPlayer.color = PlayerData.winners[3].TeamColor;
        }
        if (fifthPlayer.enabled)
        {
            int fifthPos = Array.IndexOf(board.Camps, PlayerData.winners[4]) + 1;
            fifthPlayer.text = ("Player " + fifthPos);
            fifthPlayer.color = PlayerData.winners[4].TeamColor;
        }
        */
        secondPlayer.text = ("2. Player " + PlayerData.secondNumber);
        secondPlayer.color = PlayerData.secondColour;
        if (thirdPlayer.enabled)
        {
            thirdPlayer.text = ("3. Player " + PlayerData.thirdNumber);
            thirdPlayer.color = PlayerData.thirdColour;
        }
        if (fourthPlayer.enabled)
        {
            fourthPlayer.text = ("4. Player " + PlayerData.fourthNumber);
            fourthPlayer.color = PlayerData.fourthColour;
        }
        if (fifthPlayer.enabled)
        {
            fifthPlayer.text = ("5. Player " + PlayerData.fifthNumber);
            fifthPlayer.color = PlayerData.fifthColour;
        }
    }

    /**
     * Returns the user from the winner screen to the start menu
     * @author Lawrence Howes-Yarlett
     */
    public void BackToMenuPress ()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
