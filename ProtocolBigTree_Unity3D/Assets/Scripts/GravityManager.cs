using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GravityManager : MonoBehaviour
{


    private List<GravityObject> gravityObjectsList;

    [SerializeField]
    private GravityObjectInterface[] gravityObjectsPublicArray;

    [SerializeField]
    private float bigGConst;

    [SerializeField]
    private float calculationPause = 0.02f; // time for corutine pause

    [SerializeField]
    private bool calculateAllGravityObjectsInScene = false; // load all gravity objects from scene 

    [SerializeField]
    private float shiftTimeSpeedModifier;

    [SerializeField]
    private float massDifferenceIgnore = 0; // ignore force from objects with mass difference m1/m2 smaller than coef 

    private void Awake()
    {
        gravityObjectsList = new List<GravityObject>();
        // Fill gravity object container
        if (calculateAllGravityObjectsInScene)
        {
            GravityObjectInterface[] gravityObjectInterfaces = FindObjectsOfType<GravityObjectInterface>();
            foreach(GravityObjectInterface gravityObjectInterface in gravityObjectInterfaces)
            {
                gravityObjectInterface.CreateGravityObject();
                gravityObjectsList.Add(gravityObjectInterface.GetGravityObject());
            }

        }
        else
        {
            foreach (GravityObjectInterface gravityObjectInterface in gravityObjectsPublicArray)
            {
                gravityObjectInterface.CreateGravityObject();
                gravityObjectsList.Add(gravityObjectInterface.GetGravityObject());
            }
        }

        StartCoroutine(GravityUpdate());

    }


    public List<Vector3> PredictOrbit(GravityObject predictionAim, int steps, bool predictCollisions , out GravityObject collissionObject)
    {

        List<Vector3> result = new List<Vector3>();

        int resultId = -1;

        float currentGConst = bigGConst;

        for(int i=0; i< gravityObjectsList.Count; i++)
        {
            if(predictionAim == gravityObjectsList[i])
            {
                resultId = i;
                break;
            }
        }

        List<GravityObject> modelObjects = new List<GravityObject>();

        //copy real object properties to model objects
        foreach(GravityObject obj in gravityObjectsList)
        {
            modelObjects.Add(new GravityObject(obj.GetMass(), obj.GetCurrentImpuls(), obj.GetPosition(), obj.IsKinematic(), obj.GetRadius()));
        }

        //calculate
        for (int i=0; i<steps; i++)
        {
            UpdateGravityFrame( modelObjects);

            foreach (GravityObject modelGravityObject in modelObjects)
            {
                modelGravityObject.UpdatePosition();

                if (predictCollisions)
                {
                    Vector3 offset = modelGravityObject.GetPosition() - modelObjects[resultId].GetPosition();
                    if(offset == Vector3.zero)
                    {
                        continue;
                    }
                    float sqrLen = offset.sqrMagnitude;
                    if (sqrLen < modelGravityObject.GetRadius() * modelGravityObject.GetRadius())
                    {
                        result.Add(modelObjects[resultId].GetPosition());
                        collissionObject = modelGravityObject;
                        return result;
                    }
                }

            }

            result.Add(modelObjects[resultId].GetPosition());
        }
        collissionObject = null;
        return result;
    }
    
    // calculate gravity force beetween 2 objets 

    public float GetGravityForce2obj(Vector3 objPos1, float objMass1, Vector3 ObjPos2, float objMass2, float bigGConst)
    {
        Vector3 direction = ObjPos2 - objPos1;

        if (direction == Vector3.zero) //same object/clipping object - skip calculation
        {
            return 0.0f;
        }

        float sqrLength = direction.sqrMagnitude;

        float interactionForce = bigGConst * (objMass1 * objMass2) / sqrLength;

        return interactionForce;
    }

    void UpdateGravityFrame( List<GravityObject> FrameObjects)
    {
        foreach(GravityObject firstObject in FrameObjects)
        {
            Vector3 force = Vector3.zero;
            foreach(GravityObject secondObject in FrameObjects)
            {
                if( firstObject.GetMass() / secondObject.GetMass() > massDifferenceIgnore)
                {
                    continue;
                }

                Vector3 direction = secondObject.GetPosition() - firstObject.GetPosition();

                force += direction.normalized *
                    (GetGravityForce2obj(firstObject.GetPosition(), firstObject.GetMass(), secondObject.GetPosition(), secondObject.GetMass(), bigGConst)
                    / firstObject.GetMass());

            }
            firstObject.SetImpuls(firstObject.GetCurrentImpuls() + force);
        }
    }

    public void ChangeGConstDynamicly(float newGConst, bool scaleObjectsImpulse, ref List<GravityObject> gravityObjects)
    {
        if(bigGConst==0.0f)
        {
            bigGConst = newGConst;
            return;
        }

        if (scaleObjectsImpulse)
        {
            foreach (GravityObject gravityObject in gravityObjects)
            {
                gravityObject.SetImpuls(gravityObject.GetCurrentImpuls() * Mathf.Sqrt(newGConst / bigGConst));
            }
        }

        bigGConst = newGConst;
    }

    IEnumerator GravityUpdate()
    {
        while(true)
        {
            UpdateGravityFrame( gravityObjectsList);

            foreach (GravityObject gravityObject in gravityObjectsList)
            {
                gravityObject.UpdatePosition();
                gravityObject.gravityObjectInterface.transform.position = gravityObject.GetPosition();
            }
            if (calculationPause == 0.0f)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(calculationPause);
            }
        }
    }

    void OrbitInterpolation(GravityObject orbitObject)
    {
        orbitObject.gravityObjectInterface.transform.position += orbitObject.GetCurrentImpuls() * Time.deltaTime / calculationPause;
    }

    

    private void Update()
    {
        float shiftSpeedCoef = 1;
        if (GameManager.Instance.GetInputManager().Shift())
        {
            shiftSpeedCoef = shiftTimeSpeedModifier;
        }
        calculationPause += (GameManager.Instance.GetInputManager().GetTimeInput() / 200) * shiftSpeedCoef;

        if(calculationPause<0)
        {
            calculationPause = 0.0f;
        }

        if (calculationPause > 0)
        {
            foreach (GravityObject gravityObject in gravityObjectsList)
            {
                OrbitInterpolation(gravityObject);
            }
        }

        /*if(Input.GetKey(KeyCode.Z))
        {
            ChangeGConstDynamicly(bigGConst - (2 * Time.deltaTime), true, ref gravityObjectsList);
        }
        if(Input.GetKey(KeyCode.X))
        {
            ChangeGConstDynamicly(bigGConst + (2 * Time.deltaTime), true, ref gravityObjectsList);
        }*/

    }

}
