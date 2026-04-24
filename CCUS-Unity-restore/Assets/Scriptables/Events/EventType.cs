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
        BeginDragPlaceTiles,
        EndDragPlaceTiles,
        PlaceGrass,
        PlaceTree,
        PlaceRoad,
        PlaceSmallBuilding,
        PlaceMediumBuilding,
        PlaceLargeBuilding,

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
    }
    // //Progress Events
    // public const string ProgressEventJustCalled = "progress_event_just_called";
    // public const string NewTileUnlocked = "new_tile_unlocked";
    
    // //People Events
    // public const string PersonJustAdded = "person_just_added";
    // public const string NumberOfPeopleChanged = "number_of_people_changed";

    // //UI Interaction Events
    // public const string MouseMovedToNewGridTile = "mouse_moved_to_new_grid_tile";
    // public const string ButtonHasBeenSelected = "button_has_been_selected";

    // //Money / Carbon Changed
    // public const string NetCarbonUpdated = "net_carbon_updated";
    // public const string NetMoneyUpdated  = "net_money_updated";
    // public const string MoneyAmountUpdated = "money_amount_updated";

    // //Chunk Loading events
    // public const string SwitchedCurrentGroundChunk = "switched_current_ground_chunk";
    // public const string SwitchedCurrentGroundChunkLate = "switched_current_ground_chunk_late";
    // public const string PurchasedCurrentGroundChunk = "purchased_current_ground_chunk";
    // public const string BeginSwitchingCurrentGroundChunk = "begin_switching_current_ground_chunk";
    // public const string BeginSwitchingCurrentGroundChunkLate = "begin_switching_current_ground_chunk_late";
    // public const string NewAreaUnlocked = "new_area_unlocked";

    // //Tile destruction and activation
    // public const string TileJustPlaced = "tile_just_placed";
    // public const string ActivatableTileJustPlaced = "activatable_tile_just_placed";
    // public const string ActivatableTileJustDestroyed = "activatable_tile_just_destroyed";
    // public const string TileJustDestroyed = "tile_just_destroyed";
    // public const string BuildingActivationStateChanged  = "building_activation_state_changed";
    // public const string NumOfCarbonCaptureTilesChanged = "num_of_carbon_capture_tiles_changed";
    // public const string NumOfWorkPlaceTilesChanged = "num_of_workplace_tiles_changed";
    // public const string FailedToPlaceTile = "failed_to_place_tile";

    // //UI Events. NOTE: Move to separate class eventually
    // public const string TileSelectPanelScrolled = "tile_select_panel_scrolled";
    // public const string TutorialTipHasAppeared = "tutorial_tip_has_appeared";
    // public const string CloseTutorialNotification = "close_tutorial_notification";
    // public const string CarbonMeterAppeared = "carbon_meter_appeared";
    // public const string PeopleButtonAppeared = "people_button_appeared";
    // public const string ClickTileButton = "click_tile_button";
    // public const string ClickSwitchAreaArrow = "click_switch_area_arrow";
    // public const string PeopleButtonPressed = "people_button_pressed_down";
    // public const string PeopleButtonHeldDown = "people_button_held_down";
    // public const string PeopleButtonReleased = "people_button_released";

    // //Error Events
    // public const string NotEnoughMoneyToPlace = "not_enough_money_to_place_tile";
    // public const string TooMuchCarbonToPlace = "too_much_carbon_to_place";
    // public const string FailToAddPeople = "fail_to_add_people";

    //Stores a list of all events
    // public static List<string> AllEvents = new List<string>{
    //     ProgressEventJustCalled,
    //     NewTileUnlocked,
    //     PersonJustAdded,
    //     NumberOfPeopleChanged,
    //     MouseMovedToNewGridTile,
    //     ButtonHasBeenSelected,
    //     NetCarbonUpdated,
    //     NetMoneyUpdated,
    //     MoneyAmountUpdated,
    //     SwitchedCurrentGroundChunk,
    //     SwitchedCurrentGroundChunkLate,
    //     PurchasedCurrentGroundChunk,
    //     BeginSwitchingCurrentGroundChunk,
    //     BeginSwitchingCurrentGroundChunkLate,
    //     NewAreaUnlocked,
    //     TileJustPlaced,
    //     ActivatableTileJustPlaced,
    //     ActivatableTileJustDestroyed,
    //     TileJustDestroyed,
    //     BuildingActivationStateChanged,
    //     NumOfCarbonCaptureTilesChanged,
    //     NumOfWorkPlaceTilesChanged,
    //     FailedToPlaceTile,
    //     TileSelectPanelScrolled,
    //     TutorialTipHasAppeared,
    //     CloseTutorialNotification,
    //     CarbonMeterAppeared,
    //     PeopleButtonAppeared,
    //     ClickTileButton,
    //     ClickSwitchAreaArrow,
    //     NotEnoughMoneyToPlace,
    //     TooMuchCarbonToPlace,
    //     FailToAddPeople,
    //     PeopleButtonPressed,
    //     PeopleButtonHeldDown,
    //     PeopleButtonReleased,

    // };
}
