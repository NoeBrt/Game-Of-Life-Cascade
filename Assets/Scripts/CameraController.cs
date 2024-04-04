using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 5f; // Adjust this value to control the zoom speed
    public float minSize = 1f; // Minimum size of the camera
    public float maxSize = 10f; // Maximum size of the camera

    void Update()
    {
        // Check for mouse scroll wheel input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Zoom the camera based on the mouse scroll input
        ZoomCamera(scrollInput);
    }

    void ZoomCamera(float scrollInput)
    {
        // Get the current size of the camera
        float currentSize = GetComponent<Camera>().orthographicSize;

        // Calculate the new size after zooming
        float newSize = Mathf.Clamp(currentSize - scrollInput * zoomSpeed, minSize, maxSize);

        // Set the new size to the camera
        GetComponent<Camera>().orthographicSize = newSize;
    }
}
