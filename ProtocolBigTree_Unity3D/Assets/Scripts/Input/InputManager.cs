using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

/// 
/// Never add through editor or script outside gamemanager
/// 
public class InputManager : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private bool shift;
    private float timeInput;
    private Vector3 mousePosition;
    private float mouseWheelInput;
    private bool rightMouseButton;
    public float GetHorizontalInput()
    {
        return horizontalInput;
    }

    public float GetVerticalInput()
    {
        return verticalInput;
    }

    public bool Shift()
    {
        return shift;
    }

    public float GetTimeInput()
    {
        return timeInput;
    }

    public Vector3 GetMousePosition()
    {
        return mousePosition;
    }

    public float GetMouseScrollWheelInput()
    {
        return mouseWheelInput;
    }

    public bool GetRightMouseButtonDown()
    {
        return rightMouseButton;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        shift = Input.GetKey(KeyCode.LeftShift);
        timeInput = Input.GetAxis("TimeAxis");
        mousePosition = Input.mousePosition;
        mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");
        rightMouseButton = Input.GetButton("Fire2");
    }
}
