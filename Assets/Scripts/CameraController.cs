using UnityEngine;
using System.Collections;
using System;
public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 5f; // Adjust this value to control the zoom speed
    public float minSize = 1f; // Minimum size of the camera
    public float maxSize = 10f; // Maximum size of the camera
    public float transitionSpeed = 1f; // Adjust for the smooth transition speed

    private Camera _camera; // Cache the camera component
    private Vector3 _dragOrigin; // To keep track of where the drag started
    public float rotationSpeed = 1f; // Adjust for the smooth transition speed

    private Vector3 profileAngleRotation = new Vector3(328, 132.455002f, 64.1399918f);
    private Vector3 faceAngleRotation = new Vector3(0, 180, 0);
    private Vector3 profilePosition = new Vector3(-46.1500015f, -38.4599991f, 42.6300011f);
    private Vector3 facePosition = new Vector3(0, 0, 100);
    public bool isProfile = true;
    public bool startTransition = false;
   public Texture2D handCursorTexture; // Assign this in the Unity Editor
    public Texture2D handCursorClosedTexture; // Assign this in the Unity Editor

    private Vector2 cursorHotspot; // This is the position within the cursor image that will click
    public bool isDragging = false;
    private void Awake()
    {
        _camera = GetComponent<Camera>(); // Get the Camera component once at start
    }
    private void Start()
    {
        cursorHotspot = new Vector2(handCursorTexture.width / 2, handCursorTexture.height / 2);
    }


	// Use this for initialization
	
    void Update()
    {
        // Check for mouse scroll wheel input for zooming
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            ZoomCamera(scrollInput);
        }

        if(Input.GetKey(KeyCode.LeftAlt))
        {
            isDragging = true;
            Cursor.SetCursor(handCursorTexture, cursorHotspot, CursorMode.Auto);
        }
        else
        {
            isDragging = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }


        // Initiate camera movement
         if (Input.GetMouseButtonDown(2) || Input.GetKey(KeyCode.LeftAlt) && (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))) // 1 is for right mouse button
        {
            isDragging = true;
            _dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);
            // Change icon of the cursor to hand
            Cursor.SetCursor(handCursorTexture, cursorHotspot, CursorMode.Auto);
        }
        else if (Input.GetMouseButtonUp(2) || Input.GetKey(KeyCode.LeftAlt) && (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0)))
        {
            isDragging = false;
            // Reset the cursor to default when mouse button is released
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        else if (Input.GetMouseButton(2) || Input.GetKey(KeyCode.LeftAlt) && (Input.GetMouseButton(1) || Input.GetMouseButton(0)))
        {
            isDragging = true;
            Cursor.SetCursor(handCursorClosedTexture, cursorHotspot, CursorMode.Auto);
            MoveCamera();
        }

        // Toggle rotation on space a press
        if (startTransition == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(TogglePositionAndRotation());
            }
        }
    }

    
  
  
    IEnumerator TogglePositionAndRotation()
    {
        Vector3 targetPosition = isProfile ? facePosition : profilePosition;
        Vector3 targetRotation = isProfile ? faceAngleRotation : profileAngleRotation;

        // Calculate the duration of the transition based on the transition speed
        float duration = 1f / transitionSpeed;
        float time = 0;

        bool pauseStateBeforeTransition = GridManager.instance.isPause;
        GridManager.instance.isPause = true;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(targetRotation);
        startTransition = true;
        KeyBindingUI.instance.ToggleViewChange();

        while (time < duration)
        {
            // decrease the opacity of the cellshistory
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, time / duration);
            time += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        startTransition = false;
        KeyBindingUI.instance.ToggleViewChange();
        // Ensure the target position and rotation are exactly applied after the interpolation
        transform.position = targetPosition;
        transform.rotation = endRotation;
        isProfile = !isProfile; // Toggle the flag
        GridManager.instance.isPause = pauseStateBeforeTransition;


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
