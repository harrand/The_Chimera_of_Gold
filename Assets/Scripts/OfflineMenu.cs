using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OfflineMenu : MonoBehaviour
{
    public Dropdown numberOfPlayers;
    public Text player3Text;
    public GameObject player3AI;
    public Text player4Text;
    public GameObject player4AI;
    public Text player5Text;
    public GameObject player5AI;

    // Disable player 3, 4 and 5 elements at the start
    void Start ()
    {
        DisableThree();
        DisableFour();
        DisableFive();
    }

    /**
     * Method to get the number of desired players from the drop down menu and update the GUI
     * @author Lawrence Howes-Yarlett
     */
    public void PlayerSelect()
    {
        string selectedNumber = numberOfPlayers.options[numberOfPlayers.value].text;
        int number = Int32.Parse(selectedNumber);
        if (number == 2)
        {
            DisableThree();
            DisableFour();
            DisableFive();
        }
        else if (number == 3)
        {
            ThreePlayers();
        }
        else if (number == 4)
        {
            FourPlayers();
        }
        else if (number == 5)
        {
            FivePlayers();
        }
    }

    /**
     * Method to disable elements of the GUI related to player 3
     * @author Lawrence Howes-Yarlett
     */
    public void DisableThree()
    {
        player3Text.enabled = false;
        player3AI.SetActive(false);
    }

    /**
     * Method to disable elements of the GUI related to player 4
     * @author Lawrence Howes-Yarlett
     */
    public void DisableFour()
    {
        player4Text.enabled = false;
        player4AI.SetActive(false);
    }

    /**
     * Method to disable elements of the GUI related to player 5
     * @author Lawrence Howes-Yarlett
     */
    public void DisableFive()
    {
        player5Text.enabled = false;
        player5AI.SetActive(false);
    }

    /**
     * Method to enable elements of the GUI for 3 players. Disables player 4 and 5 elements
     * @author Lawrence Howes-Yarlett
     */
    public void ThreePlayers()
    {
        player3AI.SetActive(true);
        player3Text.enabled = true;
        DisableFour();
        DisableFive();
    }


    /**
     * Method to enable elements of the GUI for 4 players. Disables player 5 elements
     * @author Lawrence Howes-Yarlett
     */
    public void FourPlayers()
    {
        ThreePlayers();
        player4AI.SetActive(true);
        player4Text.enabled = true;
        DisableFive();
    }

    /**
     * Method to enable elements of the GUI for 5 players.
     * @author Lawrence Howes-Yarlett
     */
    public void FivePlayers()
    {
        FourPlayers();
        player5AI.SetActive(true);
        player5Text.enabled = true;
    }


    /**
     * Method to go back to the mode select menu
     * @author Lawrence Howes-Yarlett
     */
    public void BackToModeSelect()
    {
        SceneManager.LoadScene(1);
    }

    /**
     * Method to start the game with the desired number of players and AI
     * @author Lawrence Howes-Yarlett
     */
    public void StartGame()
    {
        SceneManager.LoadScene(4);
        //add stuff so correct amount of players spawn and correct amount of them are AI
    }

    //Add AI toggle stuff so if player3AI.GetComponent<Toggle>().isOn = true player 3 will be AI and same for players 4 and 5
}
