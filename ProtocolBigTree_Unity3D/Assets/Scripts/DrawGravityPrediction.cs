using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DrawGravityPrediction : MonoBehaviour
{
    public GravityManager gravityManager;
    public GravityObjectInterface gravityObject;
    LineRenderer orbitLineRenderer;

    public Material LineMat;
    public Color colorLine;

    [SerializeField]
    private bool staticOrbit = false; // draw orbit once 

    [SerializeField]
    private bool drawCollisions = false;

    [SerializeField]
    private int Steps = 1; // number of steps per one calculation call 

    [SerializeField]
    private int lineThickness = 10;

    private GravityObject collisionObject;
    public Material collisionObjectMat; 

    private void Start()
    {


        GameObject drawObjectChild = new GameObject();
        drawObjectChild.transform.SetParent(transform);

        orbitLineRenderer = drawObjectChild.AddComponent<LineRenderer>();
        orbitLineRenderer.material = LineMat;
        orbitLineRenderer.startColor = colorLine;
        orbitLineRenderer.endColor = colorLine;
        orbitLineRenderer.positionCount = Steps;
        orbitLineRenderer.widthMultiplier = lineThickness; 

        if(colorLine.a==0)
        {
            colorLine.a = 1.0f;
        }


        StartCoroutine(DrawOrbit());

    }

   
    IEnumerator DrawOrbit()
    {
        GameObject collisionSphere = null;
        while (true)
        {
            int numberPoints = Steps;

            Vector3[] points = gravityManager.PredictOrbit(gravityObject.GetGravityObject(), Steps, drawCollisions , out collisionObject).ToArray();
            orbitLineRenderer.positionCount = points.Length;

            if (collisionObject != null)
            {
                if (collisionSphere == null)
                {
                    collisionSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    collisionSphere.transform.localScale = Vector3.one * collisionObject.GetRadius() * 2f;
                    collisionSphere.transform.position = collisionObject.GetPosition();
                    collisionSphere.GetComponent<Renderer>().material = collisionObjectMat;
                }
                else
                {
                    collisionSphere.transform.position = collisionObject.GetPosition();
                }
            }

            if(collisionObject == null && collisionSphere != null)
            {
                Destroy(collisionSphere);
            }

            orbitLineRenderer.SetPositions(points);
            /*for (int i = 0; i < numberPoints; i++)
            {
                orbitLineRenderer.SetPosition(i, points[i* resolutionStep]); 
            }*/

            if (staticOrbit)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
            }

        }
    }

}
