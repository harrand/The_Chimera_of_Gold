using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 /**
  * Controls the settings of the game in game
  * @author Lawrence Howes-Yarlett
  */
public class Settings : MonoBehaviour
{
    //Variables assigned in the unity editor
    public Canvas settingsMenu;
    public Dropdown resolution;
    public Toggle fullScreen;
    public Slider volumeSlider;
    private bool isFullScreen;

    /**
     * Disables the settings menu at the start
     * @author Lawrence Howes-Yarlett
     */
    void Start()
    {
        settingsMenu.enabled = false;
        isFullScreen = Screen.fullScreen;
        fullScreen.isOn = isFullScreen;
    }

    /**
     * Enables the setting menu when settings is pressed
     * @author Lawrence Howes-Yarlett
     */
    public void SettingsPress()
    {
        settingsMenu.enabled = true;
    }

    /**
     * Disables the settings menu when the back button is pressed
     * @author Lawrence Howes-Yarlett
     */
    public void BackPress()
    {
        settingsMenu.enabled = false;
    }

    /**
     * Toggles between full screen and windowed
     * @author Lawrence Howes-Yarlett
     */
    public void FullScreenToggle()
    {
        Screen.fullScreen = fullScreen.isOn;
        isFullScreen = Screen.fullScreen;
    }

    /**
     * Changes the resolution of the game
     * @author Lawrence Howes-Yarlett
     */
    public void ChangeResolution()
    {
        string option = resolution.options[resolution.value].text;
        string[] tokens = option.Split("x".ToCharArray());
        int max = Int32.Parse(tokens[0]);
        int min = Int32.Parse(tokens[1]);
        isFullScreen = Screen.fullScreen;
        Screen.SetResolution(max, min, isFullScreen);
    }

    /**
     * Change the volume of the game
     * @author Lawrence Howes-Yarlett
     */
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }
}