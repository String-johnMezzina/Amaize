using System.Collections.Generic;
using UnityEngine;

public class MazePath
{
    private MazeGenerator mazeGenerator;
    private int rows;
    private int columns;

    //node class for A* algorithm
    private class PathNode
    {
        public int row;
        public int column;
        public float gCost; //cost from start
        public float hCost; //heuristic cost to goal
        public float fCost => gCost + hCost; //total cost
        public PathNode parent;

        public PathNode(int row, int column)
        {
            this.row = row;
            this.column = column;
            this.gCost = float.MaxValue;
            this.hCost = 0;
            this.parent = null;
        }
    }

    public MazePath(MazeGenerator mazeGenerator)
    {
        this.mazeGenerator = mazeGenerator;
        this.rows = mazeGenerator.rowCount;
        this.columns = mazeGenerator.columnCount;
    }

    //Heuristic function 
    private float CalculateHeuristic(int startRow, int startCol, int goalRow, int goalCol)
    {
        return Mathf.Abs(startRow - goalRow) + Mathf.Abs(startCol - goalCol);
    }

    //find path from start to goal using A*
    public bool FindPath(Vector2Int start, Vector2Int goal, out List<Vector2Int> path)
    {
        path = new List<Vector2Int>();

        //Check if start and goal are valid
        if (start.x < 0 || start.x >= rows || start.y < 0 || start.y >= columns ||
            goal.x < 0 || goal.x >= rows || goal.y < 0 || goal.y >= columns)
        {
            Debug.LogError("Invalid start or goal position");
            return false;
        }

        //create grid of nodes
        PathNode[,] grid = new PathNode[rows, columns];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                grid[r, c] = new PathNode(r, c);
            }
        }

        //initialize open and closed sets
        List<PathNode> openSet = new List<PathNode>();
        HashSet<PathNode> closedSet = new HashSet<PathNode>();

        //start node
        PathNode startNode = grid[start.x, start.y];
        startNode.gCost = 0;
        startNode.hCost = CalculateHeuristic(start.x, start.y, goal.x, goal.y);
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            //node with lowest fCost
            PathNode current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < current.fCost ||
                    (openSet[i].fCost == current.fCost && openSet[i].hCost < current.hCost))
                {
                    current = openSet[i];
                }
            }

            //remove current from open set and add to closed set
            openSet.Remove(current);
            closedSet.Add(current);

            //check if reached goal
            if (current.row == goal.x && current.column == goal.y)
            {
                //reconstruct path
                ReconstructPath(grid[goal.x, goal.y], path);
                return true;
            }

            //process neighbors
            List<PathNode> neighbors = GetNeighbors(current, grid);
            foreach (PathNode neighbor in neighbors)
            {
                if (closedSet.Contains(neighbor))
                    continue;

                float tentativeGCost = current.gCost + 1; //distance between nodes is always 1

                if (tentativeGCost < neighbor.gCost)
                {
                    neighbor.parent = current;
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = CalculateHeuristic(neighbor.row, neighbor.column, goal.x, goal.y);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        //no path found
        return false;
    }

    //get accessible neighbors for a node
    private List<PathNode> GetNeighbors(PathNode node, PathNode[,] grid)
    {
        List<PathNode> neighbors = new List<PathNode>();
        MazeCell currentCell = mazeGenerator.GetMazeCell(node.row, node.column);

        //Check right
        if (!currentCell.wallRight && node.column + 1 < columns)
        {
            neighbors.Add(grid[node.row, node.column + 1]);
        }

        //Check front
        if (!currentCell.wallFront && node.row + 1 < rows)
        {
            neighbors.Add(grid[node.row + 1, node.column]);
        }

        //Check left
        if (!currentCell.wallLeft && node.column > 0)
        {
            neighbors.Add(grid[node.row, node.column - 1]);
        }

        //Check back
        if (!currentCell.wallBack && node.row > 0)
        {
            neighbors.Add(grid[node.row - 1, node.column]);
        }

        return neighbors;
    }

    //reconstruct the path from goal to start
    private void ReconstructPath(PathNode endNode, List<Vector2Int> path)
    {
        PathNode current = endNode;

        while (current != null)
        {
            path.Add(new Vector2Int(current.row, current.column));
            current = current.parent;
        }

        //reverse the path to get start to goal
        path.Reverse();
    }

    //method to validate if maze is solvable
    public bool IsMazeSolvable()
    {
        //find goal cell
        Vector2Int goalPos = Vector2Int.zero;
        bool goalFound = false;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (mazeGenerator.GetMazeCell(r, c).isGoal)
                {
                    goalPos = new Vector2Int(r, c);
                    goalFound = true;
                    break;
                }
            }
            if (goalFound) break;
        }

        if (!goalFound)
        {
            Debug.LogWarning("No goal cell found in maze");
            return false;
        }

        //start position is always (0,0)
        Vector2Int startPos = new Vector2Int(0, 0);

        //check if there's a path from start to goal
        List<Vector2Int> path;
        bool solvable = FindPath(startPos, goalPos, out path);

        if (solvable)
        {
            Debug.Log($"Maze is solvable with path length: {path.Count}");
        }
        else
        {
            Debug.LogWarning("Maze is not solvable!");
        }

        return solvable;
    }
}