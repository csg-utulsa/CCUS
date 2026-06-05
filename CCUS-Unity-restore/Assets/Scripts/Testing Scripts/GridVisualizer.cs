using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    public GameObject cubePrefab;

    public void ActivateGridVisualization(){
        GridCell[] allGridCells = GridManager.GM.GetAllGridCells();
        foreach(GridCell gridCell in allGridCells){
            Instantiate(cubePrefab, GridManager.GM.SwitchFromArrayToWorldCoordinates(new Vector2Int((int)gridCell.xArrayLocation, (int)gridCell.yArrayLocation)), Quaternion.identity);
        }
    }
}
