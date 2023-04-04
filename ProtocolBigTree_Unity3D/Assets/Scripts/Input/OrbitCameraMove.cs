using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


// Move camera with mouse in orbit prediction window 
public class OrbitCameraMove : MonoBehaviour
{
    Vector3 mouseCoords;
    Vector2 screenSize;


    // what percent of screen will be used as active area for moving (border)
    [SerializeField]
    private float activeScreenPercent = 10f;

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
        mainCamera.transform.position += mainCamera.transform.right * -direction.x * cameraMovingSpeed;

        mainCamera.transform.position += Vector3.ProjectOnPlane(mainCamera.transform.forward,new Vector3(0,1,0)) * -direction.y * cameraMovingSpeed; /// fucking magic
    }

    void MoveCameraZoom(float direction)
    {
        mainCamera.transform.position += mainCamera.transform.forward * zoomSpeed * direction;
    }

    void RotateCameraYaw(float direction, float speed = 1.0f)
    {
        mainCamera.transform.Rotate(mainCamera.transform.up, -direction * speed,Space.World);
        
        mainCamera.transform.eulerAngles = new Vector3(mainCamera.transform.rotation.eulerAngles.x, mainCamera.transform.rotation.eulerAngles.y, 0);
    }

    void RotateCameraPitch(float direction, float speed = 1.0f)
    {
        mainCamera.transform.Rotate(mainCamera.transform.right, direction * speed, Space.World);
       
        mainCamera.transform.eulerAngles = new Vector3(mainCamera.transform.rotation.eulerAngles.x, mainCamera.transform.rotation.eulerAngles.y, 0);
    }

    //check if pointer in active area
    bool pointerInActiveArea(Vector3 mousePosition)
    {
        Vector2 mouseScreenPositionNormalised = new Vector2(mousePosition.x / screenSize.x, mousePosition.y / screenSize.y); // position from 0 to 1 in x/ 0 to 1 in y

        if (mouseScreenPositionNormalised.x <= activeScreenPercent / 100f ||
                mouseScreenPositionNormalised.x >= 1 - activeScreenPercent / 100f ||
                mouseScreenPositionNormalised.y <= activeScreenPercent / 100f ||
                mouseScreenPositionNormalised.y >= 1 - activeScreenPercent / 100f)
        {
            return true;
        }

        return false;
    }

    private void Update()
    {
        
        screenSize.x = Screen.width;
        screenSize.y = Screen.height;

        mouseCoords = GameManager.Instance.GetInputManager().GetMousePosition();

        float zoomAxis = GameManager.Instance.GetInputManager().GetMouseScrollWheelInput();

        Vector3 windowCenter = new Vector3(screenSize.x / 2, screenSize.y / 2, 0);
        Vector3 direction = windowCenter - mouseCoords;
        bool rotateCamera = GameManager.Instance.GetInputManager().GetRightMouseButtonDown();

        if (rotateCamera == false)
        {
            if(pointerInActiveArea(mouseCoords))
            {
                MoveCameraXZ(direction.normalized);
            }
        }
        else 
        {
            RotateCameraYaw(Mathf.Clamp(direction.x / (screenSize.x / 2), -1.0f, 1.0f), 0.5f);
            RotateCameraPitch(Mathf.Clamp(direction.y / (screenSize.y / 2), -1.0f, 1.0f), 0.5f);
        }

        if(zoomAxis != 0.0f)
        {
            MoveCameraZoom(zoomAxis);
        }

    }
}
