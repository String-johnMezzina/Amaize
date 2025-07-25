using UnityEngine;

public abstract class MazeGenerator
{
    //Gets the row and column from the private variables
    public int rowCount { get { return mazeRows; } }
    public int columnCount { get { return mazeColumns; } }

    private int mazeRows;
    private int mazeColumns;
    private MazeCell[,] Maze;

    //makes rows and colmuns not zero
    //instantiates a new mazeCell at that specific rank and range
    public MazeGenerator(int rows, int columns)
    {
        mazeRows = Mathf.Abs(rows);
        mazeColumns = Mathf.Abs(columns);

        if (mazeRows == 0)
        {
            mazeRows = 1;
        }

        if (mazeColumns == 0)
        {
            mazeColumns = 1;
        }

        Maze = new MazeCell[rows, columns];
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Maze[row, column] = new MazeCell();
            }
        }

    }


    //called by the algorithm class to start
    public abstract void GenerateMaze();

    public MazeCell GetMazeCell(int row, int column)
    {
        if (row >= 0 && column >= 0 && row < mazeRows && column < mazeColumns)
        {
            return Maze[row, column];
        }
        else
        {
            Debug.Log(row + " " + column);
            throw new System.ArgumentOutOfRangeException();
        }
    }
}

