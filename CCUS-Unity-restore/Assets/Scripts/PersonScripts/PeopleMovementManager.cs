using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class PeopleMovementManager : MonoBehaviour
{
    public static PeopleMovementManager current;
    public GameObject personPrefab;
    public Vector3 peopleSpawnPoint = new Vector3(0f, 0f, 0f);
    public MovementPathGenerator pathGenerator;

    //Keeps a list of the people that shouldn't be destroyed when they enter a house (the indestructible people)
    //Basically, it's all the people that aren't spawned in when the "Add Person" button is pressed
    public List<PersonWalk> allIndestructiblePeople = new List<PersonWalk>();

    public List<PersonWalk> allPeopleOnCurrentGridChunk = new List<PersonWalk>();

    //Returns the number of people that should be wandering between houses along roads at any moment
    private int CorrectNumberOfIndestructiblePeople{
        get{

            int numberOfBuildings = TileTypeCounter.current.GetAllActivatedBuildings().Length;
            int maxNumOfPeople;


            if(numberOfBuildings <= 7){
                //Max, there should be as many people as activated buildings
                maxNumOfPeople = numberOfBuildings;
                
            } else{
                
                //If there's more than 7 buildings, the max number of people increases more slowly than the number of buildings
                maxNumOfPeople = 7 + (int)(.3f * (numberOfBuildings - 7));
            }

            
            float percentOfMaxPeopleAdded = ((float)PeopleManager.current.NumberOfPeople / (float)PeopleManager.current.maxPeople);
            float correctNumOfPeople = (float)maxNumOfPeople * percentOfMaxPeopleAdded;
            //Debug.Log("Correct num of people: " + (int)Mathf.Round(correctNumOfPeople));
            return (int)Mathf.Round(correctNumOfPeople);
        }
    }

    void Start(){
        //GameEventManager.current.BeginSwitchingCurrentGroundChunk.AddListener(SwitchedChunk);
        GameEventManager.current.BuildingActivationStateChanged.AddListener(BuildingActivationChanged);
        //GameEventManager.current.PersonJustAdded.AddListener(NewPersonAdded);
        if(current == null){
            current = this;
        }

        //Sets the person prefab position
        peopleSpawnPoint = GridManager.GM.GetCenter();
    }


    //Returns true if there are too many indestructible people
    public bool TooManyIndestructiblePeople(){
        if(CorrectNumberOfIndestructiblePeople < allIndestructiblePeople.Count){
            return true;
        }else{
            return false;
        }
    }


    public void SetSpawnPoint(Vector3 newSpawnPoint){
        peopleSpawnPoint = newSpawnPoint;
    }


    //Updates the number of indestructible people when the number of activatable buildings changes
    // private void ActivatableBuildingCountChanged(){
    //     UpdateIndestructiblePeopleCount();
    // }

    public void BuildingActivationChanged(){
        UpdateIndestructiblePeopleCount(true);
    }

    //Adds the correct number of new people
    public void UpdateIndestructiblePeopleCount(bool createPeopleInHouses){
        int previousIndestructiblePeopleCount = allIndestructiblePeople.Count;
        int maxTimesToRunFailsafe = 0;

        while(allIndestructiblePeople.Count < CorrectNumberOfIndestructiblePeople && maxTimesToRunFailsafe < 100){
            CreatePersonAtRandomLocation(createPeopleInHouses);

            //If it fails to increment the number of indestructible people, it breaks
            if(previousIndestructiblePeopleCount == allIndestructiblePeople.Count){
                break;
            }

            //Increments the max times to run failsafe
            maxTimesToRunFailsafe++;
        }
    }
    

    public void PersonFinishedPath(PersonWalk person){

        //If person finishes path on a building, teleports them to a different building.
        //Unless there's too many people or no activated buildings. In that case, it destroys them
        bool personIsOnBuilding = person.PersonIsOnActivatableBuilding();

        if(personIsOnBuilding){ //Person finished path on building
            GameObject randomBuilding = RandomTileGenerator.current.GetRandomActivatedBuilding();
            if(randomBuilding != null){
                //Debug.Log("finished path on building");
                
                //Checks if person should be destroyed;
                if(!ShouldDestroyPerson(person)){
                    person.TeleportPersonToLocation(RandomTileGenerator.current.GetRandomActivatedBuilding().transform.position); 
                    SendPersonAlongRoadPath(person);
                } else{
                    DestroyPerson(person);

                    //Checks if new person should be created after destroying person
                    UpdateIndestructiblePeopleCount(true);
                }

                
            } else{
                DestroyPerson(person);
            }
        
           //If person finishes their path on a road, it gives them a new path along the road
        } else if(pathGenerator.PointIsOnPath(person.gameObject.transform.position)) { 
            Debug.Log("finished path on road");
            SendPersonAlongRoadPath(person);
            //MovementPath randomRoadPath = pathGenerator.MakeRandomPathAlongRoads(person.gameObject.transform.position);
            //If their's no connected roads
            // if(randomRoadPath.GetPathLength <= 0){
            //     DestroyPerson(person);
            // }
            //person.RunAlongPath(randomRoadPath);

           //If person finishes path somewhere other than a road or building, it gives them a path back to an activated tile
        } else { 
            SendPersonToActivatedRoad(person);
            // GameObject randomRoad = RandomTileGenerator.current.GetRandomActivatedRoad();
            // if(randomRoad != null){
            //     MovementPath randomRoadPath = MovementPathGenerator.current.MakeDirectPath(person.gameObject.transform.position, randomRoad.transform.position);
            //     person.RunAlongPath(randomRoadPath);
            // }else{
            //     DestroyPerson(person);
            // }
            
        }
 


    }

    //Sends people to random activated building when they're created
    private void SendPersonToActivatedBuilding(PersonWalk person){
        GameObject randomBuilding = RandomTileGenerator.current.GetRandomActivatedBuilding();

        //If there are any activated buildings, sends the person to one of them
        if(randomBuilding != null){
            MovementPath movePath = pathGenerator.MakeDirectPath(person.gameObject.transform.position, randomBuilding.transform.position, true);
            person.RunAlongPath(movePath);
        }else{
            //If there's no activated buildings, sends the person to the point on the far left of the active chunk of the grid
            //Gets bottom left point on grid
            int halfOfGridChunkSize = GridDataLoader.current.gridChunkSize / 2;
            Vector2Int bottomLeftGridPoint = new Vector2Int(-halfOfGridChunkSize + 1, -halfOfGridChunkSize + 1);
            Vector3 bottomLeftWorldPoint = GridManager.GM.SwitchFromGridToWorldCoordinates(bottomLeftGridPoint);
            //Vector3 bottomLeftWorldPointShifted = new Vector3(bottomLeftWorldPoint.x + .5f, bottomLeftWorldPoint.y, bottomLeftWorldPoint.z + .5f);
            MovementPath movePath = pathGenerator.MakeDirectPath(person.gameObject.transform.position, bottomLeftWorldPoint, false);
            person.RunAlongPath(movePath);
        }
        

    }

    //Sends person to random activated road when they're off the road system
    private void SendPersonToActivatedRoad(PersonWalk person){

        GameObject randomRoad = RandomTileGenerator.current.GetRandomActivatedRoad();
        if(randomRoad != null){
            MovementPath randomRoadPath = pathGenerator.MakeDirectPath(person.gameObject.transform.position, randomRoad.transform.position, true);
            person.RunAlongPath(randomRoadPath);
        }else{
            //If there's no activated roads, destroys the person
            DestroyPerson(person);
        }
        
    }

    private void SendPersonAlongRoadPath(PersonWalk person){
            MovementPath randomRoadPath = pathGenerator.MakeRandomPathAlongRoads(person.gameObject.transform.position);
            //If there's no connected roads
            if(randomRoadPath.GetPathLength() <= 0){
                Debug.Log("No road paths, destroyed person");
                DestroyPerson(person);
            }
            person.RunAlongPath(randomRoadPath);
    }


    public void RemovePersonFromMemory(PersonWalk person){
        if(allIndestructiblePeople.Contains(person)){
                allIndestructiblePeople.Remove(person);
            }
        if(allPeopleOnCurrentGridChunk.Contains(person)){
            allPeopleOnCurrentGridChunk.Remove(person);
        }
    }

    public void RemoveAllPeopleFromMemory(){
        allIndestructiblePeople.Clear();
        allPeopleOnCurrentGridChunk.Clear();
    }

    private void DestroyPerson(PersonWalk person){
        if(person.gameObject != null){
            RemovePersonFromMemory(person);
            Destroy(person.gameObject);
        }

        //Upon destroying person, checks if a new person should be created
        //UpdateIndestructiblePeopleCount(true);
    }

    private bool ShouldDestroyPerson(PersonWalk person){
        if(person.PreventDestruction){
            if(TooManyIndestructiblePeople()){
                return true;
            }else{
                return false;
            }
        }else{
            return true;
        }
    }

    //Creates a new person and sends them to a random activated building. Called when people button is pressed.
    public void NewPersonAdded(){
        //Debug.Log("New person added");
        Vector3 newPersonPosition = new Vector3(peopleSpawnPoint.x, personPrefab.transform.position.y, peopleSpawnPoint.z);
        GameObject newPerson = Instantiate(personPrefab, newPersonPosition, personPrefab.transform.rotation);
        PersonWalk newPersonWalk = newPerson.GetComponent<PersonWalk>();
        
        if(newPersonWalk != null){
            newPersonWalk.PreventDestruction = false;
            SendPersonToActivatedBuilding(newPersonWalk);
            allPeopleOnCurrentGridChunk.Add(newPersonWalk);
        }
        
    }

    //Creates a person at a random location.
    //If onlyInstantiateInHouses is true, they can only be created in a house.
    //If it's false, they will be instantiated on a road.
    private void CreatePersonAtRandomLocation(bool instantiateInHouse){

        //Debug.Log("Random location");

        Vector3 spawnLocation;

        
        
        if(instantiateInHouse){ //Makes spawn location on a random house
            //Debug.Log("Adding person to random building");
            GameObject randomBuilding = RandomTileGenerator.current.GetRandomActivatedBuilding();
            spawnLocation = randomBuilding.transform.position;
            
        } else{ //Spawn location on a random road
            //Debug.Log("Adding person to random road");
            GameObject randomRoad = RandomTileGenerator.current.GetRandomActivatedRoad();
            spawnLocation = randomRoad.transform.position;
        }

       

        //Creates the new person and sends them along the road path
        Vector3 newPersonPosition = new Vector3(spawnLocation.x, personPrefab.transform.position.y, spawnLocation.z);
        //Debug.Log("New Random location: " + newPersonPosition);
        GameObject newPerson = Instantiate(personPrefab, newPersonPosition, personPrefab.transform.rotation);
        PersonWalk newPersonWalk = newPerson.GetComponent<PersonWalk>();

        allIndestructiblePeople.Add(newPersonWalk);
        allPeopleOnCurrentGridChunk.Add(newPersonWalk);

        if(newPersonWalk != null){
            newPersonWalk.PreventDestruction = true;
            SendPersonAlongRoadPath(newPersonWalk);
        }
        
    }


}
