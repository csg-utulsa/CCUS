using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class MovementPathGenerator : MonoBehaviour
{
    public float shiftUpAmount = .75f;
    public float shiftDownAmount = .25f;
    //The bigger this number is, the less often movement paths are generated, which is more efficienct.
    //The smaller this number is, the more often the people will update where they think the roads are.
    public int LengthOfMovementPathToGenerate = 20;

    public MovementPathMap movementPathMap;

    public float percentChanceOfTurningOnNonRoadPath = .25f;

    private Vector2Int[] directions = new Vector2Int[]{
        new Vector2Int(0, 1), //North / up
        new Vector2Int(1, 0), //East / right
        new Vector2Int(0, -1), //South / down
        new Vector2Int(-1, 0), //West / left
    };

    private Vector3[] worldDirections = new Vector3[]{
        new Vector3(0f, 0f, 1f), //North / up
        new Vector3(1f, 0f, 0f), //East / right
        new Vector3(0f, 0f, -1f), //South / down
        new Vector3(-1f, 0f, 0f), //West / left
    };

    void Start(){

    }

    //Checks if point is on road
    public bool PointIsOnPath(Vector3 pointToCheck){
        return true;
    }


    //Makes a random path along the roads. Ends path if person goes into a building
    public MovementPath MakeRandomPathAlongRoads(Vector3 worldStartLocation){

        //Makes a list to store directions in
        List<int> directions = new List<int>();

        //Vector2Int gridStartLocation = GridManager.GM.SwitchToGridCoordinates(worldStartLocation);
        List<Vector2> pathPoints = new List<Vector2>();

        Vector3 currentWorldLocation = worldStartLocation;

        MovementPathMap.MapTileType[] neighbors;

        //Generates random direction to start path from
        System.Random randNumberGen = new System.Random();
        int currentDirection = randNumberGen.Next(0, 4);

        bool travelingPositively = (currentDirection == 0 || currentDirection == 3);

        //Adds each point on path one by one
        for(int i = 0; i < LengthOfMovementPathToGenerate; i++){
            
            //Gets neighbors for current point
            neighbors = movementPathMap.GetNeighborsForPoint(currentWorldLocation);

            

            //Makes sure that the tile has neighbors
            if(neighbors.Length > 0){
                
                //Decides which direction to go next and stores it
                int nextRoadTileDirection = PickNextRoadTile(neighbors, currentDirection, i);

                //Debug.Log("Current Direction: " + currentDirection);

                //If this tile has nowhere to go, it stops moving
                if(nextRoadTileDirection == -1){
                    break;
                }
                currentDirection = nextRoadTileDirection;

                //Moves the world location to the new position and stores it in the pathPoints array
                currentWorldLocation += worldDirections[nextRoadTileDirection];
                Vector2Int currentGridLocation = GridManager.GM.SwitchToGridCoordinates(currentWorldLocation);
                Vector2 shiftedLocation;

                
                if(currentDirection == 0){ //Shifts location to right of cell if traveling up
                    shiftedLocation = ShiftPointToRightOfCell((Vector2)currentGridLocation);

                } else if(currentDirection == 1){ //Shifts location to bottom of cell if traveling right

                    shiftedLocation = ShiftPointToBottomOfCell((Vector2)currentGridLocation);
                    
                } else if(currentDirection == 2){ //Shifts location to left of cell if travelling down

                    shiftedLocation = ShiftPointToLeftOfCell((Vector2)currentGridLocation);

                } else{ //Shifts location to top of cell if travelling left

                    shiftedLocation = ShiftPointToTopOfCell((Vector2)currentGridLocation);
                }

                //if this tile is a building, it stops adding points
                if(movementPathMap.GetTileForPoint(currentWorldLocation) == MovementPathMap.MapTileType.Building){
                    
                    //Shifts last point to cell center
                    AddGridPointToPathPoints(pathPoints, ShiftPointToCellCenter((Vector2)currentGridLocation));
                    //Stores current direction in array
                    directions.Add(currentDirection);
                    
                    break;

                } else{

                    AddGridPointToPathPoints(pathPoints, shiftedLocation);
                    //Stores current direction in array
                    directions.Add(currentDirection);
                }

                
                

            }

            
            
        }

        //returns the created path
        MovementPath roadPath = new MovementPath(pathPoints.ToArray(), directions.ToArray());
        roadPath.isCenteredOnTile = true;
        return roadPath;
    }

    //Makes a random path between two points, ignoring all placed tiles. Used when people spawn in over the world.
    public MovementPath MakeDirectPath(Vector3 worldStartLocation, Vector3 worldEndLocation){

        //Makes list to store path points in
        List<Vector2> pathPoints = new List<Vector2>();
        
        //Makes a list to store directions in
        List<int> directions = new List<int>();

        //Switches start and end location to grid coordinates
        Vector2Int gridStartLocation = GridManager.GM.SwitchToGridCoordinates(worldStartLocation);
        Vector2Int gridEndLocation = GridManager.GM.SwitchToGridCoordinates(worldEndLocation);

        AddGridPointToPathPoints(pathPoints, gridStartLocation);

        //Calculates the number of moves that will need to be made
        int deltaX = gridEndLocation.x - gridStartLocation.x;
        int deltaY = gridEndLocation.y - gridStartLocation.y;
        int numberOfMoves = Math.Abs(deltaX) + Math.Abs(deltaY);

        //Stores direction of movement for x and y
        Vector2Int directionX = new Vector2Int(Math.Sign(deltaX), 0);
        Vector2Int directionY = new Vector2Int(0, Math.Sign(deltaY));

        //Stores current path point position
        Vector2Int currentPathPosition = gridStartLocation;

        //Creates random number generator
        System.Random randNumberGen = new System.Random();

        //Randomly decides whether it starts off moving vertically or horizontally
        bool movingVertically = false;
        if(randNumberGen.Next(0, 2) == 0){
            movingVertically = true;
        }

        //defines the chance of turning at each tile
        float[] chancesOfTurning = new float[]{1f-percentChanceOfTurningOnNonRoadPath, percentChanceOfTurningOnNonRoadPath};

        //Adds points to path, moving one tile at a time
        for(int i = 0; i < numberOfMoves; i++){

            //Randomly decides if path should turn
            if(RandomlySelectOption(chancesOfTurning) == 0){
                //Switches direction of movement
                movingVertically = !movingVertically;
            }


            //Calculates and stores current direction to array
            int direction;
            if(movingVertically){
                if(directionY.y > 0){
                    direction = 0;
                }else{
                    direction = 2;
                }
            }else{
                if(directionX.x > 0){
                    direction = 1;
                }else{
                    direction = 3;
                }
            }
            directions.Add(direction);


            if(currentPathPosition.x == gridEndLocation.x){ //path is already at the max value for x
                //moves path in y direction
                currentPathPosition += directionY;
                //AddGridPointToPathPoints(pathPoints, ShiftPointToCellCenter((Vector2)currentPathPosition));

            } else if(currentPathPosition.y == gridEndLocation.y){ //path is already at the max value for y
                //moves path in x direction
                currentPathPosition += directionX;
                //AddGridPointToPathPoints(pathPoints, ShiftPointToCellCenter((Vector2)currentPathPosition));

            } else if(movingVertically){ //path is traveling vertically
                //moves path in y direction
                currentPathPosition += directionY;
                //AddGridPointToPathPoints(pathPoints, ShiftPointToCellCenter((Vector2)currentPathPosition));

            } else{ //path is traveling horizontally
                //moves path in x direction
                currentPathPosition += directionX;
                
            }

            //If not on the last move, doesn't center the movement path on the tile
            if(i != (numberOfMoves - 1)){
                AddGridPointToPathPoints(pathPoints, (Vector2)currentPathPosition);
            }
            //If on the last move, shifts the last point to the center of the tile
            else{
                AddGridPointToPathPoints(pathPoints, ShiftPointToCellCenter((Vector2)currentPathPosition));
            }
            

        }

        MovementPath directMovementPath = new MovementPath(pathPoints.ToArray(), directions.ToArray());
        directMovementPath.isCenteredOnTile = true;
        return directMovementPath;
    }

    //Shifts points by grid manager center
    private void AddGridPointToPathPoints(List<Vector2> pathPoints, Vector2 gridPoint){
        Vector3 adjustedPosition = GridManager.GM.AdjustCoordinatesByGridCenter(gridPoint);
        Vector2 adjustedPosition2D = new Vector2(adjustedPosition.x, adjustedPosition.z);
        pathPoints.Add(adjustedPosition2D);
    }

    //Moves Grid Points to the center of the cell
    private Vector2 ShiftPointToCellCenter(Vector2 UnshiftedPoint){
        Vector2 centered2DCoords = new Vector2(UnshiftedPoint.x + 0.5f, UnshiftedPoint.y + 0.5f);
        return centered2DCoords;
    }

    //Moves Grid Point to different parts of the cell
    private Vector2 ShiftPointToRightOfCell(Vector2 UnshiftedPoint){
        Vector2 rightCoords = new Vector2(UnshiftedPoint.x + shiftUpAmount, UnshiftedPoint.y);
        return rightCoords;
    }
    private Vector2 ShiftPointToLeftOfCell(Vector2 UnshiftedPoint){
        Vector2 leftCoords = new Vector2(UnshiftedPoint.x + shiftDownAmount, UnshiftedPoint.y);
        return leftCoords;
    }
    private Vector2 ShiftPointToTopOfCell(Vector2 UnshiftedPoint){
        Vector2 topCoords = new Vector2(UnshiftedPoint.x, UnshiftedPoint.y + shiftUpAmount);
        return topCoords;
    }
    private Vector2 ShiftPointToBottomOfCell(Vector2 UnshiftedPoint){
        Vector2 bottomCoords = new Vector2(UnshiftedPoint.x, UnshiftedPoint.y + shiftDownAmount);
        return bottomCoords;
    }



    //Returns an option, with each option i having a percentChances[i] chance of being chosen
    private int RandomlySelectOption(float[] percentChances){

        System.Random randNumberGen = new System.Random();
        float randomPercent = (float)randNumberGen.Next(0, 101) / 100f;
        
        float counter = 0f;
        for(int i = 0; i < percentChances.Length; i++){
            counter += percentChances[i];
            if(randomPercent < counter){
                //Debug.Log("Selected turn " + i);
                return i;
            }
        }

        //Returns -1 if other options not selected
        return -1;

    }

    


    //Decides which tile a path should go down
    //TODO: make it more likely to decide to go to houses
    private int PickNextRoadTile(MovementPathMap.MapTileType[] tileOptions, int currentDirection, int pointAlongPath){

        int chanceOfTurningAround = 1;
        int chanceOfTurningToSide = 12;
        int chanceOfGoingForward = 25;

        // Index 0 is up, Index 1 is right, Index 2 is down, Index 3 is left
        int[] chancesOfEachDirection = new int[]{
            chanceOfGoingForward,
            chanceOfTurningToSide,
            chanceOfTurningAround,
            chanceOfTurningToSide
        };

        //Reorients the chancesOfEachDirection array in whatever direction the path is currently going
        int[] reorientedChancesOfTurning = ReorientDirectionArray(chancesOfEachDirection, currentDirection);

        int[] chancesOfTurning = new int[4];

        for(int i = 0; i < tileOptions.Length; i++){
            //if tile isn't empty, it adds the chances of turning that way, unless it's the first move and there's a house there
            bool isFirstMoveAndOptionIsHouse = pointAlongPath == 0 && tileOptions[i] == MovementPathMap.MapTileType.Building;
            if(tileOptions[i] != MovementPathMap.MapTileType.Empty && !isFirstMoveAndOptionIsHouse){
                chancesOfTurning[i] = reorientedChancesOfTurning[i];
            }else{ //tile is empty, and so it has no chance of going that way
                chancesOfTurning[i] = 0;
            }
        }


        //Adjust array so that all elements sum to 1
        float[] chancesOfTurningAsPercent = new float[chancesOfTurning.Length];
        float totalChanceNum = (float)chancesOfTurning.Sum();
        if(totalChanceNum == 0){
            for(int i = 0; i < chancesOfTurning.Length; i++){
                chancesOfTurningAsPercent[i] = 0f;
            }
        }else{
            for(int i = 0; i < chancesOfTurning.Length; i++){
                chancesOfTurningAsPercent[i] = (float)chancesOfTurning[i] / totalChanceNum;
            }
        }

        //Debug.Log("Chances of each turn: \n" + "0: " + chancesOfTurning[0] + "\n1: " + chancesOfTurning[1] + "\n2: " + chancesOfTurning[2] + "\n3: " + chancesOfTurning[3]);

        
        int decision = RandomlySelectOption(chancesOfTurningAsPercent);
        return decision;
    }

    //Reorients direction array towards a specific direction
    private int[] ReorientDirectionArray(int[] directions, int orientation){
        //Debug.Log("old directions: \n0: " + directions[0] + "\n1: " + directions[1] + "\n2:" + directions[2] + "\n3:" + directions[3]);
        int[] newDirections = new int[4];

        for(int i = 0; i < directions.Length; i++){
            newDirections[((i + orientation) % 4)] = directions[i];
        }
        //Debug.Log("NEW directions oriented at " + orientation + ": \n0: " + newDirections[0] + "\n1: " + newDirections[1] + "\n2:" + newDirections[2] + "\n3:" + newDirections[3]);
        
        return newDirections;
    }
}
