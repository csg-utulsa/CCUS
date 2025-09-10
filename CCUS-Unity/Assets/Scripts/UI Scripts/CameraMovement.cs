using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Keyboard Movement")]
    public float speed = 5f;

    [Header("Drag Settings")]
    public float dragSpeed = 2f;
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

        // --- Keyboard movement (unchanged) ---
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        movement *= speed * Time.deltaTime;
        movement = Quaternion.AngleAxis(45, Vector3.up) * movement;
        pos += movement;

        // --- Mouse drag ---
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;

            // Convert delta to world space based on camera orientation
            Vector3 right = cam.transform.right;
            Vector3 forward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;

            Vector3 dragMove = (-delta.x * right + -delta.y * forward) * (dragSpeed * Time.deltaTime);
            pos += dragMove;
        }

        // --- Apply bounds ---
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.z = Mathf.Clamp(pos.z, minBounds.y, maxBounds.y);

        transform.position = pos;
    }
}