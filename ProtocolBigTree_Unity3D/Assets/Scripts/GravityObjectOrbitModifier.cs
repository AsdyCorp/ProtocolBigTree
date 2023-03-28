using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObjectOrbitModifier : MonoBehaviour
{
    [SerializeField]
    private GravityObjectInterface gravityObject;

    [SerializeField]
    private float rotationMultiplier;

    [SerializeField]
    private float speedMultiplier;

    [SerializeField]
    private float shiftSpeedMultiplier;

    [SerializeField]
    private float shiftRotationMultiplier;

    void Start()
    {
        if (gravityObject == null)
        {
            gravityObject = gameObject.GetComponent<GravityObjectInterface>();
        }
    }

    public void Accelerate(float speed)
    {
        gravityObject.GetGravityObject().SetImpuls(
            gravityObject.GetGravityObject().GetCurrentImpuls() 
            + (gravityObject.transform.forward * Time.deltaTime * speed));
    }

    public void Rotate (float speed)
    {
        gravityObject.transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }

    void Update()
    {
        float shiftRotationCoef = 1;
        float shiftSpeedCoef = 1;
        if (GameManager.Instance.GetInputManager().Shift())
        {
            shiftRotationCoef = shiftRotationMultiplier;
            shiftSpeedCoef = shiftSpeedMultiplier;
        }
        Accelerate(GameManager.Instance.GetInputManager().GetVerticalInput() * speedMultiplier * shiftSpeedCoef);
        Rotate(GameManager.Instance.GetInputManager().GetHorizontalInput() * rotationMultiplier * shiftRotationCoef);
    }
}
