using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using Unity.Jobs;
using UnityEngine;

public class loogika : MonoBehaviour
{
    public int startingEnergy = 10;
    public int energy;
    public List<GameObject> tiles;
    public GameObject ButtonPrefab;
    public int yoffset = 2;
    public int xGridStep = 8;
    public int zGridStep = 8;
    public List<int> gridSize = new List<int> {8,8};
    private Camera camera;

    private Vector3 BoardPosition;

    // Define the 8x8 grid
    private int[,] grid;
    private Vector3[,] gridVectors;
    private GameObject[,] väljaolek;
    private PauseMenu pauseMenu;
    private int Score = 0;
    public GameObject floorTile;

    Sequence seq;

    void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
        pauseMenu.SetEnergy(startingEnergy);
        pauseMenu.SetScore(0);
        camera = Camera.main;

        energy = startingEnergy;
        BoardPosition = transform.position;
        grid = new int[gridSize[0], gridSize[1]];
         for (int a = 0; a < gridSize[0]; a++)
        {
            for (int b = 0; b < gridSize[1]; b++)
            {
                grid[a,b] = 0;
            }
        }
        gridVectors = new Vector3[gridSize[0], gridSize[1]+1];
        väljaolek = new GameObject[gridSize[0], gridSize[1]];
        // Calculate grid positions
        for (int i = 0; i < gridSize[0]; i++)
        {
            for (int j = 0; j <= gridSize[1]; j++)
            {
                gridVectors[i,j] = new Vector3((i)*xGridStep+xGridStep/2 - xGridStep*gridSize[0]/2,yoffset ,j*zGridStep+zGridStep/2 - zGridStep*gridSize[1]/2);
                GameObject.Instantiate(floorTile,new Vector3(gridVectors[i,j].x,gridVectors[i,j].y-1,gridVectors[i,j].z),Quaternion.identity, this.transform);
                //Debug.Log(gridVectors[i,j]);
                if(i < gridSize[0]  && j < gridSize[1] )
                {
                    GenerateNewButton(i,j);
                }
            }
        }
        //Populate board state with GenerateNewTile
        for (int j = 0; j < gridSize[1]; j++)
        {
            for (int i = 0; i < gridSize[0]; i++)
            {
                GenerateNewTile(i);
            }
        }
        // Parse over the grid
        //ParseGrid();
    }
     void Update()
    {
       DetectObjectWithRaycast();
    }

    public void DetectObjectWithRaycast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                ClickOnObject clickObject;
                
                    hit.collider.TryGetComponent<ClickOnObject>(out clickObject);
                    if (clickObject != null)
                    {
                    clickObject.OnClick();
                        
                    }
                
                Debug.Log($"{hit.collider.name} Detected",
                    hit.collider.gameObject);
            }
        }
    }
    void GenerateNewButton(float x, float z)
    {
        GameObject.Instantiate(ButtonPrefab, new Vector3(x, 0, z), Quaternion.identity, this.transform);
    }

    void GenerateNewTile(int i)
    {
        // Check if empty
        if (grid[i, gridSize[1]-1] != 0)
        {
            return;
        }
        int uus = UnityEngine.Random.Range(1, 4);
        grid[i, 7] = uus;
        // Spawn new tile GameObject
        Vector3 position = gridVectors[i,gridSize[1]];
        väljaolek[i,gridSize[1]-1] = Instantiate(tiles[uus], position, Quaternion.identity, this.transform);
        for (int c = 0; c < gridSize[0]; c++)
        {
            väljaolek[i,gridSize[1]-1].GetComponent<TileScript>().SetDestination(gridVectors[i,gridSize[1]-1]);
        }
        ParseGridFall();
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

    void MoveTile(int i,int j, int targeti,int targetj){
        if (grid[targeti,targetj] == 0)
        {
            grid[targeti,targetj] = grid[i,j];
            grid[i,j] = 0;
            väljaolek[targeti,targetj] = väljaolek[i,j];
            //Debug.Log( gridVectors[targeti,targetj]);
            väljaolek[i,j].GetComponent<TileScript>().SetDestination(gridVectors[targeti,targetj]);
            väljaolek[i,j] = null;
        } 

    }
        void moveToPosition(int x,int y){
        
            väljaolek[x,y].GetComponent<TileScript>().SetDestination(gridVectors[x,y]);

    }
    private void swap(int x1, int y1, int x2, int y2)
    {
        int hold = grid[x1,y1];
        grid[x1,y1] = grid[x2,y2];
        grid[x2,y2] = hold;

        GameObject GOhold = väljaolek[x1,y1];
        väljaolek[x1,y1] = väljaolek[x2,y2];
        väljaolek[x2,y2] = GOhold;
        moveToPosition(x1,y1);
        moveToPosition(x2,y2);
    }

    private List<EffectStruct> searchForPatterns(int x,int y){
        List<EffectStruct> effectList = new List<EffectStruct>{}; 
        for (int a = 0; a < 2; a++)
        {
            for (int b = 0; b < 2; b++)
            {
                if (grid[x+a,y+b] == 1)
                {
                    //int inilvl = grid[x+a,y+b];
                    //basic triangle
                    //in bounds?
                    
                    
                    pauseMenu.SetEnergy(energy);

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
                if (grid[x+a,y+b] == 2)
                {
                    pauseMenu.SetEnergy(energy);

                    if (x+a+2<gridSize[0] & y+b+2<gridSize[1])
                    {
                        if (grid[x+a+2,y+b]==2)
                        {
                            if (grid[x+a+2,y+b+2]==2)
                            {
                                if (grid[x+a,y+b+2]==2)
                                {
                                    //add effect Square to buffer with direction NE
                                    effectList.Add(new EffectStruct(x+a,y+b,0,1)); 
                                }
                            }
                        }
                    }
                    if (x+a+2<gridSize[0] & 2<=y+b)
                    {
                        if (grid[x+a+2,y+b-2]==2)
                        {
                            if (grid[x+a+2,y+b-2]==2)
                            {
                                if (grid[x+a,y+b-2]==2)
                                {
                                    //add effect Square to buffer with direction SE
                                    effectList.Add(new EffectStruct(x+a,y+b,1,1));
                                }
                            }
                        }
                    }
                    if (2<=x+a & 2<=y+b)
                    {
                        if (grid[x+a-2,y+b]==2)
                        {
                            if (grid[x+a-2,y+b-2]==2)
                            {
                                if (grid[x+a,y+b-2]==2)
                                {
                                    //add effect Square to buffer with direction SW
                                    effectList.Add(new EffectStruct(x+a,y+b,2,1));
                                }
                            }   
                            
                        }
                    }
                    if (2<=x+a & y+b+2<gridSize[1])
                    {
                        if (grid[x+a-2,y+b]==2)
                        {
                            if (grid[x+a-2,y+b+2]==2)
                            {
                                if (grid[x+a,y+b+2]==2)
                                {
                                    //add effect Square to buffer with direction NW
                                    effectList.Add(new EffectStruct(x+a,y+b,3,1));
                                }
                            }
                        }
                    }
                    
                    Debug.Log("looking for type 2 cross");
                    
                    if (1<=x+a & x+a+1<gridSize[0] & y+b+2<gridSize[1])
                    {
                        if (grid[x+a-1,y+b+1]==2)
                        {
                            if (grid[x+a,y+b+2]==2)
                            {
                                if (grid[x+a+1,y+b+1]==2)
                                {
                                    //add effect Cross to buffer with direction N
                                    effectList.Add(new EffectStruct(x+a,y+b,0,2)); 
                                }
                            }
                        }
                    }
                    if (x+a+2<gridSize[0] & 1<=y+b & y+b+1<gridSize[1])
                    {
                        if (grid[x+a+1,y+b+1]==2)
                        {
                            if (grid[x+a+2,y+b]==2)
                            {
                                if (grid[x+a+1,y+b-1]==2)
                                {
                                    //add effect Cross to buffer with direction E
                                    effectList.Add(new EffectStruct(x+a,y+b,1,2));
                                }
                            }
                        }
                    }
                    if (1<=x+a & x+a+1<gridSize[0] & 2<=y-b)
                    {
                        if (grid[x+a+1,y+b-1]==2)
                        {
                            if (grid[x+a,y+b-2]==2)
                            {
                                if (grid[x+a-1,y+b-1]==2)
                                {
                                    //add effect Cross to buffer with direction S
                                    effectList.Add(new EffectStruct(x+a,y+b,2,2));
                                }
                            }   
                            
                        }
                    }
                    if (2<=x+a & 1<=y+b & y+b+1<gridSize[1])
                    {
                        if (grid[x+a-1,y+b-1]==2)
                        {
                            if (grid[x+a-2,y+b]==2)
                            {
                                if (grid[x+a-1,y+b+1]==2)
                                {
                                    //add effect Cross to buffer with direction W
                                    effectList.Add(new EffectStruct(x+a,y+b,3,2));
                                }
                            }
                        }
                    }

                }

            }
        }
        return(effectList);
    }
    
    private void destroy(int x,int y)
    {
        grid[x,y] = 0;
        Destroy(väljaolek[x,y]);
        ParseGridFall();
    }
    private void upgrade(int x,int y){
        if (grid[x,y]<tiles.Count-1)
        {
            grid[x,y] ++;
            //TODO add gfx
            Destroy(väljaolek[x,y]);
            väljaolek[x,y] = Instantiate(tiles[grid[x,y]], gridVectors[x,y], Quaternion.identity, this.transform);
        }
    }
    private void ApplyEffects(List<EffectStruct> EffectList){
        foreach (EffectStruct effect in EffectList)
        {
            
        Debug.Log("appling effects");
            if (effect.type == 0)
            {
                // effect.direction 
                // 0 = up
                // 1 = left
                // 2 = down
                // 3 = left
                Score += 1;
                pauseMenu.SetScore(Score);
                
                if (effect.direction == 0)
                {
                    if (effect.y != gridSize[1] - 1 )
                    {
                        destroy(effect.x,effect.y+1);
                        swap(effect.x-1,effect.y-2,effect.x-1,effect.y-1);
                        swap(effect.x+1,effect.y-2,effect.x+1,effect.y-1);
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
                        destroy(effect.x+1,effect.y);
                        swap(effect.x-2,effect.y-1,effect.x-1,effect.y-1);
                        swap(effect.x-2,effect.y+1,effect.x-1,effect.y+1);
                        if (effect.x != gridSize[0] - 2)
                        {
                            destroy(effect.x+2,effect.y);
                        }
                    }
                    
                }
                else if (effect.direction == 2)
                {
                    if (effect.y != 0 )
                    {
                        destroy(effect.x,effect.y-1);
                        swap(effect.x-1,effect.y+2,effect.x-1,effect.y+1);
                        swap(effect.x+1,effect.y+2,effect.x+1,effect.y+1);
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
                        destroy(effect.x-1,effect.y);
                        swap(effect.x+2,effect.y-1,effect.x+1,effect.y-1);
                        swap(effect.x+2,effect.y+1,effect.x+1,effect.y+1);
                        if (effect.x != 1)
                        {
                            destroy(effect.x-2,effect.y);
                        }
                    }
                }
            }
            if (effect.type == 1)
            {
                energy++;
                Score += 5;
                pauseMenu.SetScore(Score);

                if (effect.direction == 0)
                {
                    
                    Debug.Log("t1d0");
                    upgrade(effect.x+1,effect.y+1); 
                    swap(effect.x+UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2),effect.x+UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2));
                    swap(effect.x+UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2),effect.x+UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2));
                }
                else if (effect.direction == 1)
                {
                    
                    Debug.Log("t1d1");
                    upgrade(effect.x+1,effect.y-1);
                    swap(effect.x+UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2),effect.x+UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2));
                    swap(effect.x+UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2),effect.x+UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2));
                }
                else if (effect.direction == 2)
                {
                    Debug.Log("t1d2");
                    upgrade(effect.x-1,effect.y-1);
                    swap(effect.x-UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2),effect.x-UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2));
                    swap(effect.x-UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2),effect.x-UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2));
                }
                else if (effect.direction == 3)
                {
                    Debug.Log("t1d3");
                    upgrade(effect.x-1,effect.y+1);
                    swap(effect.x-UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2),effect.x-UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2));
                    swap(effect.x-UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2),effect.x-UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2));
                } 
            }
            if (effect.type == 2)
            {
                energy++;
                Score += 10;
                pauseMenu.SetScore(Score);
                
                if (effect.direction == 0)
                {   
                    Debug.Log("t2d0");
                    upgrade(effect.x+1,effect.y+1);
                    swap(effect.x+UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2),effect.x+UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2));
                    swap(effect.x+UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2),effect.x+UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2));
                    swap(effect.x+UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2),effect.x+UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2));
                    swap(effect.x+UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2),effect.x+UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2));
                }
                else if (effect.direction == 1)
                {
                    Debug.Log("t2d1");
                    upgrade(effect.x+1,effect.y-1);
                    swap(effect.x+UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2),effect.x+UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2));
                    swap(effect.x+UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2),effect.x+UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2));
                    swap(effect.x+UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2),effect.x+UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2));
                    swap(effect.x+UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2),effect.x+UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2));
                }
                else if (effect.direction == 2)
                {
                    Debug.Log("t2d2");
                    upgrade(effect.x-1,effect.y-1);
                    swap(effect.x-UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2),effect.x-UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2));
                    swap(effect.x-UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2),effect.x-UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2));
                    swap(effect.x-UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2),effect.x-UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2));
                    swap(effect.x-UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2),effect.x-UnityEngine.Random.Range(0,2),effect.y-UnityEngine.Random.Range(0,2));
                }
                else if (effect.direction == 3)
                {
                    Debug.Log("t2d3");
                    upgrade(effect.x-1,effect.y+1);
                    swap(effect.x-UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2),effect.x-UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2));
                    swap(effect.x-UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2),effect.x-UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2));
                    swap(effect.x-UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2),effect.x-UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2));
                    swap(effect.x-UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2),effect.x-UnityEngine.Random.Range(0,2),effect.y+UnityEngine.Random.Range(0,2));
                } 
            }
        }
    }
    public void ImputPress(int x, int y){
        
        Debug.Log("Click");
        energy--;
        pauseMenu.SetEnergy(energy);

        int[] jugglerIds = new int[] {grid[x,y],grid[x,y+1],grid[x+1,y+1],grid[x+1,y]};

        GameObject[] juggleObjects = {väljaolek[x,y],väljaolek[x,y+1],väljaolek[x+1,y+1],väljaolek[x+1,y]};
        
        grid[x,y] = 0;
        grid[x,y+1] = 0;
        grid[x+1,y+1] = 0;
        grid[x+1,y] = 0;
        
        //MoveTile(x,y,x,y+1);
        juggleObjects[0].GetComponent<TileScript>().SetDestination(gridVectors[x,y+1]);
        //MoveTile(x+1,y,x,y);
        juggleObjects[1].GetComponent<TileScript>().SetDestination(gridVectors[x+1,y+1]);
        //MoveTile(x+1,y+1,x+1,y);
        juggleObjects[2].GetComponent<TileScript>().SetDestination(gridVectors[x+1,y]);
        //MoveTile(x,y+1,x+1,y+1);
        juggleObjects[3].GetComponent<TileScript>().SetDestination(gridVectors[x,y]);
        
        grid[x,y] = jugglerIds[3];
        grid[x,y+1] = jugglerIds[0];
        grid[x+1,y+1] = jugglerIds[1];
        grid[x+1,y] = jugglerIds[2];

        väljaolek[x,y] = juggleObjects[3];
        väljaolek[x,y+1] = juggleObjects[0];
        väljaolek[x+1,y+1] = juggleObjects[1];
        väljaolek[x+1,y] = juggleObjects[2];
        Debug.Log("looking for patterns");
        List<EffectStruct> patterns = searchForPatterns(x,y);
        Debug.Log("done" + patterns.Count);
        //while (patterns.Count>0)
        //{  
        ApplyEffects(patterns);
        //patterns = searchForPatterns(x,y);
        //}
        //applyEffects(searchForPatterns(x,y));
        if (energy < 1)
        {
            pauseMenu.GameOver();
        }

    }
    

}
