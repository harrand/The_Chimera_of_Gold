using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static uint numberOfPlayers = 2;
    public static bool[] isAIPlayer = new bool[5];
    public static bool[] isHardAI = new bool[5];
    public static Color winnerColour;
    public static int winnerNumber = 0;
}
