using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/* this script is written by Yutian and Zibo */
public class DecisionTree : MonoBehaviour
{
    public Vector2 target;
    public Vector3 origin;
    int value;
    //public ObstacleManager obstaclescript;
    public Board board;

    public DecisionTree()
    {

    }

    class Node
    {
        public Vector2 position;
        public int depth = -1;
        public Node(int depth, Vector2 position)
        {
            this.depth = depth;
            this.position = position;
        }

        public Node()
        {

        }

        public Vector2 Getposition()
        {
            return position;
        }

        public void SetPosition(Vector2 arg)
        {
            this.position = arg;
        }
        public void SetDepth(int arg)
        {
            this.depth = arg;
        }
    }

    /*public int Movement(int index, int moves)
    {
        //go to (x, y, z)
        Vector2 endPosition = new Vector2();
        Vector2 startPosition = new Vector2();

        startPosition.x = transform.position.x;
        startPosition.y = transform.position.y;

        endPosition = BFS_Find_Path(moves, startPosition);

        target.x = endPosition.x;
        target.y = endPosition.y;
        transform.position = target;
		Obstacle currentObstacle = null;

		/* let AI have the ability to move obstacles */
	/*
		if (board.GetObstacleByTileSpace(target) != null)
		{
			currentObstacle = board.GetObstacleByTileSpace (target);
		}

		if (currentObstacle != null) 
		{
			ObstacleEasyMovement (currentObstacle);
		}
        return moves;
    }*/

	/* Obstacle movement for easy mode */
	void ObstacleEasyMovement (Obstacle obstacle)
	{
		Vector2 posi = new Vector2 (Random.Range (0, 20), Random.Range (1, 18));
		while (true) 
		{
			// should check if there is a Obstacle on the tile.
			if (isvalid (posi)) 
			{
				obstacle.transform.position = board.GetTileByTileSpace(posi).transform.position;
				break;
			}
			posi = new Vector2 (Random.Range (0, 20), Random.Range (1, 18));
		}

	}
		
	/* Obstacle movement for hard mode */
	void ObstacleHardMovement(Obstacle obstacle)
	{
		Player[] playerPawns = new Player[board.Camps.Length * 5];
		int q = 0;
		for (int i = 0; i < board.Camps.Length; i++) 
		{
			for (int j = 0; j < board.Camps[i].TeamPlayers.Length; j++) 
			{
				playerPawns[q] = board.Camps [i].TeamPlayers[j];
				q++;
			}
		}
		Debug.Log ("PlayerPawns length K value IS " + q);
		Vector2 invalidPosition = new Vector2(-999, -999);
		Vector2 posi = new Vector2 ();

		for (int i = 1; i < playerPawns.Length; i++)
		{
			Player temp = playerPawns[i];
			if(playerPawns[i].transform.position.y > playerPawns[i - 1].transform.position.y)
			{
				for(int j = 0; j < i; j++)
				{
					if(playerPawns[i].transform.position.y > playerPawns[j].transform.position.y)
					{
						temp = playerPawns[j];
						playerPawns[j] = playerPawns[i];
						playerPawns[i] = temp;
					}
				}
			}
		}
		for(int k = 0; k < playerPawns.Length; k++)
		{
			if (CheckAdjacentObstacles(playerPawns[k]) != invalidPosition)
			{
				posi = CheckAdjacentObstacles(playerPawns[k]);
				obstacle.transform.position = posi;
				Debug.Log("*********************Position: (" + posi.x + ", " + posi.y);
				return;
			}
		}
		Debug.Log("Can't move the obstacle (hard mode)");
	}

	public Vector2 CheckAdjacentObstacles(Player pawn)
	{
		Player pawnCopy = pawn;
		Vector2 invalidPosition = new Vector2(-999, -999);
		Vector2 posi = new Vector2();
		int[,] dir = new int[8, 2]
		{
			{0, 1}, {1, 0},
			{0, -1}, {-1, 0},
			{0, 2}, {2, 0},
			{0, -2}, {-2, 0}
		};
		for (int j = 0; j < 8; ++j)
		{
			posi.x = (pawnCopy.transform.position.x) + dir[j, 0]; //search the neighbour node 
			posi.y = (pawnCopy.transform.position.y) + dir[j, 1];
			if (board.GetObstacleByTileSpace(target) == null && isvalid(posi))
			{
				return posi;
			}
			else
			{
				continue;
			}
		}
		return invalidPosition;
	}
	
    public Tile MovementTo(Tile startTile, int moves)
    {
        Vector2 destination = BFS_Find_Path(moves, startTile.PositionTileSpace);   
		target = destination;
		return this.board.GetTileByTileSpace(destination);
	}

