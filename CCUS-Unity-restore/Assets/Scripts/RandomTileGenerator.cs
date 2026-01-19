using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RandomTileGenerator : MonoBehaviour
{

    public static RandomTileGenerator current;

    void Start()
    {
        if(current == null){
            current = this;
        }
    }


    //Returns a random activated building
    public GameObject GetRandomActivatedBuilding(){

        //Gets list of all activatable buildings, like factories and residences
        //Basically, it gets the ones that need to be connected by roads
        Tile[] activatableBuildings = TileTypeCounter.current.ActivatableBuildingTileTracker.GetAllTiles();

        //Tracks all the buildings that are activated
        List<Tile> activatedBuildings = new List<Tile>();
        foreach(Tile activatableBuilding in activatableBuildings){
            //Checks if the tile is activated
            if(activatableBuilding is ActivatableTile activatableTile && activatableTile.IsActivated){
                activatedBuildings.Add(activatableBuilding);
            }
        }



        //Checks if there are any activated buildings
        if(activatedBuildings.Count > 0){
            //Returns random activated building
            System.Random randNumberGen = new System.Random();
            int randomBuildingIndex = randNumberGen.Next(0, activatedBuildings.Count);
            //Debug.Log("Activated Building coords: " + activatedBuildings[randomBuildingIndex].gameObject.transform.position);
            return activatedBuildings[randomBuildingIndex].gameObject;
        }else{
            return null; //if no activated buildings, return null
        }

    }


    //Returns a random activated road
    public GameObject GetRandomActivatedRoad(){

        //Gets a list of all roads
        Tile[] roads = TileTypeCounter.current.RoadTileTracker.GetAllTiles();

        //Tracks all the roads that are activaed
        List<Tile> activatedRoads = new List<Tile>();
        foreach(Tile road in roads){
            //Checks if the tile is activated
            if(road is ActivatableTile activatableTile && activatableTile.IsActivated){
                activatedRoads.Add(road);
            }
        }

        //Checks if there are any activated roads
        if(activatedRoads.Count > 0){
            //Returns random activated road
            System.Random randNumberGen = new System.Random();
            int randomRoadIndex = randNumberGen.Next(0, activatedRoads.Count);
            return activatedRoads[randomRoadIndex].gameObject;
        }else{
            return null; //if no activated roads, return null
        }

    }

}
