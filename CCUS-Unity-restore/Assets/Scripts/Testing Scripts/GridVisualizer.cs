using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    public GameObject cubePrefab;

    public void ActivateGridVisualization(){
        GridCell[] allGridCells = GridManager.GM.GetAllGridCells();
        foreach(GridCell gridCell in allGridCells){
            Instantiate(cubePrefab, new Vector3(gridCell.xLocation, 0f, gridCell.yLocation), Quaternion.identity);
        }
    }
}
