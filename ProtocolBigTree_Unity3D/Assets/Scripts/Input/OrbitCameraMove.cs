using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Move camera with mouse in orbit prediction window 
public class OrbitCameraMove : MonoBehaviour
{
    Vector3 mouseCoords;
    Vector2 screenSize;


    // what percent of screen will be used as active area for moving (border)
    [SerializeField]
    private float activeScreenPercent;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private float cameraMovingSpeed = 1.0f;

    [SerializeField]
    private float zoomSpeed = 1.0f;


    private void Start()
    {

        if(mainCamera == null )
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    void MoveCameraXZ(Vector3 direction)
    {
        mainCamera.transform.position += new Vector3(-direction.x, 0, -direction.y) * cameraMovingSpeed;
    }

    void MoveCameraY(Vector3 direction)
    {
        mainCamera.transform.position += -direction * zoomSpeed;
    }

    void RotateCameraY(float direction, float speed = 1.0f)
    {
        mainCamera.transform.Rotate(0, 0,direction * speed);
    }

    void RotateCameraX(float direction, float speed = 1.0f)
    {
        mainCamera.transform.Rotate(direction * speed, 0, 0);
    }


    private void Update()
    {
        screenSize.x = Screen.width;
        screenSize.y = Screen.height;

        mouseCoords = GameManager.Instance.GetInputManager().GetMousePosition();

        float zoomAxis = GameManager.Instance.GetInputManager().GetMouseScrollWheelInput();

        Vector2 mouseScreenPositionNormalised = new Vector2(mouseCoords.x / screenSize.x, mouseCoords.y / screenSize.y); // position from 0 to 1 in x/ 0 to 1 in y

        Vector3 windowCenter = new Vector3(screenSize.x / 2, screenSize.y / 2, 0);
        Vector3 direction = windowCenter - mouseCoords;
        bool rotateCamera = GameManager.Instance.GetInputManager().GetRightMouseButtonDown();

        if (rotateCamera == false)
        {
            if (mouseScreenPositionNormalised.x <= activeScreenPercent / 100f ||
                mouseScreenPositionNormalised.x >= 1 - activeScreenPercent / 100f ||
                mouseScreenPositionNormalised.y <= activeScreenPercent / 100f ||
                mouseScreenPositionNormalised.y >= 1 - activeScreenPercent / 100f)
            {
                MoveCameraXZ(direction.normalized);
            }
        }
        else 
        {
            RotateCameraY(direction.normalized.x);
            RotateCameraX(direction.normalized.y);
        }

        if(zoomAxis != 0.0f)
        {
            MoveCameraY(new Vector3(0, zoomAxis, 0));
        }


    }
}
