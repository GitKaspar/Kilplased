using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridParser : MonoBehaviour
{
    // Declare 8 public GameObjects
    public List<GameObject> tiles;
    public int yoffset = 0;
    public int xGridStep = 8;
    public int zGridStep = 8;
    public List<int> gridSize = new List<int> {8,8};

    private Vector3 BoardPosition;

    // Define the 8x8 grid
    private int[,] grid;
    private GameObject[,] väljaolek; 
    void Start()
    {
        BoardPosition = transform.position;
        grid = new int[gridSize[0], gridSize[1]];
        väljaolek = new GameObject[gridSize[0], gridSize[1]];
        // Initialize the grid with some values
        // TODO: Populate board state with GenerateNewTile
        for (int i = 0; i < gridSize[0]; i++)
        {
            GenerateNewTile(i);
//            for (int j = 0; j < 8; j++)
//            {
//                grid[i, j] = Random.Range(0,3);
//            }
        }

        // Parse over the grid
        ParseGrid();
    }

    void ParseGrid()
    {
        // Iterate over the grid
        for (int i = 0; i < gridSize[0]; i++)
        {
            for (int j = 0; j < gridSize[1]; j++)
            {
                // Perform operations on each tile
                
                Debug.Log("Cell [" + i + ", " + j + "] has value: " + grid[i, j]);
            }
        }
    }

    void ParseGridFall()
    {
        // Iterate over the grid
        for (int i = 0; i < gridSize[0]; i++)
        {
            for (int j = 1; j < gridSize[1]; j++)
            {
                // Perform operations on each tile
                if (grid[i,j] != 0 & grid[i,j-1] == 0 )
                {
                    //TODO: move tiles down here
                    int targetj = j-1;
                    for (int t = j-1; t > 0; i--)
                    {
                        if (grid[i,t] != 0)
                        {
                            targetj = t+1;
                            break;
                        }
                    }
                    MoveTile(i,j,i,targetj);
                    GenerateNewTile(i);
                }
                //Debug.Log("Cell [" + i + ", " + j + "] has value: " + grid[i, j]);
            }
        }

    }
    void GenerateNewTile(int i)
    {
        // Check if empty
        if (grid[i, gridSize[1]-1] != 0)
        {
            return;
        }
        int uus = Random.Range(0, 3);
        grid[i, 7] = uus;
        // Spawn new tile GameObject
        Vector3 position = new Vector3(i*xGridStep+xGridStep/2 - xGridStep*gridSize[0], zGridStep*gridSize[1]+zGridStep/2 , yoffset);
        väljaolek[i,7] = Instantiate(tiles[uus], position, Quaternion.identity, this.transform);
        ParseGridFall();
    }
    void MoveTile(int i,int j, int targeti,int targetj){
        if (grid[i,j] != 0 & grid[targeti,targetj] == 0)
        {
            grid[targeti,targetj] = grid[i,j];
            grid[i,j] = 0;
            väljaolek[targeti,targetj] = väljaolek[i,j];
            väljaolek[i,j].SetDestination();
        } 

    }

}
