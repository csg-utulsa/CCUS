using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeoplePanel : MonoBehaviour
{
    public static PeoplePanel _peoplePanel;
    // Start is called before the first frame update
    void Start()
    {
        if(_peoplePanel == null){
            _peoplePanel = this;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isMouseOverPanel(){
        Vector2 mousePos = Input.mousePosition;
        return RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), mousePos);
    }
}
