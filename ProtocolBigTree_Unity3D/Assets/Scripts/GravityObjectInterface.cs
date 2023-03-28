using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GravityObjectInterface : MonoBehaviour
{
    [SerializeField]
    private float initialSpeed;

    [SerializeField]
    private Vector3 initialImpuls;

    [SerializeField]
    private float mass;

    [SerializeField]
    private float radius;

    [SerializeField]
    private bool isKinematic = false; // not interactable trhough gravitation 

    private GravityObject gravityObject; 

    public void CreateGravityObject()
    {
        if(gravityObject == null)
        {
            gravityObject = new GravityObject(mass, initialImpuls * initialSpeed, transform.position, isKinematic, radius, this);
        }
    }

    public GravityObject GetGravityObject()
    {
        return gravityObject;
    }

}

public class GravityObject 
{

    private float mass;

    private bool isKinematic = false; 

    private Vector3 currentImpuls = Vector3.zero;

    private Vector3 position;

    private float radius;

    public GravityObjectInterface gravityObjectInterface { get; }
    

    public GravityObject(float _mass, Vector3 _currentImpuls, Vector3 _position, bool _isKinematic, float _radius, GravityObjectInterface _gravityObjectInterface = null)
    {
        mass = _mass;
        currentImpuls = _currentImpuls;
        position = _position;
        isKinematic = _isKinematic;
        radius = _radius;
        gravityObjectInterface = _gravityObjectInterface;
    }

    public bool IsKinematic()
    {
        return isKinematic;
    }

    public float GetMass()
    {
        return mass;
    }
    
    public float GetRadius()
    {
        return radius;
    }


    public void SetImpuls(Vector3 newImpuls)
    {
        if (isKinematic == false)
        {
            currentImpuls = newImpuls;
        }
    }

    public Vector3 GetCurrentImpuls()
    {
        return currentImpuls;
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    /*public void SetPosition(Vector3 _position)
    {
        position = _position;
    }**/

    public void UpdatePosition()
    {
        position += currentImpuls;
    }
}
