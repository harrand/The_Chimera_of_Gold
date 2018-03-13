using System.Collections;
using System.Collections.Generic;
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
        firstPlayer.text = ("Player " + PlayerData.winnerNumber);
        firstPlayer.color = PlayerData.winnerColour;
        //secondPlayer.text = Howvever 2nd place is stored
        //if (thirdPlayer.enabled)
        //{
            //thirdPlayer.text = Howvever 3rd place is stored
        //}
        //etc
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
