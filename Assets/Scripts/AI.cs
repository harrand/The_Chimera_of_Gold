using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/**
* AI is for creating the AI players and set them to the board. It controls all the AI behaviours (move obstacle, find optimal path) and have the options to choose AI difficulties. 
* @author Zibo Zhang and Yutian Xue
*/
public class DecisionTree : MonoBehaviour
{
    public Vector2 target;
    public Vector3 origin;
	public Board board;
    public bool HardMode { get; private set; }
    int value;

	/**
    * This is to initialize the board member and store the board object.
    * @author Zibo Zhang and Yutian Xue
    * @param board - the whole board object of the game.
    */
    public static DecisionTree Create(Board board, Camp camp, bool hardMode)
    {
        DecisionTree tree = camp.gameObject.AddComponent<DecisionTree>();
        tree.board = board;
        tree.HardMode = hardMode;
        return tree;
    }
    /*
    public DecisionTree(Board board, bool hardMode)
    {
        this.board = board;
        this.HardMode = HardMode;
    }
    */

	/**
    * This is a class which is the used to store the information of BFS search tree.
    * @author Zibo Zhang and Yutian Xue
    */
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

	/**
    * This function is to get a destination tile (where the AI player should move to) which is selected by using a BFS search with limited movements.
    * @author Zibo Zhang and Yutian Xue
    * @param startTile - the start tile which the current player is on
    * @param moves - the limited number of movements which is decided by the dice.
    * @return - the destination tile object.
    */
    public Tile MovementTo(Tile startTile, int moves)
    {
        Vector2 destination = BFS_Find_Path(moves, startTile.PositionTileSpace);   
		target = destination;
		return this.board.GetTileByTileSpace(destination);
	}

	/**
    * This function is to let AI players have the ability to move the obstacles and choose the difficulty of AI.
    * @author Zibo Zhang and Yutian Xue
    * @param para - the tile which the ai player is currently on to check if it lands on an obstacle.
    * @param aiPlayer - currently selected AI pawn.
    */
	public void MoveObstacle(Tile para, Player aiPlayer)
	{
		Obstacle currentObstacle = null;

		if (board.GetObstacleByTile(para) != null)
		{
			currentObstacle = board.GetObstacleByTile (para);
		}

		if (currentObstacle != null) 
		{
            if (this.HardMode)
                ObstacleHardMovement(currentObstacle, aiPlayer);
            else
                ObstacleEasyMovement(currentObstacle);
		}
	}

	/**
    * This is the easy mode for the AI obstacle movements.
    * It randomly choose the position where AI pawns put obstacles and move the obstalce to the destination.
    * @author Zibo Zhang and Yutian Xue
    * @param obstacle - the obstacle object which the AI player currently lands on.
    */
    void ObstacleEasyMovement(Obstacle obstacle)
    {
        Vector2 posi = new Vector2(Random.Range(0, 20), Random.Range(2, 19));
        while (true)
        {
            // should check if there is an obstacle or a player pawn on the tile.
			if (isvalid(posi) && !board.TileOccupiedByObstacle(posi) && !board.TileOccupiedByPlayerPawn(posi) && !isgoal(posi) && posi.y != 1)
            {
                obstacle.transform.position = board.GetTileByTileSpace(posi).transform.position;
                break;
            }
            posi = new Vector2(Random.Range(0, 20), Random.Range(2, 19));
        }

    }

