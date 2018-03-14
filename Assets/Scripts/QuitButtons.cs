using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

  /**
   * The functionalities to leave the game
   * @Author Lawrence Howes-Yarlett
   */
public class QuitButtons : MonoBehaviour
{
    
    //Variables     
    public Canvas quitMenu;
    public Button start;
    public Button cont;
    public Button settings;
    public Button quit;
    public Canvas quitToMenuMenu;
    
    /**
     * Sets Quit to disabled when game starts
     * @author Lawrence Howes-Yarlett
     */
    public void Start()
    {
        if(quitMenu != null)
            quitMenu.enabled = false;
        if(quitToMenuMenu != null)
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
     * @author Lawrence Howes-Yarlett
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
     * YesPress() Allows you to quit the game
     * @author Lawrence Howes-Yarlett
     */
    public void YesPress()
    {
        Application.Quit();
    }

   /**
	* NoPress() Allows you to return to the main menu
	* @author Lawrnce Howes-Yarlett
	*/
    public void NoPress()
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
     * YesMenu() Allows you to quit the game to the main menu
     * @author Lawrence Howes-Yarlett
     */
    public void YesMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
	/**
	 * NoMenu() Allows you to return to the game
	 * @author Lawrence Howes-Yarlett
	 */
    public void NoMenu()
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
