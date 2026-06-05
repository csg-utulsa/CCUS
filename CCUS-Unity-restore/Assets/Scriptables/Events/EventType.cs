/*
*   To add a new event:
*      1) Add an entry to the enum E
*      2) Go to the place you want it to be invoked and add a line like this:
*           GameEventManager.current.GetEvent(EventType.E.MyCoolNewEvent).Invoke();
*      3) To listen to the event use a line like this:
*           GameEventManager.current.GetEvent(EventType.E.MyCoolNewEvent).AddListener(MyNewTerrifyingFunction);
*
*/



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class EventType
{
    public enum E {
        ///<b>WARNING!!! DO NOT MOVE THE NEXT SIX EVENTS IN THE ENUM, OR PUT EVENTS ABOVE THEM!!!!!!!!!!</b>
        //The Unity Editor links Enums to index integers when you select them in the Inspector.
        //That means whenever the number tied to an enum changes, which enum an integer represents changes.
        //The result is that altering the position of the next six enums will change what events the tiles
        //call, since the events they call are selected in the editor through their scriptable.
        PlaceGrass,
        PlaceTree,
        PlaceRoad,
        PlaceSmallBuilding,
        PlaceMediumBuilding,
        PlaceLargeBuilding,
        PlaceSapling,


        //Progress Events
        ProgressEventJustCalled,
        NewTileUnlocked,
        
        //People Events
        PersonJustAdded,
        NumberOfPeopleChanged,

        //UI Interaction Events
        MouseMovedToNewGridTile,
        ButtonHasBeenSelected,

        //Money / Carbon Changed
        NetCarbonUpdated,
        NetMoneyUpdated,
        MoneyAmountUpdated,
        MaxCarbonAmountHit,
        NewMoney,

        //Chunk Loading events
        SwitchedCurrentGroundChunk,
        SwitchedCurrentGroundChunkLate,
        PurchasedCurrentGroundChunk,
        BeginSwitchingCurrentGroundChunk,
        BeginSwitchingCurrentGroundChunkLate,
        NewAreaUnlocked,
        SwitchedChunkLeft,
        SwitchedChunkRight,
        BeginSwitchingChunkLeft,
        BeginSwitchingChunkRight,

        //Tile destruction and activation
        TileJustPlaced,
        TilePlacedWithoutDrag,
        ActivatableTileJustPlaced,
        ActivatableTileJustDestroyed,
        TileJustDestroyed,
        BuildingActivationStateChanged,
        NumOfCarbonCaptureTilesChanged,
        NumOfWorkPlaceTilesChanged,
        FailedToPlaceTile,
        FailedToPlaceTileWithoutDrag,
        BeginDragPlaceTiles,
        EndDragPlaceTiles,
        DeleteTileWithoutDrag,

        //UI Events. NOTE: Move to separate class eventually
        TileSelectPanelScrolled,
        TutorialTipHasAppeared,
        CloseTutorialNotification,
        CarbonMeterAppeared,
        PeopleButtonAppeared,
        ClickTileButton,
        ClickSwitchAreaArrow,
        PeopleButtonPressed,
        PeopleButtonHeldDown,
        PeopleButtonReleased,
        BeginFillingPeopleButton,
        FinishFillingPeopleButton,
        SelectedTileButton,
        DeselectedTile,
        SelectedTrashButton,
        DeselectedTrashButton,

        //Error Events
        NotEnoughMoneyToPlace,
        TooMuchCarbonToPlace,
        FailToAddPeople,

        //Called when cheats are enabled
        CheatsEnabled,
    }
}
