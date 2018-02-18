using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuitButtons : MonoBehaviour
{
    /**
     * @Author Lawrence
     */
    //Variables     
    public Canvas quitMenu;

    public Canvas quitToMenuMenu;

    public Button quit;
    /**
     * Sets Quit to disabled when game starts
     */
    public void Start()
    {

        quitMenu.enabled = false;
        quitToMenuMenu.enabled = false;
    }


    /**
     * Enables menu when quit is pressed 
     */
    public void QuitPress()
    {
        quitMenu.enabled = true;
    }

    public void quitToMenu()
    {
        quitToMenuMenu.enabled = true;
    }

    /**
     * YayPress() Allows you to quit the game
     * NayPress() Allows you to return to the main menu
     */
    public void YayPress()
    {
        Application.Quit();
    }

    public void NayPress()
    {
        quitMenu.enabled = false;
    }


    public void YayMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void NayMenu()
    {
        quitToMenuMenu.enabled = false;
    }
}
