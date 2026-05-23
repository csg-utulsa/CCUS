using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float cameraMoveTime = .5f;
    private bool isMoving;
    private float moveTimer = 0f;
    private Vector3 moveDestination;
    private Vector3 moveOrigin;

    public void MoveCamera(Vector3 _moveDestination, float _cameraMoveTime){
        cameraMoveTime = _cameraMoveTime;
        isMoving = true;
        moveTimer = 0f;
        moveDestination = _moveDestination;
        moveOrigin = transform.position;
    }


    void Update(){
        if(isMoving){
            
            moveTimer += Time.deltaTime;
            float percentageMoved = moveTimer / cameraMoveTime;
            if(percentageMoved >= 1){
                isMoving = false;
                transform.position = moveDestination;
                moveTimer = 0;
            } else{
                transform.position = movePercentageBetweenPoints(moveOrigin, moveDestination, percentageMoved);
            }
        }
    }

    private Vector3 movePercentageBetweenPoints(Vector3 pointOne, Vector3 pointTwo, float percentage){
        float deltaX = pointTwo.x - pointOne.x;
        float deltaY = pointTwo.y - pointOne.y;
        float deltaZ = pointTwo.z - pointOne.z;

        float newXLocation = pointOne.x + (deltaX * percentage);
        float newYLocation = pointOne.y + (deltaY * percentage);
        float newZLocation = pointOne.z + (deltaZ * percentage);

        return new Vector3(newXLocation, newYLocation, newZLocation);
    }
}
