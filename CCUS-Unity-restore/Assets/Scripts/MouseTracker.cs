using UnityEngine;

public class MouseTracker : MonoBehaviour
{
    public Vector2Int PreviousGridPosition {get; set;} = new Vector2Int(0, 0);

    void Start()
    {
        //Sets the previous position to the mouse's starting position
        Vector3 pos = BuildingSystem.GetMouseWorldPosition();
        PreviousGridPosition = GridManager.GM.SwitchToGridCoordinates(pos);
    }

    void LateUpdate()
    {
        Vector3 pos = BuildingSystem.GetMouseWorldPosition();
        transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);
        Vector2Int currentGridPosition = GridManager.GM.SwitchToGridCoordinates(pos);
        if(PreviousGridPosition != currentGridPosition){
            //Debug.Log("Moving Tile From: " + PreviousGridPosition + " to " + currentGridPosition);
            MouseMovedToNewGridTile();
            PreviousGridPosition = currentGridPosition;
        }
    }

    private void MouseMovedToNewGridTile(){
        GameEventManager.current.MouseMovedToNewGridTile.Invoke();
    }
}
