using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButtons : MonoBehaviour {

    /**
     * @Author Lawrence
     */
    //Variables     
    public Canvas quitMenu;
    public Button quit;
    /**
     * Sets Quit to disabled when game starts
     */
    public void Start()
    {
       quitMenu.enabled = false;
    }


    /**
     * Enables menu when quit is pressed 
     */
    public void quitPress()
    {
        quitMenu.enabled = true;
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
}
