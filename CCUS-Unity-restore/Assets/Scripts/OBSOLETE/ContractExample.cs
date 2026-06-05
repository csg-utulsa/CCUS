using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractExample : MonoBehaviour
{
    public string taskName;
    public enum resourceOption { CARBON, MONEY }
    [Space(10)]
    public resourceOption goalResource;
    public float goalAmount;
    //Might need goal units, I'm not sure what they're for though
}
