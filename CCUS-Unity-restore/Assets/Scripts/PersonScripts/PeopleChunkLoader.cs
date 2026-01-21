using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PeopleChunkLoader : MonoBehaviour
{
    public PeopleMovementManager peopleMoveManager;

    public PersonWalk[] allPeopleOnPreviousChunk;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        peopleMoveManager = GetComponent<PeopleMovementManager>();

        //Adds listeners for when the chunk switch begins and ends
        GameEventManager.current.BeginSwitchingCurrentGroundChunk.AddListener(BeginChunkSwitch);
        GameEventManager.current.SwitchedCurrentGroundChunk.AddListener(EndChunkSwitch);

    }

    private void BeginChunkSwitch(){

        //Updates spawn position for people
        Vector3 GridCenterPoint = GridManager.GM.GetCenter();
        peopleMoveManager.SetSpawnPoint(GridCenterPoint);

        //Gets all of the people on the previous grid chunk
        List<PersonWalk> allPeople = peopleMoveManager.allPeopleOnCurrentGridChunk;

        //Stores all of the people on the previous grid chunk
        allPeopleOnPreviousChunk = allPeople.ToArray();


        foreach(PersonWalk person in allPeopleOnPreviousChunk){

            //Freezes all the people on the previous grid chunk
            person.IsFrozen = true;

        }

        //Deletes all the people from the PeopleMovementManager's memory
        peopleMoveManager.RemoveAllPeopleFromMemory();

        //Creates correct number of people on the new chunk
        peopleMoveManager.UpdateIndestructiblePeopleCount(false);

    
        
        
    }

    private void EndChunkSwitch(){
        //Destroys all of the people on the previous chunk
        foreach(PersonWalk person in allPeopleOnPreviousChunk){
            if(person != null && person.gameObject != null){
                Destroy(person.gameObject);
            }
        }
    }  

    // Update is called once per frame
    void Update()
    {
        
    }
}
