using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitObject : MonoBehaviour
{
    [SerializeField]
    private GravityObjectInterface ObjectToOrbit;

    [SerializeField]
    private GravityManager GravityManager;

    private void Awake()
    {
        float distance = Vector3.Distance(transform.position, ObjectToOrbit.transform.position);
        GravityObjectInterface orbitingObject = gameObject.GetComponent<GravityObjectInterface>();
        float speed = Mathf.Sqrt(GravityManager.GetGConst() * (ObjectToOrbit.mass/distance));
        if(orbitingObject.initialImpuls == Vector3.zero)
        {
            orbitingObject.initialImpuls = Vector3.back;
        }
        orbitingObject.initialSpeed = speed;
    }
}
