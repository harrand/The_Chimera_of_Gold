using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static uint numberOfPlayers = 2;
    public static bool[] isAIPlayer = new bool[5];
    public static bool[] isHardAI = new bool[5];
    //public static Camp[] winners = new Camp[5];
    public static Color winnerColour;
    public static int winnerNumber = 0;
    public static Color secondColour;
    public static int secondNumber = 0;
    public static Color thirdColour;
    public static int thirdNumber = 0;
    public static Color fourthColour;
    public static int fourthNumber = 0;
    public static Color fifthColour;
    public static int fifthNumber = 0;
}
