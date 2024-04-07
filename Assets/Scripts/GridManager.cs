using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    [SerializeField]
    public int width = 10;
    [SerializeField]
    public int height = 10;

    [SerializeField]
    GameObject cellPrefab;

    [SerializeField]
    float deltaT = 0.2f;

    public bool isPause = true;
   public List<GameObject> CellHistory = new List<GameObject>();

    public Cell[,] Cells { get; set; }

    public GameObject cubePrefab;

    bool isHistoryActive = false;

    public GameObject gridMesh;
    public float offset = -0.025f;

    void initGrid()
    {
        Cells = new Cell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject newCell = Instantiate(cellPrefab,
                new Vector3(x + (cellPrefab.transform.localScale.x / 2) - (width / 2)-offset, y + (cellPrefab.transform.localScale.y / 2) - (height / 2)-offset, 0),
                Quaternion.identity);
                Cell cell = newCell.GetComponent<Cell>();
                cell.position = new int[2] { x, y };
                Cells[x, y] = cell;
            }
        }
    }
    void Start()
    {

        initGrid();
        StartCoroutine(UpdateCellsCoroutine());
    }



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    IEnumerator UpdateCellsCoroutine()
    {
        while (true)
        {
            if (!isPause)
            {
                for (int i = 0; i < CellHistory.Count; i++)
                {
                    GameObject cell = CellHistory[i];
                    cell.transform.position = cell.transform.position - new Vector3(0, 0, 1);
                    if (cell.transform.position.z < -55)
                    {
                        CellHistory.Remove(cell);
                        Destroy(cell);
                    }
                }
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Cells[x, y].UpdateCell();
                        if (Cells[x, y].state == StateEnum.ALIVE)
                        {
                            if (isHistoryActive){
                            GameObject previousCell = Instantiate(cubePrefab, Cells[x, y].cube.transform.position - new Vector3(0, 0, 1f), Quaternion.identity);
                            CellHistory.Add(previousCell);}
                        }

                    }
                }

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Cells[x, y].state = Cells[x, y].nextState;
                    }
                }
            }
            yield return new WaitForSeconds(deltaT); // Attendre deltaT seconde avant la prochaine mise à jour
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPause = !isPause;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            isHistoryActive = !isHistoryActive;

        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            gridMesh.SetActive(!gridMesh.activeSelf);

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            CellHistory.ForEach(cell => Destroy(cell));
            CellHistory.Clear();
            foreach (Cell cell in Cells)
            {
                cell.state = StateEnum.DEAD;
                cell.nextState = StateEnum.DEAD;
            }
        }
        if(!Camera.main.GetComponent<CameraController>().isProfile && !Camera.main.GetComponent<CameraController>().startTransition )
        {
            CellHistory.ForEach(cell => cell.SetActive(false));
        }
        if (!Camera.main.GetComponent<CameraController>().isProfile && Camera.main.GetComponent<CameraController>().startTransition)
        {
            CellHistory.ForEach(cell => cell.SetActive(isHistoryActive));
        }


    }

}