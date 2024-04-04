using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 5f; // Adjust this value to control the zoom speed
    public float minSize = 1f; // Minimum size of the camera
    public float maxSize = 10f; // Maximum size of the camera

    private Camera _camera; // Cache the camera component
    private Vector3 _dragOrigin; // To keep track of where the drag started

    private void Awake()
    {
        _camera = GetComponent<Camera>(); // Get the Camera component once at start
    }

   void Update()
    {
        // Check for mouse scroll wheel input for zooming
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            ZoomCamera(scrollInput);
        }

        // Initiate camera movement
        if (Input.GetMouseButtonDown(1)) // 1 is for right mouse button
        {
            _dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(1))
        {
            MoveCamera();
        }
    }

    void MoveCamera()
    {
        Vector3 difference = _dragOrigin - _camera.ScreenToWorldPoint(Input.mousePosition);

        // Apply the movement to the camera
        transform.position += difference;

        // Update the drag origin for the next frame, to make the movement continuous
        _dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);
    }
  void ZoomCamera(float scrollInput)
    {
        // Get the current size of the camera
        float currentSize = _camera.orthographicSize;

        // Calculate the new size after zooming
        float newSize = Mathf.Clamp(currentSize - scrollInput * zoomSpeed, minSize, maxSize);

        if (Mathf.Abs(newSize - currentSize) > Mathf.Epsilon)
        {
            // Calculate how much we will zoom
            float zoomFactor = currentSize / newSize;

            // Calculate the world position of the mouse before zooming
            Vector3 worldBeforeZoom = ScreenToWorldPoint(Input.mousePosition, currentSize);

            // Set the new size to the camera
            _camera.orthographicSize = newSize;

            // Calculate the world position of the mouse after zooming
            Vector3 worldAfterZoom = ScreenToWorldPoint(Input.mousePosition, newSize);

            // Move the camera by the difference in world positions caused by zooming
            Vector3 cameraPositionAdjustment = worldBeforeZoom - worldAfterZoom;
            transform.position += cameraPositionAdjustment;
        }
    }

    // Converts screen point to world point at a given z distance from the camera
    private Vector3 ScreenToWorldPoint(Vector3 screenPoint, float zDistance)
    {
        // Adjust the screen point to account for the distance from the camera
        screenPoint.z = zDistance - _camera.transform.position.z;
        return _camera.ScreenToWorldPoint(screenPoint);
    }

    // Your existing ScreenToWorldPoint method...



  
}
