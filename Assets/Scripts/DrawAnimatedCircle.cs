using System.Collections;
using UnityEngine;

public class DrawAnimatedCircle : MonoBehaviour
{
    //Circle Line Renderer
    private LineRenderer circleRenderer;
    
    //Points for each circle
    private Vector3[] rendererPositions;
    private int pointCount;
    
    //Circle Radius
    private float radius = 10f; 
    
    //Radius accesor
    public float Radius
    {
        get { return radius; }
        set { radius = value; }
    }

    //Function is called from TestCircleGenerator Script
    //Generate all the points that make up a circle
    public void GenerateCirclePoints()
    {
        circleRenderer = GetComponent<LineRenderer>();
        circleRenderer.loop = true;

        //Draw a circle with 50 steps and r = radius
        DrawCircle(50, radius);

        //Initialize renderPositions array with n = pountCount elements
        //renderPositions is used for circle animation
        pointCount = circleRenderer.positionCount;
        rendererPositions = new Vector3[(int)pointCount];

        //Copy each circleRenderer position to renderer positions array
        for (int i = 0; i < pointCount; i++)
        {
            rendererPositions[i] = circleRenderer.GetPosition(i);
        }
    }

    //Function is called from TestCircleGenerator Script
    //Draws a circle of r = radius by setting n=steps positions of circle line renderer
    void DrawCircle(int steps, float radius)
    {
        circleRenderer.positionCount = steps+1;

        for(int i = 0; i < steps; i++)
        {
            //Get the radians for current step 
            float progress =(float) i/ steps;
            float radians = (progress * 2 * Mathf.PI) + (Mathf.PI/2);
            
            //Calculate x and y components
            float xComp = radius * Mathf.Cos(radians);
            float yComp = radius * Mathf.Sin(radians);

            //Set the Renderer current position with calculated values
            Vector3 currentPosition = new Vector3(xComp, yComp, 0);
            circleRenderer.SetPosition(i, currentPosition);
        }
    }

    //Function is called from TestCircleGenerator Script
    //Function that handles the animaton of each individual segment that makes up the circle.
    public IEnumerator AnimateCircle(float animationDuration)
    {
        float segmentDuration = animationDuration / pointCount;

        for(int i = 0; i < pointCount -1;i++)
        {
            //Get start time
            float startTime = Time.time;

            //Set start position for lie, set end position for line
            Vector3 startPos = rendererPositions[i];
            Vector3 endPos = rendererPositions[i+1];

            Vector3 pos = startPos;
            
            while (pos != endPos)
            {
                //elapsed time
                float t = (Time.time - startTime)/segmentDuration;

                //Draw a line from start position to end position using interpolation
                pos = Vector3.Lerp(startPos, endPos, t);

                for(int j = i+1; j < pointCount; j++)
                    circleRenderer.SetPosition(j,pos);

                yield return null;
            }
        }
    }
}