	public void MoveObstacle(Tile para)
	{
		Obstacle currentObstacle = null;

		/* let AI have the ability to move obstacles */
		if (board.GetObstacleByTile(para) != null)
		{
			currentObstacle = board.GetObstacleByTile (para);
		}

		if (currentObstacle != null) 
		{
			ObstacleEasyMovement (currentObstacle);
		}
	}

    /*
    public DecisionTree(ObstacleManager obstaclescript)
    {
        this.obstaclescript = obstaclescript;
    }
    */
    public DecisionTree(Board board)
    {
        this.board = board;
    }

    //this method is to find all possibilities of paths after rolling a dice.
    public Vector2 BFS_Find_Path(int moves, Vector2 startPosition)
    {
        // four direction: up,right,down,left 
        int[,] dir = new int[4, 2]
        {
            {0, 1}, {1, 0},
            {0, -1}, {-1, 0}
        };
        int maxDepth = 0;
        int tempMoves = moves;
        var path = new List<Vector2>();
        var visited = new HashSet<Vector2>(); //store node that has been visited
        var endPosition = new Vector2();

        var waitList = new Queue<Node>();  //store node that need to be visited

        Node start = new Node(0, startPosition);

        waitList.Enqueue(start);
        while (waitList.Count > 0 && moves >= 0)
        {
            Node current = waitList.Dequeue();
            Vector2 tem = new Vector2();
            //Debug.Log("x position = " + current.x + "  y position = " + current.y);  //use to debug

            visited.Add(current.Getposition());  // add the current to the visited list
            //Debug.Log("Visited: " + current.Getposition().x + ", " + current.Getposition().y);
            moves = tempMoves - maxDepth;

            for (int i = 0; i < 4; ++i)
            {
                Node neighbour = new Node();

                tem.x = (current.Getposition().x) + dir[i, 0]; //search the neighbour node 
                tem.y = (current.Getposition().y) + dir[i, 1];
                //Debug.Log("i = " + i + "; Temp Neighbor Position: " + tem.x + ", " + tem.y);

                neighbour.SetPosition(tem);
                neighbour.SetDepth(0);

                //neighbour.x = current.x+dir[i,0]; //search the neighbour node 
                //neighbour.y = current.y+dir[i,1];

                //Debug.Log("Rest moves: " + moves + "  Max depth: " + maxDepth + "  Current Position: " + current.Getposition().x + ", " + current.Getposition().y);

                if ((isvalid(neighbour.Getposition())) && !visited.Contains(neighbour.Getposition()))
                {
                    Tile neighbourTile = this.board.GetTileByTileSpace(neighbour.Getposition());
                    if (neighbourTile.BlockedByObstacle())
                    {
                        if (tempMoves - current.depth != 1)
                        {
                            if (!path.Contains(current.Getposition()))
                            {
                                path.Add(current.Getposition());
                                //Debug.Log("I meet the obstacle oh so sad!");
                            }
                            continue;
                        }
                        else if (tempMoves - current.depth == 1)
                        {
                            if (!path.Contains(neighbour.Getposition()))
                            {
                                path.Add(neighbour.Getposition());
                            }
                            continue;
                        }
                        else
                        {
                            //Debug.Log("moves less than 0!!!!");
                        }
                    }
                    if (tempMoves - current.depth == 1)
                    {
                        if (!path.Contains(neighbour.Getposition()))
                        {
                            path.Add(neighbour.Getposition());
                        }
                        continue;
                    }
                    else
                    {
                        //Debug.Log("Neighbour: " + neighbour.depth);
                        neighbour.SetDepth(current.depth + 1);
                        //maxDepth = neighbour.depth;
                        //Debug.Log("After setting Neighbour: " + neighbour.depth);
                        waitList.Enqueue(neighbour);  //join the neighbour to the waitList to wait for next search
                    }
                }
            }

            foreach (var n in waitList)
            {
                if (n.depth > maxDepth)
                    maxDepth = n.depth;
                //Debug.Log("Temp depth: " + maxDepth);
            }
        }

        endPosition = choosePath(path);
        return endPosition;
    }

