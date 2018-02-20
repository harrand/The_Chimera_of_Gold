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
    public Button start;
    public Button cont;
    public Button settings;
    public Button quit;
    public Canvas quitToMenuMenu;
    
    /**
     * Sets Quit to disabled when game starts
     */
    public void Start()
    {

        quitMenu.enabled = false;
        quitToMenuMenu.enabled = false;
        if (start != null)
        {
            start.enabled = true;
        }
        if (cont != null)
        {
            cont.enabled = true;
        }
        if (settings != null)
        {
            settings.enabled = true;
        }
        if (quit != null)
        {
            quit.enabled = true;
        }
    }


    /**
     * Enables menu when quit is pressed 
     */
    public void QuitPress()
    {
        quitMenu.enabled = true;
    }

    public void QuitToMenu()
    {
        quitToMenuMenu.enabled = true;
        if (start != null)
        {
            start.enabled = false;
        }
        if (cont != null)
        {
            cont.enabled = false;
        }
        if (settings != null)
        {
            settings.enabled = false;
        }
        if (quit != null)
        {
            quit.enabled = false;
        }
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
        if (start != null)
        {
            start.enabled = true;
        }
        if (cont != null)
        {
            cont.enabled = true;
        }
        if (settings != null)
        {
            settings.enabled = true;
        }
        if (quit != null)
        {
            quit.enabled = true;
        }
    }

    /**
     * YayMenu() Allows you to quit the game to the main menu
     * NayMenu() Allows you to return to the game
     */
    public void YayMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void NayMenu()
    {
        quitToMenuMenu.enabled = false;
        if (start != null)
        {
            start.enabled = true;
        }
        if (cont != null)
        {
            cont.enabled = true;
        }
        if (settings != null)
        {
            settings.enabled = true;
        }
        if (quit != null)
        {
            quit.enabled = true;
        }
    }
}
