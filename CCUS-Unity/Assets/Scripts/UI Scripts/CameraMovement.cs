using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Keyboard Movement")]
    public float speed = 5f;

    [Header("Drag Settings")]
    public float dragSpeed = 2f;
    public float momentumDamp = 5f; // how quickly momentum slows
    private Vector3 dragMomentum = Vector3.zero;
    private Vector3 lastMousePos;

    [Header("Camera Bounds")]
    public Vector2 minBounds = new Vector2(-10f, -10f);
    public Vector2 maxBounds = new Vector2(10f, 10f);

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        //Keyboard movement
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        movement *= speed * Time.deltaTime;
        movement = Quaternion.AngleAxis(45, Vector3.up) * movement;
        pos += movement;

        //Mouse drag movement
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePos = Input.mousePosition;
            dragMomentum = Vector3.zero; // reset momentum when starting drag
        }

        //Right Click and Drag
        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;

            //Convert delta to world space movement
            Vector3 dragMove = new Vector3(-delta.x, 0, -delta.y) * (dragSpeed * Time.deltaTime);
            pos += dragMove;

            //Save momentum for flick
            dragMomentum = dragMove / Time.deltaTime;
        }
        else
        {
            //Apply momentum when not dragging
            if (dragMomentum.magnitude > 0.01f)
            {
                pos += dragMomentum * Time.deltaTime;
                dragMomentum = Vector3.Lerp(dragMomentum, Vector3.zero, momentumDamp * Time.deltaTime);
            }
        }

        //apply bounds
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.z = Mathf.Clamp(pos.z, minBounds.y, maxBounds.y);

        transform.position = pos;
    }
}
