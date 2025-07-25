using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public class MazeSpawner : MonoBehaviour
{
    public enum MazeGenerationAlgorithm
    {
        PureRecursive
    }
    public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
    public bool fullRandom = false;
    public int randomSeed = 12345;
    public GameObject Floor = null;
    public GameObject Wall = null;
    public GameObject Pillar = null;
    public int Rows = 5;
    public int Columns = 5;
    public float cellWidth = 4;
    public float cellHeight = 4;
    public bool addGaps = false;
    public GameObject goalPrefab = null;
    public float goalHeight = 1.0f;

    private MazeGenerator mazeGenerator = null;
    private ItemSystem itemSystem;

    void Start()
    {

        if (goalPrefab != null)
        {
            GameObject originalGoal = GameObject.Find(goalPrefab.name);
            if (originalGoal != null && originalGoal != goalPrefab)
            {
                Debug.Log("Disabling original goal in hierarchy: " + originalGoal.name);
                originalGoal.SetActive(false);
            }
        }

        itemSystem = Object.FindFirstObjectByType<ItemSystem>();

        if (!fullRandom)
        {
            Random.InitState(randomSeed);
        }
        switch (Algorithm)
        {
            case MazeGenerationAlgorithm.PureRecursive:
                mazeGenerator = new MazeAlgorithm(Rows, Columns);
                break;
        }
        mazeGenerator.GenerateMaze();

        //generate the maze
        GenerateMazeObjects();

        void GenerateMazeObjects()
    {
        //generate the floor, walls, and pillars
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                float x = column * (cellWidth + (addGaps ? .2f : 0));
                float z = row * (cellHeight + (addGaps ? .2f : 0));
                MazeCell cell = mazeGenerator.GetMazeCell(row, column);
                GameObject tmp;

                //floor
                tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                tmp.transform.parent = transform;

                //right wall
                if (cell.wallRight)
                {
                    tmp = Instantiate(Wall, new Vector3(x + cellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;
                    tmp.transform.parent = transform;
                    tmp.tag = "Wall"; 
                }

                //front wall
                if (cell.wallFront)
                {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z + cellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                    tmp.transform.parent = transform;
                    tmp.tag = "Wall"; 
                }

                //left wall
                if (cell.wallLeft)
                {
                    tmp = Instantiate(Wall, new Vector3(x - cellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;
                    tmp.transform.parent = transform;
                    tmp.tag = "Wall"; 
                }

                //back wall
                if (cell.wallBack)
                {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z - cellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
                    tmp.transform.parent = transform;
                    tmp.tag = "Wall"; 
                }

                    //goal
                    if (cell.isGoal && goalPrefab != null)
                    {
                        Debug.Log("Creating goal at position: (" + row + "," + column + ")");

                        //Create the goal object
                        GameObject goalObject = Instantiate(goalPrefab, new Vector3(x, goalHeight, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                       
                        goalObject.SetActive(true);
                      
                        goalObject.transform.parent = transform;
                        goalObject.tag = "Goal";
                        goalObject.name = "MazeGoal";

                        Debug.Log("Goal created with DirectGoalTrigger at " + goalObject.transform.position);
                    }

                    //try spawning item in cell
                    //item cannot spawn at 0,0 or goal

                    if (!(row == 0 && column == 0) && !cell.isGoal && itemSystem != null)
                    {
                        //Try to spawn an item at this cell
                        Vector3 itemPosition = new Vector3(x, 0, z);
                        itemSystem.TrySpawnItem(itemPosition, transform);
                    }
                }
            }
        }

        //generate pillars
        if (Pillar != null)
        {
            for (int row = 0; row < Rows + 1; row++)
            {
                for (int column = 0; column < Columns + 1; column++)
                {
                    float x = column * (cellWidth + (addGaps ? .2f : 0));
                    float z = row * (cellHeight + (addGaps ? .2f : 0));
                    GameObject tmp = Instantiate(Pillar, new Vector3(x - cellWidth / 2, 1, z - cellHeight / 2), Pillar.transform.rotation) as GameObject;
                    tmp.transform.parent = transform;
                }
            }
        }
    }

    //get the spawn position for the players
    public Vector3 GetPlayerSpawnPosition()
    {
        //start position is at the first cell 0,0
        return new Vector3(0, 1, 0); 
    }
}
