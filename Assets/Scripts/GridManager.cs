using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    [SerializeField]
   public  int width = 10;
    [SerializeField]
   public int height = 10;

    [SerializeField]
    GameObject cellPrefab;

    [SerializeField]
    float deltaT = 0.2f;
    
    bool isPause=false;

   public Cell[,] Cells {get; set;}

    


    void initGrid(){
        Cells=  new Cell[width,height];
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                GameObject newCell = Instantiate(cellPrefab, new Vector3(x + cellPrefab.transform.localScale.x / 2 - width / 2, y + cellPrefab.transform.localScale.y / 2 - height / 2,0), Quaternion.identity);
                Cell cell = newCell.GetComponent<Cell>();
                cell.position = new int[2]{x,y};
                Cells[x,y] = cell;
            }
        }
    }
    void Start()
    {

    initGrid();
    StartCoroutine(UpdateCellsCoroutine());
    }

    

    private void Awake() {
        if (instance == null){
            instance = this;
        }else{
            Destroy(this);
        }
    }

    // Update is called once per frame
    IEnumerator UpdateCellsCoroutine()
    {
        while(true){

        if (!isPause){
         for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){

                Cells[x,y].UpdateCells();
            } }

             for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){

                Cells[x,y].state= Cells[x,y].nextState;
            } }}
        yield return new WaitForSeconds(deltaT); // Attendre 0.5 seconde avant la prochaine mise à jour
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)){
            isPause=!isPause;
        }
    }

}