    //this method is to decide which path should the AI to go. It compares the distance of goal position and the end position after moving.
    public int BFS_Assese_Value(Vector2 startPosition)
    {
        // four direction: up,right,down,left 
        int[,] dir = new int[4, 2]
        {
            {0, 1}, {1, 0},
            {0, -1}, {-1, 0}
        };

        Vector2 goalposition = new Vector2(10, 18);
        // Tile goalTile = this.board.GetTileByTileSpace(new Vector2(10, 18));

        var visited = new HashSet<Vector2>(); //store node that has been visited

        var waitList = new Queue<Node>();  //store node that need to be visited
        Node start = new Node(0, startPosition);
        waitList.Enqueue(start);

        while (waitList.Count > 0)
        {
            Node current = waitList.Dequeue();
            Vector2 tem = new Vector2();

            //Debug.Log("x position = " + current.x + "  y position = " + current.y);  //use to debug

            visited.Add(current.Getposition());  // add the current to the visited list

            for (int i = 0; i < 4; ++i)
            {
                Node neighbour = new Node();
                tem.x = (current.Getposition().x) + dir[i, 0]; //search the neighbour node 
                tem.y = (current.Getposition().y) + dir[i, 1];

                neighbour.SetPosition(tem);
                neighbour.SetDepth(0);

                if ((isvalid(neighbour.Getposition())) && !visited.Contains(neighbour.Getposition()))
                {
                    neighbour.SetDepth(current.depth + 1);
                    Tile neighbourTile = this.board.GetTileByTileSpace(neighbour.Getposition());
                    if (neighbourTile.BlockedByObstacle())
                    {
                        neighbour.depth += 20; //this means that if there is an obstacle the value should plus 20
                    }
                    waitList.Enqueue(neighbour);  //join the neighbour to the waitList to wait for next search
                }

                if (neighbour.Getposition() == goalposition)
                {
                    //Debug.Log("We found the goal! distance: " + neighbour.depth);
                    //Debug.Log(startPosition);
                    return neighbour.depth; // when we found the goal
                }
            }

        }
        //Debug.Log("we can't find the goal!!!!!!");
        return -1;

    }

    public Vector2 choosePath(List<Vector2> path)
    {
        Dictionary<Vector2, int> myPath = new Dictionary<Vector2, int>();
        Vector2 endPosition = new Vector2();
        Vector2 tmpPosition = new Vector2();
        int distence_to_goal = -1;
        int score = 0;
        int tmpScore = int.MaxValue;

        for (int i = 0; i < path.Count; i++)
        {
            //Debug.Log(path.Count);
            tmpPosition = path[i];
            distence_to_goal = BFS_Assese_Value(tmpPosition);
            //Debug.Log("Distence to the goal: " + distence_to_goal);

            score = distence_to_goal;
            //Debug.Log("Score: " + score);

            if (distence_to_goal == -1)
            {
                //Debug.Log("error in calculating distence!!!!!!");
                score = 0;
            }

            myPath.Add(tmpPosition, score);
        }

        foreach (var emt in myPath)
        {
            if (emt.Value < tmpScore)
            {
                endPosition = emt.Key;
                tmpScore = emt.Value;
            }
        }
        return endPosition;
    }

    public void resetMoves()
    {
        //path = null;

    }

    public bool isvalid(Vector2 position)
    {
        int x = (int)position.x;
        int y = (int)position.y;

        if (y == -1 || x == -1 || y == 19 || x == 21)
        {
            return false;
        }
        if (y == 0)
        {
            return true;
        }
        else if (y == 1)
        {
            if (x == 0 || x == 4 || x == 8 || x == 12 || x == 16 || x == 20)
                return true;
            else
                return false;
        }
        else if (y == 2)
        {
            return true;
        }
        else if (y == 3 || y == 4)
        {
            if (x == 2 || x == 6 || x == 10 || x == 14 || x == 18)
                return true;
            else
                return false;
        }
        else if (y == 5)
        {
            if (x >= 2 && x <= 18)
                return true;
            else
                return false;
        }
        else if (y == 6)
        {
            if (x == 4 || x == 8 || x == 12 || x == 16)
                return true;
            else
                return false;
        }
        else if (y == 7)
        {
            if (x >= 4 && x <= 16)
                return true;
            else
                return false;
        }
        else if (y == 8 || y == 9)
        {
            if (x == 6 || x == 14)
                return true;
            else
                return false;
        }
        else if (y == 10)
        {
            if (x >= 6 && x <= 14)
                return true;
            else
                return false;
        }
        else if (y == 11)
        {
            if (x == 8 || x == 12)
                return true;
            else
                return false;
        }
        else if (y == 12)
        {
            if (x >= 8 && x <= 12)
                return true;
            else
                return false;
        }
        else if (y == 13)
        {
            if (x == 10)
                return true;
            else
                return false;
        }
        else if (y == 14)
        {
            if (x >= 4 && x <= 16)
                return true;
            else
                return false;
        }
        else if (y == 15 || y == 16 || y == 17)
        {
            if (x == 4 || x == 16)
                return true;
            else
                return false;
        }
        else if (y == 18)
        {
            if (x >= 4 && x <= 16)
                return true;
            else
                return false;
        }
        else
            return false;
    }
}
