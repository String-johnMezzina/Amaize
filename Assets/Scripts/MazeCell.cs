using UnityEngine;
using System.Collections;

public enum Direction
{
    Start,
    Right,
    Front,
    Left,
    Back,
};

public class MazeCell
{
    public bool isVisited = false;
    public bool wallRight = false;
    public bool wallLeft = false;
    public bool wallBack = false;
    public bool wallFront = false;
    public bool isGoal = false;   
}
