//Used as a storage container for a path that a person should move along
using UnityEngine;
using System;

public class MovementPath
{
    public bool isCenteredOnTile = false;
    private Vector2[] pathPoints;

    //Directions:
    // 0 = up, 1 = right, 2 = down, 3 = left
    private int[] directions;
    private float[] directionAngles;

    //Creates a new MovementPath Instance from an array of path points
    public MovementPath(Vector2[] _pathPoints, int[] _directions){

        //Saves the path points
        pathPoints = _pathPoints;

        directions = new int[_directions.Length + 1];

        //Makes last direction same as the previous one
        for(int i = 0; i < _directions.Length; i++){
            directions[i] = _directions[i];
        }
        if(_directions.Length >= 1){
            directions[directions.Length - 1] = _directions[_directions.Length - 1];
        }
        
        //Saves angle of direction between each path point
        directionAngles = new float[pathPoints.Length];
        if(directionAngles.Length > 0) directionAngles[0] = 90f;
        for(int i = 1; i < directionAngles.Length; i++){
            Vector2 previousPoint = pathPoints[i - 1];
            Vector2 nextPoint = pathPoints[i];

             float deltaX =  (nextPoint.x - previousPoint.x);
             float deltaY = (nextPoint.y - previousPoint.y);

             float radianConversionFactor = 180f / Mathf.PI;

            

            // float directionAngleRadians = (radianConversionFactor * Mathf.Atan(deltaY/deltaX));

            // if(deltaY < 0f){
            //     directionAngle = FlipAngleBy180(directionAngle);
            // }


            directionAngles[i] = (Vector2.SignedAngle(nextPoint - previousPoint, Vector2.up));
            Debug.Log("DX: " + deltaX + ", DY: " + deltaY + ", angle: " + directionAngles[i]);
        }

        // Saves the directions between path points
        // directions = new Vector2[pathPoints.Length];
        // for(int i = 0; i < directions.Length - 1; i++){
        //     directions[i] = GetDirectionBetweenPoints(pathPoints[i], pathPoints[i + 1]);
        // }
        // //Last direction is the same as the previous one
        // if(directions.Length >= 2){
        //     directions[directions.Length - 1] = directions[directions.Length - 2];  
        // }
        
    }

    public int GetPathLength(){
        return pathPoints.Length;
    }

    public Vector2 GetPoint(int indexOfPoint){
        if(isCenteredOnTile){
            //Vector2 centered2DCoords = new Vector2(pathPoints[indexOfPoint].x + 0.5f, pathPoints[indexOfPoint].y + 0.5f);
            return pathPoints[indexOfPoint];
        }else{
            return pathPoints[indexOfPoint];
        }
        
    }

    public int GetDirection(int indexOfPoint){
        return directions[indexOfPoint];
    }

    public float GetDirectionAngle(int indexOfPoint){
        return directionAngles[indexOfPoint];
    }

    //Calculates the direction between two points
    //Directions: (0, 1) = up, (1, 0) = right, (0, -1) = down, (-1, 0) = left
    // private Vector2 GetDirectionBetweenPoints(Vector2 pointOne, Vector2 pointTwo){
    //     if(pointOne.x > pointTwo.x && Mathf.Abs(pointOne.x - pointTwo.x) > .2f){
    //         return new Vector2(-1f, 0f);
    //     }
    //     else if(pointOne.x < pointTwo.x && Mathf.Abs(pointOne.x - pointTwo.x) > .2f){
    //         return new Vector2(1f, 0f);
    //     }
    //     else if(pointOne.y > pointTwo.y && Mathf.Abs(pointOne.y - pointTwo.y) > .2f){
    //         return new Vector2(0f, -1f);
    //     }
    //     else if(pointOne.y < pointTwo.y && Mathf.Abs(pointOne.y - pointTwo.y) > .2f){
    //         return new Vector2(0f, 1f);
    //     }
    //     else{
    //         return new Vector2(0f, 0f);
    //     }
    // }

    private float MirrorAngleAlongYAxis(float angleToFlip){
        if(angleToFlip >= 0f){
            return 90f + (90f - angleToFlip);
        } else{
            return -90f + (-90f - angleToFlip);
        }
    }

    private float FlipAngleBy180(float angleToFlip){
        if(angleToFlip >= 0f){
            return angleToFlip - 180f;
        } else{
            return angleToFlip + 180f;
        }
    }
}
