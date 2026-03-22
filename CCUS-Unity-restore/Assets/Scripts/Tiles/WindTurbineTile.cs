using UnityEngine;

public class WindTurbineTile : ActivatableTile
{
    public override void ActivateTile(){
        base.ActivateTile();

        SpinObject propellers = GetComponentInChildren<SpinObject>(true);
        if(propellers != null){
            propellers.IsSpinning = true;
        }
    }

    public override void DeactivateTile(){
        base.DeactivateTile();

        SpinObject propellers = GetComponentInChildren<SpinObject>(true);
        if(propellers != null){
            propellers.IsSpinning = false;
        }
    }
}
