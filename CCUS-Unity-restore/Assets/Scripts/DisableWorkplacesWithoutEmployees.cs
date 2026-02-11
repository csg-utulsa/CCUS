using UnityEngine;

public class DisableWorkplacesWithoutEmployees : MonoBehaviour
{
    // private List<WorkPlace> allWorkPlaces = new List<WorkPlace>();
    // private int numberOfWorkPlaces;
    // private int numberOfNeededEmployees;
    // private int numberOfWorkPlacesOnActiveTile;

    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     GameEventManager.current.NumberOfPeopleChanged.AddListener(NumberOfPeopleChanged);
    //     GameEventManager.current.NumOfWorkPlaceTilesChanged.AddListener(NumOfWorkPlaceTilesChanged);
    // }

    // //Runs every time the number of people is changed
    // private void NumberOfPeopleChanged(){
    //     UpdateEnabledWorkplaces();
    // }

    // //Runs ever time a workplace tile is placed or destroyed
    // private void NumOfWorkPlaceTilesChanged(){
    //     UpdateListOfWorkPlaces();
    //     UpdateEnabledWorkplaces();
    // }       

    // private void UpdateListOfWorkPlaces(){

    // }

    // //Turns off excess workplaces
    // private void UpdateEnabledWorkplaces(){

    //     //Enough employees for all workplaces
    //     if(numberOfNeededEmployees){
    //         foreach(WorkPlace workPlace in allWorkPlaces){
    //             workPlaceTile = workPlace.GetTileReference();
    //             if(workPlaceTile != null){
    //                 workPlaceTile.SetIsEnoughResourcesToFunction(true);
    //             }
    //         }
    //     }

    //     //Enough employees for all workplaces
    // }

    // private void AddWorkPlaceToList(WorkPlace workPlace){
    //     allWorkPlaces.Add(workPlace);
    //     numberOfWorkPlaces++;
    //     numberOfNeededEmployees += workPlace.employeesRequired;
    // }

    // private void RemoveWorkPlaceFromList(WorkPlace workPlace){
    //     allWorkPlaces.Remove(workPlace);
    //     numberOfWorkPlaces--;
    //     numberOfNeededEmployees -= workPlace.employeesRequired;
    // }

}

// public class WorkPlace {
//     public int areaPlacedOn;
//     public int employeesRequired;
//     public int moneyPerHour;
//     public int carbonPerHour;
//     private Tile tileReference;
//     public bool isEnabled;
//     public Vector2Int GridPosition;

//     public bool IsCurrentlyVisible(){
//         if(GroundAreaExpansion.current != null){
//             if(areaPlacedOn == GroundAreaExpansion.current.ActiveGroundChunk){
//                 return true;
//             }
//         }
//         return false;
//     }

//     public Tile GetTileReference(){
//         if(IsCurrentlyVisible){
//             GameObject[] GameObjectsOnThisTile = GridManager.current.GetGameObjectsInGridCell(GridPosition);
//             foreach(GameObjectsOnThisTile){
//                 Tile tile = GameObjectsOnThisTile.GetComponent<Tile>();
//                 if(tile != null && tile.tileScriptableObject.RequiredEmployees == employeesRequired){
//                     return tile;
//                 }
//             }
//         } else{
//             return null;
//         }
//     }
// }
