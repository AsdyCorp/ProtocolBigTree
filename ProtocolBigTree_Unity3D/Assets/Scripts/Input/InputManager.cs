using System.Collections;
using System.Collections.Generic;
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

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        shift = Input.GetKey(KeyCode.LeftShift);
        timeInput = Input.GetAxis("TimeAxis");
    }
}
