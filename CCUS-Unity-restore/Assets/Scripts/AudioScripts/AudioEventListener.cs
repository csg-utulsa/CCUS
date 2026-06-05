//OBSOLETE
/*
*   Description: Links game events to audio events and plays sounds accordingly
*
*   To add a new sound effect, look at README.md in the Resources/Audio folder.
*
*
*/

using UnityEngine;

public class AudioEventListener : MonoBehaviour
{   

    // //The current AudioManager in the scene. Assign in Inspector.
    // AudioManager audioMan;

    // //Adds event listeners to call audio-event functions.
    // //Note: Can't be called on Awake, since that's when GameEvents are assigned
    // void Start(){
    //     audioMan = GetComponent<AudioManager>();

    //     //Switch Area
    //     GameEventManager.current.GetEvent(EventType.E.BeginSwitchingCurrentGroundChunk).AddListener(SwitchAreas);

    //     //Place Tile
    //     GameEventManager.current.GetEvent(EventType.E.TileJustPlaced).AddListener(TilePlaced);

    //     //Destroy Tile
    //     GameEventManager.current.GetEvent(EventType.E.TileJustDestroyed).AddListener(DestroyTile);

    //     //Buy Area
    //     GameEventManager.current.GetEvent(EventType.E.PurchasedCurrentGroundChunk).AddListener(PurchasedArea);

    //     //Notification Sound for when a tutorial tip appears
    //     GameEventManager.current.GetEvent(EventType.E.TutorialTipHasAppeared).AddListener(NotificationSound);

    //     //Person Added
    //     GameEventManager.current.GetEvent(EventType.E.PersonJustAdded).AddListener(PersonAdded);

    //     //Attempting to place tile, but can't
    //     GameEventManager.current.GetEvent(EventType.E.FailedToPlaceTile).AddListener(FailToPlaceTile);

    //     //Close a tutorial notification
    //     GameEventManager.current.GetEvent(EventType.E.CloseTutorialNotification).AddListener(CloseTutorialNotification);
    // }

    // private void SwitchAreas(){
    //     //Plays sound based on SoundID defined in Resources/Audio/AudioResourceMap
    //     audioMan.PlaySound(2);
    // }

    // private void TilePlaced(){
    //     audioMan.PlaySound(1);
    // }

    // private void PurchasedArea(){
    //     audioMan.PlaySound(3);
    // }

    // private void DestroyTile(){
    //     audioMan.PlaySound(6);
    // }

    // private void NotificationSound(){
    //     audioMan.PlaySound(8);
    // }

    // private void PersonAdded(){
    //     audioMan.PlaySound(7);
    // }

    // private void FailToPlaceTile(){
    //     audioMan.PlaySound(4);
    // }

    // private void CloseTutorialNotification(){
    //     audioMan.PlaySound(5);
    // }


}
