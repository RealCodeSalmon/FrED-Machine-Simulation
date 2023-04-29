using System;
using System.Collections;
using UnityEngine;

public class NewAnimateLine : MonoBehaviour
{
    //Original LineReference
    public GameObject ogLine;

    //CircleGenerator Reference;
    public GameObject cirGen;

    //ControlsManagerReference
    public GameObject ControlsManager;
    private float tanVel;

    //Line Positions
    public Transform[] LinePoints;
    
    //Line Renderers
    public LineRenderer[] LineSegments;

    //Line Segment Distances
    [SerializeField] private float[] SegmDelays = new float[12];

    //Line Parameters
    public float startWidth = 0.1f;
    public float endWidth = 0.5f;
    public float endW = 1f;
    public int incrementCounts = 10;
    public float corrFactor = 0.2f;
    private bool done1 = false;
    private bool done2 = false;

    public float EndW
    {
        get { return endW; }
        set { endW = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        ogLine.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLinePosition();

        tanVel = ControlsManager.GetComponent<FilamentSpoolingControl>().TanVelocity;
        FindSegmentDelay(tanVel);

        if (Input.GetKeyDown(KeyCode.F)) 
        {
            print("KeyPressed");
            startWidth = endWidth;
            endWidth = endW;
            StartCoroutine(AnimateLine(startWidth, endW));
        }
    }

    private IEnumerator AnimateLine(float startW, float endW)
    {
        for (int i = 0; i < LineSegments.Length; i++)
        {
            done1 = false;
            StartCoroutine(MoveStartWidth(LineSegments[i], incrementCounts,startW, endW, SegmDelays[i]));
            yield return new WaitUntil(() => done1 == true);
            done2 = false;
            StartCoroutine(MoveEndWidth(LineSegments[i], incrementCounts, startW, endW, SegmDelays[i]));
            yield return new WaitUntil(() => done2 == true);
        }
        cirGen.GetComponent<TestCircleGenerator>().CircleWidht = endW;
    }

    private IEnumerator MoveStartWidth(LineRenderer line, int counts, float startW, float endW, float delay)
    {
        float newWidth;
        int intrptIndex = Mathf.CeilToInt(counts / 2);
        for (int i = 0; i <= counts-1; i++)
        {
            newWidth = startW + (i * (endW - startW) / counts);
            line.startWidth = newWidth;
            print("i = " + i + " start width = " + newWidth);
            if(i == intrptIndex)
            {
                done1= true;
            }
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator MoveEndWidth(LineRenderer line, int counts, float startW, float endW, float delay)
    {
        float newWidth;
        for (int i = 0; i <= counts - 1; i++)
        {
            newWidth = startW + (i * (endW - startW) / counts);
            line.endWidth = newWidth;
            print("i = " + i + " end width = " + newWidth);
            yield return new WaitForSeconds(delay);
        }
        done2= true;
    }

    private void UpdateLinePosition()
    {
        for (int i = 0; i< LineSegments.Length; i++)
        {
            LineSegments[i].SetPosition(0, LinePoints[i].position);
            LineSegments[i].SetPosition(1, LinePoints[i+1].position);
        }
    } 

    private void FindSegmentDelay(float vel)
    {
        float currDelay;
        double totalDistance = 483.1686;
        float totalTime = Convert.ToSingle(totalDistance / vel);

        for (int i = 0; i < SegmDelays.Length; i++)
        {
            currDelay = Convert.ToSingle(Vector3.Distance(LinePoints[i].position, LinePoints[i+1].position)/totalDistance);
            SegmDelays[i] = (currDelay/(incrementCounts - 1)) * totalTime * corrFactor;
        }
    }

}
