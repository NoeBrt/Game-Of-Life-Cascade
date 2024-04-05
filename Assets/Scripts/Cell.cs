using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    public StateEnum state = StateEnum.DEAD;
    public List<Cell> neighbors;
    public int[] position = new int[2];

    public GameObject cubePrefab;
    public GameObject cube;

    public int count = 0;
    public StateEnum nextState = StateEnum.DEAD;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void UpdateCells()
    {


        UpdateNeighbors();
        ThreeNeighborRule();


    }
  
   
   
    private void OnMouseOver()
    {
        // improve sprite opacity
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);

        if (state == StateEnum.ALIVE)
        {
            if (cube != null)
            {
                cube.GetComponent<Renderer>().material.color = new Color(0.6f, 0.6f,0.6f, 0.95f);
                GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f,0.6f, 0.5f);


            }
        }
       

        if (Input.GetMouseButton(0) && state == StateEnum.DEAD)
        {
            state = StateEnum.ALIVE;
        }
        else if (Input.GetMouseButton(1) && state == StateEnum.ALIVE )
        {
            state = StateEnum.DEAD;
        }

    }


    private void OnMouseExit()
    {
        
        if (state == StateEnum.ALIVE)
        {
            if (cube != null)
            {
                cube.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
            }
        }
        // reset sprite opacity
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
    private void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);

    }


    void UpdateNeighbors()
    {
        neighbors = new List<Cell>();
        (int, int) dimGrid = (GridManager.instance.width, GridManager.instance.height);
        Cell[,] cells = GridManager.instance.Cells;

        // Parcourir les décalages relatifs pour les voisins
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                // Ignorer la position de la cellule elle-même (décalage 0,0)
                if (dx == 0 && dy == 0) continue;

                int neighborX = position[0] + dx;
                int neighborY = position[1] + dy;

                // Vérifier si la position est dans les limites de la grille
                if (neighborX >= 0 && neighborX < dimGrid.Item1 && neighborY >= 0 && neighborY < dimGrid.Item2)
                {
                    neighbors.Add(cells[neighborX, neighborY]);
                }
            }
        }
    }


    void ThreeNeighborRule()
    {
        Cell[,] cells = GridManager.instance.Cells;
        count = 0;
        foreach (Cell neighbor in neighbors)
        {
            if (neighbor.state == StateEnum.ALIVE)
            {
                count++;
            }
        }
        if (count == 3 && state == StateEnum.DEAD)
        {
            nextState = StateEnum.ALIVE;
        }

        else if ((count == 2 || count == 3) && state == StateEnum.ALIVE)
        {
            nextState = StateEnum.ALIVE;
        }
        else
        {
            nextState = StateEnum.DEAD;
        }


    }


    private void Update()
    {
        if (state == StateEnum.DEAD)
        {
            if (cube != null)
            {
                Destroy(cube);
            }
        }
        else if (state == StateEnum.ALIVE)
        {
            if (cube == null)
            {
                Vector3 cubePosition = new Vector3(transform.position.x, transform.position.y, 0.47f);
                cube = Instantiate(cubePrefab, cubePosition, Quaternion.identity);
            }

            cube.SetActive(true);



        }
    }


}



