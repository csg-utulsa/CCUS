using UnityEngine;
using System;

public class NewPersonAnimation : MonoBehaviour
{
    public float resizeAnimationTimePercentage = .5f;
    public float resizeAnimationSize = 2f;


    private float animationTotalTime = .5f;
    private float resizeAnimationTime = .25f;
    private float animationHeight = 1f;
    private Vector3 newPersonCreationPosition;

    private float previousHeight = 1f;
    private float targetHeight = 0f;
    private float currentHeight = 1f;

    private float timer = 0f;
    private bool isMoving = false;

    void Update()
    {
        
        if(isMoving){
            timer += Time.deltaTime;
            if(timer <= resizeAnimationTime){ //Animation is still in the resizing phase
                float currentScaleSize = GetCurrentScalingSize(1f, resizeAnimationSize, timer, animationTotalTime);
                SetCurrentScale(currentScaleSize);


            } else if(timer <= animationTotalTime){ //Animation is in the falling phase
                float currentScaleSize = GetCurrentScalingSize(1f, resizeAnimationSize, timer, animationTotalTime);
                SetCurrentScale(currentScaleSize);

                //Calculates the height the person should be at
                float timeSinceBeginningFall = timer - resizeAnimationTime;
                float timeToFall = animationTotalTime - resizeAnimationTime;
                float currentHeight = CalculateCurrentYPosition(previousHeight, targetHeight, timeSinceBeginningFall, timeToFall);
                
                //Sets the person's current height
                SetCurrentHeight(currentHeight);

            } else{ //Animation is over
                SetCurrentHeight(targetHeight);
                isMoving = false;
                timer = 0f;
                currentHeight = targetHeight;
                Destroy(this.gameObject);
            }  
        }
    }

    //Called by NewPeopleCreator when a new person is created
    public void BeginNewPersonAnimation(float _animationTotalTime, float _animationHeight, Vector3 _newPersonCreationPosition){
        //Sets beginning variables
        animationTotalTime = _animationTotalTime;
        resizeAnimationTimePercentage = animationTotalTime * resizeAnimationTime;
        animationHeight = _animationHeight;
        newPersonCreationPosition = _newPersonCreationPosition;

        isMoving = true;
        timer = 0f;

        //Sets starting height of the animation
        SetCurrentHeight(animationHeight);
        currentHeight = animationHeight;
        previousHeight = animationHeight;
        targetHeight = 0f;

        
        
    }

    //Calculates the proper size for the person during the resize the animation
    private float GetCurrentScalingSize(float sizeOne, float sizeTwo, float currentTime, float totalTime){
        float percentTimePassed = currentTime / totalTime;
        float newScaleFactor = Mathf.Sin(percentTimePassed * Mathf.PI);
        float deltaSize = sizeTwo - sizeOne;

        float finalScaleFactor = sizeOne + (deltaSize * newScaleFactor);
        return finalScaleFactor;
    }

    //Calculates the current height the person should be at during the fall-to-the-ground animation
    public float CalculateCurrentYPosition(float startPosition, float endPosition, float currentTime, float totalTime){
        float percentTimePassed = currentTime / totalTime;
        float deltaHeight = endPosition - startPosition;

        float newHeight = startPosition + (deltaHeight * percentTimePassed);
        return newHeight;
    }

    //Sets current scale of object
    private void SetCurrentScale(float currentScaleToSet){
        transform.localScale = new Vector3(currentScaleToSet, currentScaleToSet, currentScaleToSet);
    }

    //Sets Y Position of object
    private void SetCurrentHeight(float currentHeightToSet){
        transform.position = new Vector3(transform.position.x, currentHeightToSet, transform.position.z);
    }

}
