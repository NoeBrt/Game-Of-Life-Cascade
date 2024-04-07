using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Cell : MonoBehaviour
{

    public StateEnum state = StateEnum.DEAD;
    public List<Cell> neighbors;
    public int[] position = new int[2];

    public GameObject cubePrefab;
    public GameObject cube;
    public StateEnum nextState = StateEnum.DEAD;

    public SpriteRenderer spriteRenderer;
    public CameraController cameraController;

    public Color defaultColor = Color.white;
    public Color mouseOverColor = new Color(1, 1, 1, 0.5f);

    public Color mouseOverCubeColor = new Color(0.95f, 0.95f, 0.95f, 1f);

    public int countAliveNeighbors;



    // Start is called before the first frame update
    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    public void UpdateCell()
    {


        UpdateNeighbors();
        UpdateCellState();


    }
    public void UpdateCubeRender()
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



    private void OnMouseOver()
    {
        // Early return if the camera is currently dragging
        if (CheckDrag()) return;

        // Set the sprite's color to indicate mouse over, but only if not dragging
        spriteRenderer.color = mouseOverColor;

        // If the cube is associated with an ALIVE state, change its color
        if (state == StateEnum.ALIVE && cube != null)
        {
            cube.GetComponent<Renderer>().material.color = mouseOverCubeColor;
        }

        // Handle click actions
        OnClick();
    }

    private bool CheckDrag()
    {
        if (cameraController.isDragging)
        {
            // Reset sprite and cube colors if dragging
            spriteRenderer.color = new Color(1, 1, 1, 0.0f); // Assuming this means "invisible" or "no change"
            if (cube != null) // Ensure cube is not null before accessing it
            {
                cube.GetComponent<Renderer>().material.color = defaultColor;
            }
            return true;
        }
        return false;
    }

    private void OnClick()
    {
        if (Input.GetMouseButton(0))
        {
            state = StateEnum.ALIVE;
        }
        else if (Input.GetMouseButton(1))
        {
            state = StateEnum.DEAD;
        }

    }




    private void OnMouseExit()
    {


        if (cube != null)
        {
            cube.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
        }

        // reset sprite opacity
        spriteRenderer.color = new Color(1, 1, 1, 0);
    }
    private void OnMouseEnter()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);

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


    void UpdateCellState()
    {
        int aliveNeighborsCount = neighbors.Count(neighbor => neighbor.state == StateEnum.ALIVE);
        nextState = (aliveNeighborsCount == 3 || (aliveNeighborsCount == 2 && state == StateEnum.ALIVE)) ? StateEnum.ALIVE : StateEnum.DEAD;
    }
    private void Update()
    {

        UpdateCubeRender();

    }


}



