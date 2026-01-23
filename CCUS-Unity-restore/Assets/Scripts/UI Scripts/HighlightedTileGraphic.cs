using UnityEngine;

public class HighlightedTileGraphic : MonoBehaviour
{
    public GameObject activatedTileSquare;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If mouse is over the screen, it moves the highlighted graphic to it.
        if(MouseScreenPosition.MouseIsOverScreen()){
            activatedTileSquare.SetActive(true);
            Vector3 pos = BuildingSystem.GetMouseWorldPosition();
            Vector3 mouseWorldCoordinate = BuildingSystem.current.SnapCoordinateToGrid(pos);
            transform.position = new Vector3(mouseWorldCoordinate.x, transform.position.y, mouseWorldCoordinate.z);
        } else{
            activatedTileSquare.SetActive(false);
        }
    }

}