	/**
    * This is the hard mode for the AI obstacle movements.
    * It will block the player pawns which are nearest to the goal position.
    * @author Zibo Zhang and Yutian Xue
    * @param obstacle - the obstacle object which the AI player currently lands on.
    * @param aiPlayer - the AI player in current turn.
    */
    void ObstacleHardMovement(Obstacle obstacle, Player aiPlayer)
    {
        Player[] playerPawns = new Player[board.Camps.Length * 5];
        int q = 0;
        for (int i = 0; i < board.Camps.Length; i++)
        {
            for (int j = 0; j < board.Camps[i].TeamPlayers.Length; j++)
            {
                playerPawns[q] = board.Camps[i].TeamPlayers[j];
                q++;
            }
        }
        Debug.Log("PlayerPawns length K value IS " + q);
        Vector2 invalidPosition = new Vector2(-999, -999);
        Vector2 posi = new Vector2();

        for (int i = 1; i < playerPawns.Length; i++)
        {
            Player temp = playerPawns[i];
            if (playerPawns[i].transform.position.z > playerPawns[i - 1].transform.position.z)
            {
                for (int j = 0; j < i; j++)
                {
                    if (playerPawns[i].transform.position.z > playerPawns[j].transform.position.z)
                    {
                        temp = playerPawns[j];
                        playerPawns[j] = playerPawns[i];
                        playerPawns[i] = temp;
                    }
                }
            }
        }
        for (int k = 0; k < playerPawns.Length; k++)
        {
            if(aiPlayer.GetCamp() == playerPawns[k].GetCamp())
            {
                continue;
            }
            else if (CheckAdjacentObstacles(playerPawns[k]) != invalidPosition)
            {
                posi = CheckAdjacentObstacles(playerPawns[k]);
				obstacle.transform.position = board.GetTileByTileSpace (posi).transform.position;
                return;
            }
            else if(CheckAdjacentObstacles(playerPawns[k]) == invalidPosition)
            {
                ObstacleEasyMovement(obstacle);
            }
        }
        Debug.Log("Can't move the obstacle (hard mode)");
    }

