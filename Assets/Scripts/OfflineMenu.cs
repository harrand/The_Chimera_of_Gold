using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OfflineMenu : MonoBehaviour
{
    public Dropdown numberOfPlayers;
    public GameObject player2AI;
    public GameObject player2AIDifficulty;
    public Text player3Text;
    public GameObject player3AI;
    public GameObject player3AIDifficulty;
    public Text player4Text;
    public GameObject player4AI;
    public GameObject player4AIDifficulty;
    public Text player5Text;
    public GameObject player5AI;
    public GameObject player5AIDifficulty;

    // Disable player 3, 4 and 5 elements at the start
    void Start ()
    {
        player2AIDifficulty.SetActive(player2AI.GetComponent<Toggle>().isOn);
        DisableThree();
        DisableFour();
        DisableFive();
    }

    /**
     * Method to check whether any AI checkbox is ticked, and showing or hiding the toggle depending on the result
     * @author Lawrence Howes-Yarlett
     */
    private void Update()
    {
        player2AIDifficulty.SetActive(player2AI.GetComponent<Toggle>().isOn);
        player3AIDifficulty.SetActive(player3AI.GetComponent<Toggle>().isOn && player3AI.activeInHierarchy);
        player4AIDifficulty.SetActive(player4AI.GetComponent<Toggle>().isOn && player4AI.activeInHierarchy);
        player5AIDifficulty.SetActive(player5AI.GetComponent<Toggle>().isOn && player5AI.activeInHierarchy);
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
        player3AIDifficulty.SetActive(false);
    }

    /**
     * Method to disable elements of the GUI related to player 4
     * @author Lawrence Howes-Yarlett
     */
    public void DisableFour()
    {
        player4Text.enabled = false;
        player4AI.SetActive(false);
        player4AIDifficulty.SetActive(false);
    }

    /**
     * Method to disable elements of the GUI related to player 5
     * @author Lawrence Howes-Yarlett
     */
    public void DisableFive()
    {
        player5Text.enabled = false;
        player5AI.SetActive(false);
        player5AIDifficulty.SetActive(false);
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
        SceneManager.LoadScene("StartMenu");
    }

    /**
     * Method to start the game with the desired number of players and AI
     * @author Lawrence Howes-Yarlett & Harry Hollands
     */
    public void StartGame()
    {
        PlayerData.numberOfPlayers = 2;
        PlayerData.numberOfPlayers += player3AI.GetComponent<Toggle>().IsActive() ? 1u : 0u;
        PlayerData.numberOfPlayers += player4AI.GetComponent<Toggle>().IsActive() ? 1u : 0u;
        PlayerData.numberOfPlayers += player5AI.GetComponent<Toggle>().IsActive() ? 1u : 0u;
        PlayerData.isAIPlayer[0] = false;
        PlayerData.isAIPlayer[1] = player2AI.GetComponent<Toggle>().isOn;
        PlayerData.isAIPlayer[2] = player3AI.GetComponent<Toggle>().isOn;
        PlayerData.isAIPlayer[3] = player4AI.GetComponent<Toggle>().isOn;
        PlayerData.isAIPlayer[4] = player5AI.GetComponent<Toggle>().isOn;
        PlayerData.isHardAI[0] = false;
        PlayerData.isHardAI[1] = player2AIDifficulty.GetComponent<Toggle>().isOn;
        PlayerData.isHardAI[2] = player3AIDifficulty.GetComponent<Toggle>().isOn;
        PlayerData.isHardAI[3] = player4AIDifficulty.GetComponent<Toggle>().isOn;
        PlayerData.isHardAI[4] = player5AIDifficulty.GetComponent<Toggle>().isOn;
        SceneManager.LoadScene("Chimera");
    }
}
