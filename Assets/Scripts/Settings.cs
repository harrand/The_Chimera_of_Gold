using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    //variables
    public Canvas settingsMenu;
    public Button settings;
    public Dropdown resolution;
    private bool isFullScreen;

    /**
     * Disables the settings menu at the start
     */
    void Start()
    {
        settingsMenu.enabled = false;
        isFullScreen = Screen.fullScreen;
    }

    /**
     * Enables the setting menu when settings is pressed
     */
    public void SettingsPress()
    {
        settingsMenu.enabled = true;
    }

    /**
     * Disables the settings menu when the back button is pressed
     */
    public void BackPress()
    {
        settingsMenu.enabled = false;
    }

    /**
     * Toggles between full screen and windowed
     */
    public void FullScreenToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;
        isFullScreen = Screen.fullScreen;
    }

    /**
     * Changes the resolution of the game
     */
    public void ChangeResolution()
    {
        Debug.Log("Changing resolution");
        string option = resolution.options[resolution.value].text;
        Debug.Log(option);
        string[] tokens = option.Split("x".ToCharArray());
        Debug.Log("Printing tokens");
        Debug.Log(tokens[0]);
        Debug.Log(tokens[1]);
        int max = Int32.Parse(tokens[0]);
        int min = Int32.Parse(tokens[1]);
        Debug.Log(max);
        Debug.Log(min);
        Debug.Log(isFullScreen);
        Screen.SetResolution(max, min, isFullScreen);
    }
}