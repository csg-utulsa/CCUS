using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NewPeopleCreator : MonoBehaviour
{
    public float peopleAnimationTime = .3f;
    public float newPersonAnimationHeight = 1f;
    public float timeBetweenCreatingPeople = 0f;
    
    private int numOfPeopleToAdd = 0;
    private bool currentlyAddingPeople = false;

    private PeopleMovementManager peopleMover;
    private GameObject personPrefab;


    void Start()
    {
        GameEventManager.current.PersonJustAdded.AddListener(AddPersonButtonPressed);
        peopleMover = GetComponent<PeopleMovementManager>();
        personPrefab = peopleMover.personPrefab;

    }

    private void AddPersonButtonPressed(){
        numOfPeopleToAdd++;

        //Commented out portion waits to add a new person until the last person has finished its animation
        // if(!currentlyAddingPeople){
        //     currentlyAddingPeople = true;
        //     StartCoroutine(WaitingForAddingPeople());
        // }

        StartCoroutine(ImmediatelyAddPerson());
    }

    private IEnumerator ImmediatelyAddPerson(){

        //Runs new person creation animation and then creates a person when it's done
        NewPersonCreationAnimation();
        yield return new WaitForSeconds(peopleAnimationTime);
        peopleMover.NewPersonAdded();


    }

    //Runs after every person is finished being animated into place and then adds the next person
    private IEnumerator WaitingForAddingPeople(){
        while(numOfPeopleToAdd > 0){

            //Runs new person creation animation and then creates a person when it's done
            NewPersonCreationAnimation();
            yield return new WaitForSeconds(peopleAnimationTime);
            peopleMover.NewPersonAdded();
            numOfPeopleToAdd--;

            //Waits time between adding new people
            yield return new WaitForSeconds(timeBetweenCreatingPeople);

        }

        currentlyAddingPeople = false;
    }

    //Start a new person creation animation
    private void NewPersonCreationAnimation(){

        GameObject newPerson = Instantiate(personPrefab, peopleMover.peopleSpawnPoint, personPrefab.transform.rotation);
        NewPersonAnimation newPersonAnimator = newPerson.GetComponent<NewPersonAnimation>();
        newPersonAnimator.BeginNewPersonAnimation(peopleAnimationTime, newPersonAnimationHeight, peopleMover.peopleSpawnPoint);
    

    }

}
