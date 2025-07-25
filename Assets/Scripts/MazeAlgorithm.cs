using System.Collections.Generic;
using UnityEngine;

public class MazeAlgorithm : MazeGenerator
{
    //max attempts to generate a solvable maze
    public int maxGenerationAttempts = 5;

    public MazeAlgorithm(int rows, int columns) : base(rows, columns)
    {
    }

    public override void GenerateMaze()
    {
        bool validMazeGenerated = false;
        int attempts = 0;

        while (!validMazeGenerated && attempts < maxGenerationAttempts)
        {
            ResetMaze();
            VisitCell(0, 0, Direction.Start);
            SetGoal();

            //check if maze is solvable
            MazePath path = new MazePath(this);
            validMazeGenerated = path.IsMazeSolvable();

            attempts++;

            if (!validMazeGenerated)
            {
                Debug.LogWarning($"Generated an unsolvable maze. Attempt {attempts} of 5");
            }
        }

        if (!validMazeGenerated)
        {
            throw new System.Exception("Failed to generate a solvable maze after 5 attempts");
        }
        else
        {
            Debug.Log($"Generated a solvable maze after {attempts} attempts");
        }
    }

    private void ResetMaze()
    {
        //Reset all cells
        for (int r = 0; r < rowCount; r++)
        {
            for (int c = 0; c < columnCount; c++)
            {
                MazeCell cell = GetMazeCell(r, c);
                cell.isVisited = false;
                cell.wallRight = false;
                cell.wallFront = false;
                cell.wallLeft = false;
                cell.wallBack = false;
                cell.isGoal = false;
            }
        }
    }

    private void VisitCell(int row, int column, Direction moveMade)
    {
        MazeCell currentCell = GetMazeCell(row, column);

        Direction[] availableMoves = new Direction[4];
        int availableMovesCount = 0;

        do
        {
            availableMovesCount = 0;

            //check right
            if (column + 1 < columnCount && !GetMazeCell(row, column + 1).isVisited)
            {
                availableMoves[availableMovesCount] = Direction.Right;
                availableMovesCount++;
            }
            else if (!currentCell.isVisited && moveMade != Direction.Left)
            {
                currentCell.wallRight = true;
            }
            //check forward
            if (row + 1 < rowCount && !GetMazeCell(row + 1, column).isVisited)
            {
                availableMoves[availableMovesCount] = Direction.Front;
                availableMovesCount++;
            }
            else if (!currentCell.isVisited && moveMade != Direction.Back)
            {
                currentCell.wallFront = true;
            }
            //check left
            if (column > 0 && column - 1 >= 0 && !GetMazeCell(row, column - 1).isVisited)
            {
                availableMoves[availableMovesCount] = Direction.Left;
                availableMovesCount++;
            }
            else if (!currentCell.isVisited && moveMade != Direction.Right)
            {
                currentCell.wallLeft = true;
            }
            //check back
            if (row > 0 && row - 1 >= 0 && !GetMazeCell(row - 1, column).isVisited)
            {
                availableMoves[availableMovesCount] = Direction.Back;
                availableMovesCount++;
            }
            else if (!currentCell.isVisited && moveMade != Direction.Front)
            {
                currentCell.wallBack = true;
            }
            currentCell.isVisited = true;
            if (availableMovesCount > 0)
            {
                switch (availableMoves[Random.Range(0, availableMovesCount)])
                {
                    case Direction.Start:
                        break;
                    case Direction.Right:
                        VisitCell(row, column + 1, Direction.Right);
                        break;
                    case Direction.Front:
                        VisitCell(row + 1, column, Direction.Front);
                        break;
                    case Direction.Left:
                        VisitCell(row, column - 1, Direction.Left);
                        break;
                    case Direction.Back:
                        VisitCell(row - 1, column, Direction.Back);
                        break;
                }
            }
        } while (availableMovesCount > 0);
    }

    private void SetGoal()
    {
        //Reset visited status for all cells
        for (int r = 0; r < rowCount; r++)
        {
            for (int c = 0; c < columnCount; c++)
            {
                GetMazeCell(r, c).isVisited = false;
            }
        }

        //start always 0,0
        int startRow = 0;
        int startCol = 0;

        //find farthest cell
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        queue.Enqueue(new Vector3Int(startRow, startCol, 0));

        int maxDistance = 0;
        int farthestRow = startRow;
        int farthestCol = startCol;

        while (queue.Count > 0)
        {
            Vector3Int current = queue.Dequeue();
            int row = current.x;
            int col = current.y;
            int distance = current.z;

            //Skip if already visited
            if (GetMazeCell(row, col).isVisited)
                continue;

            //Mark as visited
            GetMazeCell(row, col).isVisited = true;

            //Update farthest cell if this one is farther
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestRow = row;
                farthestCol = col;
            }

            MazeCell cell = GetMazeCell(row, col);

            //Check right
            if (!cell.wallRight && col + 1 < columnCount && !GetMazeCell(row, col + 1).isVisited)
                queue.Enqueue(new Vector3Int(row, col + 1, distance + 1));

            //Check front
            if (!cell.wallFront && row + 1 < rowCount && !GetMazeCell(row + 1, col).isVisited)
                queue.Enqueue(new Vector3Int(row + 1, col, distance + 1));

            //Check left
            if (!cell.wallLeft && col > 0 && !GetMazeCell(row, col - 1).isVisited)
                queue.Enqueue(new Vector3Int(row, col - 1, distance + 1));

            //Check back
            if (!cell.wallBack && row > 0 && !GetMazeCell(row - 1, col).isVisited)
                queue.Enqueue(new Vector3Int(row - 1, col, distance + 1));
        }

        //set goal to farthest possible cell from 0,0
        GetMazeCell(farthestRow, farthestCol).isGoal = true;

        Debug.Log($"Goal set at: ({farthestRow}, {farthestCol}) - Distance: {maxDistance}");

        for (int r = 0; r < rowCount; r++)
        {
            for (int c = 0; c < columnCount; c++)
            {
                GetMazeCell(r, c).isVisited = false;
            }
        }
    }


}