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

    public Cell[,] Cells { get; set; }

    public bool isHistoryActive = false;

    public GameObject gridMesh;
    public float offset = -0.025f;
    public float historyBound = -55f;
    CameraController cameraController;
    void initGrid()
    {
        Cells = new Cell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject newCell = Instantiate(cellPrefab,
                new Vector3(x + (cellPrefab.transform.localScale.x / 2) - (width / 2) - offset, y + (cellPrefab.transform.localScale.y / 2) - (height / 2) - offset, 0),
                Quaternion.identity);
                Cell cell = newCell.GetComponent<Cell>();
                cell.position = new int[2] { x, y };
                Cells[x, y] = cell;
            }
        }
    }
    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();

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
                UpdateCellsAndHandleHistory();
                ApplyNextStatesToCells();
            }

            // Wait for deltaT seconds before the next update
            yield return new WaitForSeconds(deltaT);
        }
    }

    private void UpdateCellsAndHandleHistory()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cells[x, y].UpdateCell();
            }
        }
    }

    private void ApplyNextStatesToCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cells[x, y].state = Cells[x, y].nextState;
            }
        }
    }


    private void Update()
    {
        HandleKeyboardInput();
        UpdateCellHistoryVisibility();
    }

    private void HandleKeyboardInput()
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
            ResetCells();
        }
    }

    private void ResetCells()
    {
        foreach (var cell in Cells)
        {
            cell.state = StateEnum.DEAD;
            cell.nextState = StateEnum.DEAD;
            cell.cellHistory.ForEach(Destroy);
            cell.cellHistory.Clear();
        }
    }

    private void UpdateCellHistoryVisibility()
    {
        if (!cameraController.isProfile)
        {
            bool shouldShowCells = cameraController.startTransition && isHistoryActive;
            foreach (var cell in Cells)
            {
                cell.cellHistory.ForEach(previousCell => previousCell.SetActive(shouldShowCells));
            }
        }
    }





}