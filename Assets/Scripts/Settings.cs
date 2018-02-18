﻿using System;
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
        string option = resolution.options[resolution.value].text;
        string[] tokens = option.Split(" x ".ToCharArray());
        int max = Int32.Parse(tokens[0]);
        int min = Int32.Parse(tokens[1]);
        Debug.Log(max);
        Debug.Log(min);
        Screen.SetResolution(max, min, isFullScreen);
    }
}