	/**
    * This function is to check if a pawn is surrounded by obstacles.
    * If it is surrounded it will return an invalid position (-999,-999) and if it is not it will return the valid position.
    * @author Zibo Zhang and Yutian Xue
    * @param pawn - the player pawn which is selected to be checked currently.
    * @return - the valid position i.e. the position of the adjacent tile which is not occpupied by an obstacle or an invalid position.
    */
    public Vector2 CheckAdjacentObstacles(Player pawn)
    {
        Player pawnCopy = pawn;
        Vector2 pawnPosition = pawnCopy.GetOccupiedTile().PositionTileSpace;
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
            posi.x = pawnPosition.x + dir[j, 0]; //search the neighbour node 
            posi.y = pawnPosition.y + dir[j, 1];
			if (board.GetObstacleByTileSpace(posi) == null && isvalid(posi) && !board.TileOccupiedByPlayerPawn(posi) && !isgoal(posi) && posi.y != 1)
            {
                //Debug.Log("Hard mode obstacle work successfully! " + posi.x + ", " + posi.y);
                return posi;
            }
            else
            {
                continue;
            }
        }
        //Debug.Log("Hard mode obstacle problem!!");
        return invalidPosition;
    }

	/**
    * this method is to find which path should the AI player finally go after rolling a dice.
    * @author Zibo Zhang and Yutian Xue
    * @param moves - the limited number of movements which decided by the dice.
    * @param startPosition - the current position of AI player.
    * @return - the goal position where the AI player should go.
    */
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

            visited.Add(current.Getposition());  // add the current to the visited list
            moves = tempMoves - maxDepth;

            for (int i = 0; i < 4; ++i)
            {
                Node neighbour = new Node();

                tem.x = (current.Getposition().x) + dir[i, 0]; //search the neighbour node 
                tem.y = (current.Getposition().y) + dir[i, 1];

                neighbour.SetPosition(tem);
				neighbour.SetDepth (0);

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
                            Debug.Log("moves less than 0!!!!");
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
                        neighbour.SetDepth(current.depth + 1);
                        waitList.Enqueue(neighbour);  //join the neighbour to the waitList to wait for next search
                    }
                }
            }

            foreach (var n in waitList)
            {
                if (n.depth > maxDepth)
                    maxDepth = n.depth;
            }
        }

        endPosition = choosePath(path);
        return endPosition;
    }

	/**
    * this method is to find which path is the optimal path.
    * It checks all the possible destiantion after AI player rolling the dice and decide which path it should go.
    * @author Zibo Zhang and Yutian Xue
    * @param path - a list of the possible destination after AI player rolls the dice.
    * @return - the optimal position where the AI player should go.
    */
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
			tmpPosition = path[i];
			distence_to_goal = BFS_Assese_Value(tmpPosition);

			score = distence_to_goal;
			Debug.Log ("Path score: " + score);

			if (distence_to_goal == -1)
			{
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

	/**
    * this method is to assess for the input start position.
    * It use the BFS search, if it meet an obstacle it will get 20 scores and if it moves one step it will get 1 score until it reaches the goal.
    * This means that the lowest score is the optimal position.
    * @author Zibo Zhang and Yutian Xue
    * @param startPosition - the current position needed to be assessed.
    * @return - the score of this position.
    */
    public int BFS_Assese_Value(Vector2 startPosition)
    {
        // four direction: up,right,down,left 
        int[,] dir = new int[4, 2]
        {
            {0, 1}, {1, 0},
            {0, -1}, {-1, 0}
        };

		var visited = new HashSet<Vector2>(); //store node that has been visited
		var waitList = new Queue<Node>();  //store node that need to be visited
		Vector2 goalposition = new Vector2 (10, 18);
        Node start = new Node(0, startPosition);
        waitList.Enqueue(start);

        while (waitList.Count > 0)
        {
            Node current = waitList.Dequeue();
            Vector2 tem = new Vector2();

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
					Debug.Log ("In asses neighbour score: " + neighbour.depth);
                    return neighbour.depth; // when we found the goal
                }
            }

        }
        return -1;

    }

	/**
    * this method is to check if the current position is a valid position (on the board).
    * @author Zibo Zhang and Yutian Xue
    * @param position - the current position.
    * @return - true if this is a valid tile and false if this is a invalid tile.
    */
    public bool isvalid(Vector2 position)
    {
        int x = (int)position.x;
        int y = (int)position.y;

        if (y == 0 || x == -1 || y == 20 || x == 21)
        {
            return false;
        }
		if (y == 1) {
			return true;
		} else if (y == 2) {
			if (x == 0 || x == 4 || x == 8 || x == 12 || x == 16 || x == 20)
				return true;
			else
				return false;
		} else if (y == 3) {
			return true;
		} else if (y == 4 || y == 5) {
			if (x == 2 || x == 6 || x == 10 || x == 14 || x == 18)
				return true;
			else
				return false;
		} else if (y == 6) {
			if (x >= 2 && x <= 18)
				return true;
			else
				return false;
		} else if (y == 7) {
			if (x == 4 || x == 8 || x == 12 || x == 16)
				return true;
			else
				return false;
		} else if (y == 8) {
			if (x >= 4 && x <= 16)
				return true;
			else
				return false;
		} else if (y == 9 || y == 10) {
			if (x == 6 || x == 14)
				return true;
			else
				return false;
		} else if (y == 11) {
			if (x >= 6 && x <= 14)
				return true;
			else
				return false;
		} else if (y == 12) {
			if (x == 8 || x == 12)
				return true;
			else
				return false;
		} else if (y == 13) {
			if (x >= 8 && x <= 12)
				return true;
			else
				return false;
		} else if (y == 14) {
			if (x == 10)
				return true;
			else
				return false;
		} else if (y == 15) {
			if (x >= 4 && x <= 16)
				return true;
			else
				return false;
		} else if (y == 16 || y == 17) {
			if (x == 4 || x == 16)
				return true;
			else
				return false;
		} else if (y == 18) {
			if (x == 4 || x == 16 || x == 10)
				return true;
			else
				return false;
		}
        else if (y == 19) {
            if (x >= 4 && x <= 16)
                return true;
            else
                return false;
        }
        else
            return false;
    }

	public bool isgoal(Vector2 posi)
	{
		if ((int)posi.x == 10 && (int)posi.y == 18)
			return true;
		else
			return false;
	}

}
