using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject textBox;

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Show the text box when the mouse hovers over the button
        textBox.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide the text box when the mouse exits the button
        textBox.SetActive(false);
    }
}
