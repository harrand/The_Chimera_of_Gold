using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* this script is written by Yutian and Zibo */
public class DecisionTree : MonoBehaviour
{
    public Vector3 target;
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

    public int Movement(int index, int moves)
    {
        Debug.Log("Start movement!");
        //go to (x, y, z)
        Vector2 endPosition = new Vector2();
        Vector2 startPosition = new Vector2();

        startPosition.x = transform.position.x;
        startPosition.y = transform.position.y;

        endPosition = BFS_Find_Path(moves, startPosition);

        target.x = endPosition.x;
        target.y = endPosition.y;
        transform.position = target;
        return moves;
    }

    public Tile MovementTo(Tile startTile, int moves)
    {
        Vector2 destination = BFS_Find_Path(moves, startTile.PositionTileSpace);
        return this.board.GetTileByTileSpace(destination);
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
            Debug.Log("Visited: " + current.Getposition().x + ", " + current.Getposition().y);
            moves = tempMoves - maxDepth;

            for (int i = 0; i < 4; ++i)
            {
                Node neighbour = new Node();

                tem.x = (current.Getposition().x) + dir[i, 0]; //search the neighbour node 
                tem.y = (current.Getposition().y) + dir[i, 1];
                Debug.Log("i = " + i + "; Temp Neighbor Position: " + tem.x + ", " + tem.y);

                neighbour.SetPosition(tem);
                neighbour.SetDepth(0);

                //neighbour.x = current.x+dir[i,0]; //search the neighbour node 
                //neighbour.y = current.y+dir[i,1];

                Debug.Log("Rest moves: " + moves + "  Max depth: " + maxDepth + "  Current Position: " + current.Getposition().x + ", " + current.Getposition().y);

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
                                Debug.Log("I meet the obstacle oh so sad!");
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
                        Debug.Log("Neighbour: " + neighbour.depth);
                        neighbour.SetDepth(current.depth + 1);
                        //maxDepth = neighbour.depth;
                        Debug.Log("After setting Neighbour: " + neighbour.depth);
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
                    Debug.Log("We found the goal! distance: " + neighbour.depth);
                    Debug.Log(startPosition);
                    return neighbour.depth; // when we found the goal
                }
            }

        }
        Debug.Log("we can't find the goal!!!!!!");
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
            Debug.Log(path.Count);
            tmpPosition = path[i];
            distence_to_goal = BFS_Assese_Value(tmpPosition);
            Debug.Log("Distence to the goal: " + distence_to_goal);

            score = distence_to_goal;
            Debug.Log("Score: " + score);

            if (distence_to_goal == -1)
            {
                Debug.Log("error in calculating distence!!!!!!");
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

    // we pass in [0,0] and [-1, 0] returns [0, 1 in tile space]
    public static Vector2 TreeToTile(Vector2 nodeTileSpace, Vector2 treeSpace)
    {
        return new Vector2(-treeSpace.y, -treeSpace.x);
    }
}
