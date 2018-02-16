using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObstacle : TestBase
{
    private Obstacle obstacleScript;

    public TestObstacle()
    {
        GameObject boardObject = new GameObject();
        Board testBoard = Board.CreateNoTerrain(boardObject, 800, 800, 10, 10);
        this.obstacleScript = testBoard.Obstacles[0];
        this.success = true;
    }
}
