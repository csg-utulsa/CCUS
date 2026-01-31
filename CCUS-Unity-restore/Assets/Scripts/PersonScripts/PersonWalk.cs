//Controls where a person walks
using UnityEngine;

public class PersonWalk : MonoBehaviour
{
    public MovementPathMap.MapTileType Destination {get; set;}

    public bool PreventDestruction {get; set;} = true;

    public float personSpeed = 3f;
    
    public MoveObjectToPoint objectMover;

    private MovementPath walkingPath;
    
    private int currentPointOnPath = 0;

    public bool IsFrozen {get; set;} = false;

    void Awake(){
        objectMover = GetComponent<MoveObjectToPoint>();
        //Debug.Log("My new position: " + transform.position);



    }

    //Returns true if the person is currently on an activatable building
    public bool PersonIsOnActivatableBuilding(){
        GameObject[] GameObjectsInThisCell = GridManager.GM.GetGameObjectsInGridCell(transform.position);
        foreach(GameObject objectInThisCell in GameObjectsInThisCell){
            if(objectInThisCell.GetComponent<ActivatableBuilding>() != null){
                return true;
            }
        }
        return false;
    }


    

    public void RunAlongPath(MovementPath path){
        //If the path is 0 points long, doesn't do anything
        if(path.GetPathLength() <= 0){
            return;
        }

        walkingPath = path;

        currentPointOnPath = 0;

        MoveToPathPoint(0);

    }

    public void TeleportPersonToLocation(Vector3 teleportLocation){
        //Debug.Log("Teleporting to location" + teleportLocation);
        objectMover.TeleportTo(teleportLocation);
    }

    private void MoveToPathPoint(int indexOfPathPoint){

        //Doesn't move if the person is frozen
        if(IsFrozen) return;

        if(indexOfPathPoint >= walkingPath.GetPathLength()){
            Debug.LogError("walkingPath doesn't contain indexOfPathPoint");
            return;
        }

        //Turns model the correct direction
        TurnToDirection(walkingPath.GetDirectionAngle(indexOfPathPoint));
        Debug.Log("Instructed Angle: "+ walkingPath.GetDirectionAngle(indexOfPathPoint));


        objectMover.MoveTo(walkingPath.GetPoint(indexOfPathPoint), personSpeed, FinishedMovingToPoint);
    }

    public void FinishedMovingToPoint(){
        if(currentPointOnPath < walkingPath.GetPathLength() - 1){ //Not on last path point
            //moves to next path point
            currentPointOnPath++;
            MoveToPathPoint(currentPointOnPath);
        } else{ //On last path point
            //alerts people manager that this person finished its path
            PeopleMovementManager.current.PersonFinishedPath(this);
        }
    }

    //Turns person in the direction of travel
    private void TurnToDirection(float rotationalAngle){
        //float rotationalAngle = 90f * direction;
        transform.eulerAngles = new Vector3(0f, rotationalAngle, 0f);
    }
}
