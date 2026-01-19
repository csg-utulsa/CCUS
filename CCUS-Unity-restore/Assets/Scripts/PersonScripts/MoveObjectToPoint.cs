//Moves a GameObject to a given point

using UnityEngine;
using System;

public class MoveObjectToPoint : MonoBehaviour
{
    Vector3 targetLocation;
    Vector3 previousLocation;
    Vector3 currentLocation;
    float speed;
    Action FinishedMovingAction;

    private bool isMoving = false;
    float timer = 0f;
    float timeToMove = 1f;

    void Update()
    {
        if(isMoving){
            timer += Time.deltaTime;

            if(timer > timeToMove){ //Finished moving

                transform.position = targetLocation;
                previousLocation = targetLocation;
                currentLocation = targetLocation;

                timer = 0f;
                isMoving = false;

                FinishedMovingAction();
                
            } else{ //Still moving

                //Calculates what percentage the object should be moved
                float percentageMoved = timer / timeToMove;

                //Calculates current location of object
                currentLocation = SmoothMoveBetweenPoints(previousLocation, targetLocation, percentageMoved);

                //Moves object to correct position
                transform.position = currentLocation;

            }

        }
    }

    //Moves the object to which this script is attached to the target location at the given speed
    public void MoveTo(Vector2 _destination, float _speed, Action _FinishedMoving){

        //Saves the locations
        previousLocation = currentLocation;
        targetLocation = new Vector3(_destination.x, currentLocation.y, _destination.y);

        //Sets time it will take to move to location at given speed
        speed = _speed;
        timeToMove = Vector3.Distance(previousLocation, targetLocation) / speed; //Time = distance / speed
        
        //Stores the action to take when the object is done moving
        FinishedMovingAction = _FinishedMoving;

        //Switches to "isMoving" mode
        isMoving = true;
    }


    //Teleports object this script is attached to
    public void TeleportTo(Vector3 _destination){
        //Saves the locations
        targetLocation = new Vector3(_destination.x, currentLocation.y, _destination.z);
        previousLocation = targetLocation;
        currentLocation = targetLocation;
        transform.position = targetLocation;
    }


    //Returns point that is percentMoved between pointOne and pointTwo
    private Vector3 SmoothMoveBetweenPoints(Vector3 pointOne, Vector3 pointTwo, float percentMoved){
        float deltaX = pointTwo.x - pointOne.x;
        float deltaY = pointTwo.y - pointOne.y;
        float deltaZ = pointTwo.z - pointOne.z;

        float XValue = pointOne.x + (deltaX * percentMoved);
        float YValue = pointOne.y + (deltaY * percentMoved);
        float ZValue = pointOne.z + (deltaZ * percentMoved);

        Vector3 newLocation = new Vector3(XValue, YValue, ZValue);

        return newLocation;
    }

    
}
