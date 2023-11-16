/*
 * Created by: Krieger
 * Created on: 6/26/2023
 * 
 * Manages the Carbon Bar in the UI, recieves inputs from the game manager and sets the text, color, and fill amount of the progress bar
 * 
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBar : MonoBehaviour
{
    public float maxBarValue;
    public float currentBarValue;

    public bool doColorTransition = false;

    public Image mask;
    public Image fill;

    public bool doText = true;
    public TextMeshProUGUI barText;
    public string baseText;
    public string units;

    private void Start()
    {
        if(!doText) { barText.gameObject.SetActive(false); }
        SetBarFill();
    }

    void SetBarFill()
    {
        float fillAmount = currentBarValue / maxBarValue;

        // Set mask fill to the current fill amount
        mask.fillAmount = fillAmount;

        // Set the fill color
        if (doColorTransition)
        {
            fill.GetComponent<Image>().color = new Color(fillAmount, 1 - fillAmount, 0.2f);
        }

        // Set text
        if(doText)
        {
            string newText = baseText + "\t" + currentBarValue + " " + units + " / " + maxBarValue + " " + units;
            barText.text = newText;
        }
       
    }

    // Sets the value of the bar directly
    public void SetBarValue(float barVal)
    {
        currentBarValue = barVal;
        SetBarFill();
    }

    // Adds the inputted value to the current bar value (probably mostly useful for testing)
    public void AddToBarValue(float addToBar)
    {
        currentBarValue += addToBar;
        SetBarFill();
    }
}
