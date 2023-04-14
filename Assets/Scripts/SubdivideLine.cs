using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubdivideLine : MonoBehaviour
{

    public Material myMaterial;

    //Circle Generator reference
    private GameObject circleGen;

    //Total number of segments per line
    public int nDivisions;

    public int NDivisions
    {
        get { return nDivisions; }
    }

    //Start and end width
    public float startWidth;
    public float endWidth;

    //StartWidth accesor
    public float StartWidth
    {
        get { return startWidth; }
        set { startWidth = value; }
    }

    //EndWidth accesor
    public float EndWidth
    {   get { return endWidth; } 
        set { endWidth= value; } 
    }

    //Lists
    [SerializeField] private List<GameObject> lineSegments; //List containing all line sub segments
    [SerializeField] private List<Vector3> pointPositions;  //List containing all point positions (subdivision)
    [SerializeField] private List<float> segmentLength;     //Length of every sub segment
    [SerializeField] private List<int> nSubSegments;        //number of sub segments each segment has

    //Interpolant
    private float interpolant;

    //Elapsed time
    [SerializeField] private float elapsedTime;
    private float endTime = 2f;

    //EndTime Accesor
    public float EndTime
    {
        get { return endTime; }
        set { endTime= value; }
    }

    //Segment counter for iterations
    [SerializeField] private int counter;

    //Every ReferencePoint inside the editor that makes up the line
    public Transform[] lineRefPoints;
    
    //Total Line length
    [SerializeField] private float totalDistance;
    
    //Total Line length accessor
    public float TotalDistance
    {
        get { return totalDistance; }
        set { totalDistance = value; }
    }

    //Moved flag and lines created flag
    private bool moved;
    private bool linesCreated;


    // Start is called before the first frame update
    void Start()
    {
        //Set initial values
        interpolant = 0f;
        counter = 0;
        totalDistance = 0f;
        moved = false;
        linesCreated = false;

        //Initialize circleGenerator variable
        circleGen = GameObject.FindGameObjectWithTag("CircleGenerator");

        StartCoroutine(CheckIfPointsMoved());
    }

    // Update is called once per frame
    void Update()
    {
        //Update line positions and distance in every frame
        UpdatePointPositions();
        if (Input.GetKeyDown(KeyCode.A))
        {
            // When "A" key is pressed redraw with new widths
            print("A key pressed");
            counter = 0;
            StartCoroutine(AnimateWidth());
        }
    }

    IEnumerator CheckIfPointsMoved()
    {
        for (int i = 0; i < lineRefPoints.Length; i++)
        {
            //For all points check if any one of them has moved
            Vector3 prevPos = lineRefPoints[i].position;
            yield return new WaitForSeconds(0.05f);
            if (!(Vector3.Equals(prevPos, lineRefPoints[i].position)))
                moved = true;
        }
        //Recursion
        StartCoroutine(CheckIfPointsMoved());
    }

    void UpdatePointPositions()
    {
        //If any of the points in the lone has moved, redraw the whole line renderer
        if (moved)
        {
            //Clear every list and parameter
            totalDistance = 0;
            segmentLength.Clear();
            nSubSegments.Clear();
            pointPositions.Clear();

            //Rebuild segmentLenght List
            for (int i = 1; i < lineRefPoints.Length; i++)
            {
                float segmentLen = Vector3.Distance(lineRefPoints[i].position, lineRefPoints[i - 1].position);
                segmentLength.Add(segmentLen);
                totalDistance += segmentLen;
            }

            //Rebuild nSubSegments List
            foreach (float length in segmentLength)
            {
                float segmentRatio = length / totalDistance;
                int nSubSeg = Mathf.CeilToInt(segmentRatio*nDivisions);
                nSubSegments.Add(nSubSeg);
            }

            //Call subdivide function
            NewSubdivide();
            
            //Set moved flag
            moved = false;
        }
    }

    void NewSubdivide()
    {
        //Iterate thorugh all line segments list
        for (int i = 1; i < lineRefPoints.Length; i++)
        {
            //To each line segment corresponds an interpolant given by:
            interpolant = (float)1 / nSubSegments[i - 1];

            //Iterate to generate all points that make up the sub segments of each line segment
            for (int j = 0; j < nSubSegments[i - 1]; j++)
            {
                float currentInterpolant = interpolant * j;
                //Interpolate by current interpolant to obtain a new point position
                Vector3 currentPos = Vector3.Lerp(lineRefPoints[i - 1].position, lineRefPoints[i].position, currentInterpolant);
                pointPositions.Add(currentPos);
            }
        }

        if (!linesCreated)
        {
            CreateLineSubSegments();
        }
        else
        {
            UpdateLineSubSegments();
        }
    }

    void CreateLineSubSegments()
    {
        //Set lines Created flag
        linesCreated = true;
        //Disable this object's fixed line renderer 
        this.GetComponent<LineRenderer>().enabled = false;

        //Iterate through all subsegment's generated points to create line sub segments
        for (int i = 1; i < pointPositions.Count; i++)
        {
            //Create object
            GameObject line = new GameObject("subSegment");
            
            //Attach script
            line.AddComponent<AnimateLineSegment>();
            line.transform.parent = this.transform;
            
            //Set line widht
            line.GetComponent<AnimateLineSegment>().StartWidth = startWidth;
            line.GetComponent<AnimateLineSegment>().EndWidth = startWidth;
            
            //Set start, end position and material
            line.GetComponent<AnimateLineSegment>().StartPos = pointPositions[i - 1];
            line.GetComponent<AnimateLineSegment>().EndPos = pointPositions[i];
            line.GetComponent<AnimateLineSegment>().MyMaterial = myMaterial;
            
            //Created line is added to line segments list
            lineSegments.Add(line);
        }
    }

    void UpdateLineSubSegments()
    {
        //Iterate through all line sub segments and update their positions accordingly
        int i = 1;
        foreach (GameObject line in lineSegments)
        {
            if(i < pointPositions.Count -1)
            {
                i++;
            }
            line.GetComponent<AnimateLineSegment>().StartPos = pointPositions[i - 1];
            line.GetComponent<AnimateLineSegment>().EndPos = pointPositions[i];
        }
    }

    //Couroutine that animates the line widht by changing the lines parameters. 
    public IEnumerator AnimateWidth()
    {
        if (counter > lineSegments.Count - 1)
        {
            circleGen.GetComponent<TestCircleGenerator>().CircleWidht = endWidth;
            yield break;
        }
        //Set T = 0 and endTime to run interpolation once more
        lineSegments[counter].GetComponent<AnimateLineSegment>().ElapsedTime = 0;
        lineSegments[counter].GetComponent<AnimateLineSegment>().EndTime = endTime;
        //Set new startWidth and endWidth
        lineSegments[counter].GetComponent<AnimateLineSegment>().StartWidth = startWidth;
        lineSegments[counter].GetComponent<AnimateLineSegment>().EndWidth = endWidth;
        //Delay between cycles
        yield return new WaitForSeconds(endTime / 8);
        counter++;
        StartCoroutine(AnimateWidth());
    }
}
