using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YearClockSpinner : MonoBehaviour
{

    private float year;
    private float secondsPerYear = 4.0f; //This code spins the clockhand on the year in this many seconds

    void FixedUpdate()
    {
        year += Time.deltaTime / secondsPerYear;

        float yearNormalized = year % 1f;

        float rotationDegreesPerYear = 360f;

        transform.eulerAngles = new Vector3(0, 0, -yearNormalized * rotationDegreesPerYear);
    }
}
