using UnityEngine;

public class ToolTipArrow : MonoBehaviour
{
    public float verticalOffset = 60;
    public float VerticalOffset{
        get{
            return verticalOffset * CanvasScalarFactor.CSF.GetScaleFactor();
        }
    }
    public float edgeBuffer = 90f;

    public void SetPosition(float verticalPosition){
        verticalPosition = ClampArrowPosition(verticalPosition - verticalOffset);
        transform.position = new Vector3(transform.position.x, verticalPosition, transform.position.z);
    }

    private float ClampArrowPosition(float inputPosition){

        //NOTE: All heights (besides Screen.height) must be multiplied by scaleFactor to account for different screen sizes
        float scaleFactor = CanvasScalarFactor.CSF.GetScaleFactor();

        //Arrow is too low
        if(inputPosition < (edgeBuffer * scaleFactor)){
            return (edgeBuffer) * scaleFactor;

        //Arrow is too high
        //Adds height of top of tool tip background, because the tool tip position is marked right below it
        } else if(inputPosition > (Screen.height - (edgeBuffer * scaleFactor))){
            return (Screen.height) - (edgeBuffer * scaleFactor);
        
        }
        else{ // arrow is neither too high nor too low
            return inputPosition;
        }
    }

}
