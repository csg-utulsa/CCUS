//Used as a storage container for a path that a person should move along
using UnityEngine;

public class MovementPath
{
    public bool isCenteredOnTile = false;
    private Vector2[] pathPoints;

    //Directions:
    // 0 = up, 1 = right, 2 = down, 3 = left
    private int[] directions;

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
        

        //Saves the directions between path points
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
}
