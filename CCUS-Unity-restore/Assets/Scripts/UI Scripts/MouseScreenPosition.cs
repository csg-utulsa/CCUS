using UnityEngine;

public class MouseScreenPosition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool MouseIsOverScreen(){
        Vector2 mouseScreenPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        return mouseScreenPosition.x <= 1f && mouseScreenPosition.x >= 0f && mouseScreenPosition.y <= 1f && mouseScreenPosition.y >= 0f;
    }
}
