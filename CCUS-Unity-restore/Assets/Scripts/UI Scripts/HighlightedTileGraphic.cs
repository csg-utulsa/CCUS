using UnityEngine;

public class HighlightedTileGraphic : MonoBehaviour
{
    public GameObject activatedTileSquare;
    public Color validColor;
    public Color invalidColor;
    public SpriteRenderer HighlightedGraphicImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HighlightedGraphicImage = GetComponentInChildren<SpriteRenderer>(true);

        GameEventManager.current.MouseMovedToNewGridTile.AddListener(MouseMovedToNewGridTile);
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

    private void MouseMovedToNewGridTile(){
        if(ShouldSelectedTileGraphicBeGreen()){
            SetGreenState();
        } else{
            SetRedState();
        }
    }

    //Checks if the selected tile graphic (the square on the tile under the mouse) should be red.
    // Turns the selected tile graphic red whenever the active object is red, except over the void.
    public bool ShouldSelectedTileGraphicBeGreen(){
        if(BuildingSystem.isObjectOverVoid()) return false;
        return BuildingSystem.current.ShouldActiveTileMaterialBeValid();
    }

    //Turns selected tile graphic (the square under the tile the mouse is hovering on) red when object is unplaceable.
    private void SetRedState(){
        HighlightedGraphicImage.color = invalidColor;
    }

    //Turns selected tile graphic (the square under the tile the mouse is hovering on) green when object is placeable.
    private void SetGreenState(){
        HighlightedGraphicImage.color = validColor;
    }

}
