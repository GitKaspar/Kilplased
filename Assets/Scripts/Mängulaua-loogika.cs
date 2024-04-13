using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class GridParser : MonoBehaviour
{
  
    public List<GameObject> tiles;
    public GameObject ButtonPrefab;
    public int yoffset = 0;
    public int xGridStep = 8;
    public int zGridStep = 8;
    public List<int> gridSize = new List<int> {8,8};

    private Vector3 BoardPosition;

    // Define the 8x8 grid
    private int[,] grid;
    private Vector3[,] gridVectors;
    private GameObject[,] väljaolek;

    void Start()
    {
        BoardPosition = transform.position;
        grid = new int[gridSize[0], gridSize[1]];
         for (int a = 0; a < gridSize[0]; a++)
        {
            for (int b = 0; b < gridSize[1]; b++)
            {
                grid[a,b] = 0;
            }
        }
        gridVectors = new Vector3[gridSize[0], gridSize[1]];
        väljaolek = new GameObject[gridSize[0], gridSize[1]];
        // Calculate grid positions
        for (int i = 0; i < gridSize[0]; i++)
        {
            for (int j = 0; j < gridSize[1]; j++)
            {
                gridVectors[i,j] = new Vector3((i)*xGridStep+xGridStep/2 - xGridStep*gridSize[0]/2,yoffset ,j*zGridStep+zGridStep/2 - zGridStep*gridSize[1]/2);
                 
                Debug.Log(gridVectors[i,j]);
                if(i < gridSize[0] - 1 && j < gridSize[1] - 1)
                {
                    GenerateNewButton(gridVectors[i, j].x + xGridStep/2, gridVectors[i, j].z + zGridStep/2);
                }
            }
        }
        //Populate board state with GenerateNewTile
        for (int i = 0; i < gridSize[0]; i++)
        {
            GenerateNewTile(i);
        }
        // Parse over the grid
        //ParseGrid();
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
                    for (int t = j-1; t > 0; t--)
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
                //Debug.Log();
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
        int uus = Random.Range(1, 4);
        grid[i, 7] = uus;
        // Spawn new tile GameObject
        Vector3 position = gridVectors[i,gridSize[1]-1];
        väljaolek[i,gridSize[1]-1] = Instantiate(tiles[uus], position, Quaternion.identity, this.transform);
        ParseGridFall();
    }
    void MoveTile(int i,int j, int targeti,int targetj){
        if (grid[i,j] != 0 & grid[targeti,targetj] == 0)
        {
            grid[targeti,targetj] = grid[i,j];
            grid[i,j] = 0;
            väljaolek[targeti,targetj] = väljaolek[i,j];
            //Debug.Log( gridVectors[targeti,targetj]);
            väljaolek[i,j].GetComponent<TileScript>().SetDestination(gridVectors[targeti,targetj]);
        } 

    }

    private List<EffectStruct> searchForPatterns(int x,int y){
        List<EffectStruct> effectList = new List<EffectStruct>{}; 
        for (int a = 0; a < 2; a++)
        {
            for (int b = 0; b < 2; b++)
            {
                if (grid[x+a,y+b] == 1)
                {
                    //basic triangle
                    //in bounds?

                    if (0<=x+a+1 & x+a+1<gridSize[0] & 0<=y+b-2 & y+b-2<gridSize[1])
                    {
                        if (grid[x+a+1,y+b-2]==1)
                        {
                            if (0<=x+a-1 & x+a-1<gridSize[0] & 0<=y+b-2 & y+b-2<gridSize[1])
                            {
                             if (grid[x+a-1,y+b-2]==1){
                                //add effect Triangle to buffer with direction up
                                effectList.Add(new EffectStruct(x+a,y+b,0,0));
                             }   
                            }
                            
                        }
                    }

                    if (0<=x+a-2 & x+a-2<gridSize[0] & 0<=y+b+1 & y+b+1<gridSize[1])
                    {
                        if (grid[x+a-2,y+b+1]==1)
                        {
                            if (0<=x+a-2 & x+a-2<gridSize[0] & 0<=y+b-1 & y+b-1<gridSize[1])
                            {
                             if (grid[x+a-2,y+b-1]==1){
                                //add effect Triangle to buffer with direction right
                                effectList.Add(new EffectStruct(x+a,y+b,1,0));
                             }   
                            }
                            
                        }
                    }

                    if (0<=x+a+1 & x+a+1<gridSize[0] & 0<=y+b+2 & y+b+2<gridSize[1])
                    {
                        if (grid[x+a+1,y+b+2]==1)
                        {
                            if (0<=x+a-1 & x+a-1<gridSize[0] & 0<=y+b+2 & y+b+2<gridSize[1])
                            {
                             if (grid[x+a-1,y+b+2]==1){
                                //add effect Triangle to buffer with direction down
                                effectList.Add(new EffectStruct(x+a,y+b,2,0));
                             }   
                            }
                            
                        }
                    }

                    if (0<=x+a+2 & x+a+2<gridSize[0] & 0<=y+b+1 & y+b+1<gridSize[1])
                    {
                        if (grid[x+a+2,y+b+1]==1)
                        {
                            if (0<=x+a+2 & x+a+2<gridSize[0] & 0<=y+b-1 & y+b+1<gridSize[1])
                            {
                             if (grid[x+a+2,y+b-1]==1){
                                //add effect Triangle to buffer with direction left
                                effectList.Add(new EffectStruct(x+a,y+b,3,0));
                             }   
                            }
                            
                        }
                    }
                }
            }
        }
        return(effectList);
    }
    
    void ParseGrid()
    {
        // Iterate over the grid
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                // Perform operations on each cell
                // For example, you might want to update the GameObject's position or color based on the grid value
                //Debug.Log("Cell [" + i + ", " + j + "] has value: " + grid[i, j]);
            }
        }
    }
    private void destroy(int x,int y)
    {
        grid[x,y] = 0;
        ParseGridFall();
    }
    private void applyEffects(List<EffectStruct> EffectList){
        foreach (EffectStruct effect in EffectList)
        {
            if (effect.type == 0)
            {
                // effect.direction 
                // 0 = up
                // 1 = left
                // 2 = down
                // 3 = left
                if (effect.direction == 0)
                {
                    if (effect.y != gridSize[1] - 1 )
                    {
                        destroy(effect.x,effect.y+1);
                        if (effect.y != gridSize[1] - 2 )
                        {
                            destroy(effect.x,effect.y+2);
                        }
                    }
                }
                else if (effect.direction == 1)
                {
                    if (effect.x != gridSize[0] - 1 )
                    {
                        destroy(effect.x,effect.y+1);
                        if (effect.x != gridSize[0] - 2)
                        {
                            destroy(effect.x,effect.y+2);
                        }
                    }
                    
                }
                else if (effect.direction == 2)
                {
                    if (effect.y != 0 )
                    {
                        destroy(effect.x,effect.y-1);
                        if (effect.y != 1 )
                        {
                            destroy(effect.x,effect.y-2);
                        }
                    }
                    
                }
                else if (effect.direction == 3)
                {
                    if (effect.x != 0 )
                    {
                        destroy(effect.x,effect.y+1);
                        if (effect.x != 1)
                        {
                            destroy(effect.x,effect.y-2);
                        }
                    }
                }
                break;
            }
        }
    }
    public void ImputPress(int x, int y){
        searchForPatterns(x,y);

    }
    void GenerateNewButton(float x, float z)
    {
        GameObject.Instantiate(ButtonPrefab, new Vector3(x, 0, z), Quaternion.identity);
    }
}


