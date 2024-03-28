using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    public StateEnum state = StateEnum.DEAD;
    public List<Cell> neighbors;
    public int[] position= new int[2];

    public GameObject cubePrefab;
    public GameObject cube;

    public int count=0;
    public StateEnum nextState = StateEnum.DEAD;

    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Cell created");
        Vector3 cubePosition=new Vector3(transform.position.x,transform.position.y,cubePrefab.transform.localScale.z/2);
        cube = Instantiate(cubePrefab, cubePosition, Quaternion.identity);
        cube.SetActive(false);
        
    }

    // Update is called once per frame
    public void UpdateCells()
    {
   

        UpdateNeighbors();
        ThreeNeighborRule();


    }
    private void OnMouseDown() {
        Debug.Log("Cell clicked");
        if (state == StateEnum.DEAD)
        {
            state = StateEnum.ALIVE;
        }
    }


   void UpdateNeighbors() {
    neighbors = new List<Cell>();
    (int, int) dimGrid = (GridManager.instance.width, GridManager.instance.height);
    Cell[,] cells=GridManager.instance.Cells;

    // Parcourir les décalages relatifs pour les voisins
    for (int dx = -1; dx <= 1; dx++) {
        for (int dy = -1; dy <= 1; dy++) {
            // Ignorer la position de la cellule elle-même (décalage 0,0)
            if (dx == 0 && dy == 0) continue;

            int neighborX = position[0] + dx;
            int neighborY = position[1] + dy;

            // Vérifier si la position est dans les limites de la grille
            if (neighborX >= 0 && neighborX < dimGrid.Item1 && neighborY >= 0 && neighborY < dimGrid.Item2) {
                neighbors.Add(cells[neighborX, neighborY]);
            }
        }
    }
}


           void ThreeNeighborRule(){
            Cell[,] cells=GridManager.instance.Cells;
            count=0;
                foreach (Cell neighbor in neighbors){
                    if (neighbor.state==StateEnum.ALIVE){
                        count++;
                    }
                }
                if (count==3 && state==StateEnum.DEAD){
                    nextState=StateEnum.ALIVE;}

                else if ((count==2 || count==3) && state==StateEnum.ALIVE){
                    nextState=StateEnum.ALIVE;
                }else{
                    nextState=StateEnum.DEAD;}
        
        
            }

                       
        private void Update() {
                    if (state == StateEnum.DEAD){
           GetComponent<SpriteRenderer>().color = Color.black;
           cube.SetActive(false);
        
            
        }else if (state == StateEnum.ALIVE){
        GetComponent<SpriteRenderer>().color = Color.white;
        cube.SetActive(true);
        }
               }
               
            
            }
 
            
              
           