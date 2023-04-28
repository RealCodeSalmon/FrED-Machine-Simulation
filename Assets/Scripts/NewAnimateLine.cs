using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class NewAnimateLine : MonoBehaviour
{
    //Line Positions
    public Transform[] LinePoints;
    
    //Line Renderers
    public LineRenderer[] LineSegments;

    //Line Parameters
    public float startWidth = 0.1f;
    public float endWidth = 0.5f;
    public float endW = 1f; 

    public float StartWidth
    {
        get { return startWidth; }
        set { startWidth = value; }
    }

    public float EndWidth
    {
        get { return endWidth; }
        set { endWidth = value; }
    }

    public float EndW
    {
        get { return endW; }
        set { endW = value; }
    }

    //Width update delay
    public float delay = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AnimateLine(startWidth, endWidth));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)) 
        {
            print("KeyPressed");
            startWidth = endWidth;
            endWidth = endW;
            StartCoroutine(AnimateLine(startWidth, endW));
        }

        UpdateLinePosition();
    }

    private IEnumerator AnimateLine(float startW, float endW)
    {
        for (int i = 0; i < LineSegments.Length; i++)
        {
            StartCoroutine(MoveStartWidth(LineSegments[i], 10,startW, endW));
            yield return new WaitForSeconds(delay*10/4);
            StartCoroutine(MoveEndWidth(LineSegments[i], 10, startW, endW));
        }
    }

    private IEnumerator MoveStartWidth(LineRenderer line, int counts, float startW, float endW)
    {
        float newWidth;
        for (int i = 0; i <= counts-1; i++)
        {
            newWidth = startW + (i * (endW - startW) / counts);
            line.startWidth = newWidth;
            print("i = " + i + " start width = " + newWidth);
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator MoveEndWidth(LineRenderer line, int counts, float startW, float endW)
    {
        float newWidth;
        for (int i = 0; i <= counts - 1; i++)
        {
            newWidth = startW + (i * (endW - startW) / counts);
            line.endWidth = newWidth;
            print("i = " + i + " end width = " + newWidth);
            yield return new WaitForSeconds(delay);
        }
    }

    private void UpdateLinePosition()
    {
        for (int i = 0; i< LineSegments.Length; i++)
        {
            LineSegments[i].SetPosition(0, LinePoints[i].position);
            LineSegments[i].SetPosition(1, LinePoints[i+1].position);
        }
    } 
}
