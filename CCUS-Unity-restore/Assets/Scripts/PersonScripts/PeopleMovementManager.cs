using UnityEngine;
using System;

public class PeopleMovementManager : MonoBehaviour
{
    public static PeopleMovementManager current;
    public GameObject personPrefab;
    public GameObject peopleSpawnPoint;
    public MovementPathGenerator pathGenerator;

    void Start(){
        GameEventManager.current.BeginSwitchingCurrentGroundChunk.AddListener(SwitchedChunk);
        GameEventManager.current.PersonJustAdded.AddListener(NewPersonAdded);
        if(current == null){
            current = this;
        }

        //Sets the person prefab position
        peopleSpawnPoint.transform.position = GridManager.GM.GetCenter();
    }

    //For a test. Delete before build
    void Update(){
        if(Input.GetKeyDown(KeyCode.Q)){
            NewPersonAdded();
        }
    }

    private void SwitchedChunk(){
        peopleSpawnPoint.transform.position = GridManager.GM.GetCenter();
    }

    


    //Creates a new person and sends them to a random activated building
    public void NewPersonAdded(){
        GameObject newPerson = Instantiate(personPrefab, peopleSpawnPoint.transform.position, personPrefab.transform.rotation);
        PersonWalk newPersonWalk = newPerson.GetComponent<PersonWalk>();
        
        if(newPersonWalk != null){
            SendPersonToActivatedBuilding(newPersonWalk);
        }
        
    }

    public void PersonFinishedPath(PersonWalk person){

        //If person finishes path on a building, teleports them to a different building.
        //Unless there's too many people or no activated buildings. In that case, it destroys them
        bool personIsOnBuilding = person.PersonIsOnActivatableBuilding();
        if(personIsOnBuilding){
            GameObject randomBuilding = RandomTileGenerator.current.GetRandomActivatedBuilding();
            if(randomBuilding != null){
                person.TeleportPersonToLocation(RandomTileGenerator.current.GetRandomActivatedBuilding().transform.position); 
                SendPersonAlongRoadPath(person);
                // MovementPath randomRoadPath = pathGenerator.MakeRandomPathAlongRoads(person.gameObject.transform.position);
                // person.RunAlongPath(randomRoadPath);
            } else{
                DestroyPerson(person);
            }
        
           //If person finishes their path on a road, it gives them a new path along the road
        } else if(pathGenerator.PointIsOnPath(person.gameObject.transform.position)) { 
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
            MovementPath movePath = pathGenerator.MakeDirectPath(person.gameObject.transform.position, randomBuilding.transform.position);
            person.RunAlongPath(movePath);
        }else{
            //If there's no activated buildings, sends the person to the point on the far left of the active chunk of the grid
            //Gets bottom left point on grid
            int halfOfGridChunkSize = GridDataLoader.current.gridChunkSize / 2;
            Vector2Int bottomLeftGridPoint = new Vector2Int(-halfOfGridChunkSize, -halfOfGridChunkSize);
            Vector3 bottomLeftWorldPoint = GridManager.GM.SwitchFromGridToWorldCoordinates(bottomLeftGridPoint);
            MovementPath movePath = pathGenerator.MakeDirectPath(person.gameObject.transform.position, bottomLeftWorldPoint);
        }
        
    }

    //Sends person to random activated road when they're off the road system
    private void SendPersonToActivatedRoad(PersonWalk person){

        GameObject randomRoad = RandomTileGenerator.current.GetRandomActivatedRoad();
        if(randomRoad != null){
            MovementPath randomRoadPath = pathGenerator.MakeDirectPath(person.gameObject.transform.position, randomRoad.transform.position);
            person.RunAlongPath(randomRoadPath);
        }else{
            //If there's no activated roads, destroys the person
            DestroyPerson(person);
        }
        
    }

    private void SendPersonAlongRoadPath(PersonWalk person){
            MovementPath randomRoadPath = pathGenerator.MakeRandomPathAlongRoads(person.gameObject.transform.position);
            //If their's no connected roads
            if(randomRoadPath.GetPathLength() <= 0){
                Debug.Log("No road paths, destroyed person");
                DestroyPerson(person);
            }
            person.RunAlongPath(randomRoadPath);
    }

    private void DestroyPerson(PersonWalk person){
        if(person.gameObject != null){
            Destroy(person.gameObject);
        }
    }



}
