using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class AnimateLineSegment : MonoBehaviour
{
    LineRenderer lineRenderer;
    private Material myMaterial; //For color

    private Vector3 startPos = Vector3.zero; //line sub segment start
    private Vector3 endPos = Vector3.zero; //line sub segment end

    [SerializeField] private float elapsedTime; 
    private float endTime = 2f;
    private float startWidth = 0.1f;
    private float endWidth = 0.5f;

    //Accesor material
    public Material MyMaterial
    {
        get { return myMaterial; }
        set { myMaterial = value; }
    }

    //Accesor line start position
    public Vector3 StartPos
    {
        get { return startPos; }
        set { startPos = value; }
    }

    //Accesor line end position
    public Vector3 EndPos
    {
        get { return endPos; }
        set { endPos = value; }
    }

    //Accesor line elapsed time
    public float ElapsedTime
    {
        get { return elapsedTime; }
        set { elapsedTime = value; }
    }

    //Accesor end time 
    public float EndTime
    {
        get { return endTime; }
        set { endTime = value; }
    }

    //Accesor StartWidth
    public float StartWidth
    {
        get { return startWidth; }
        set { startWidth = value; }
    }

    //AccesorEndWith
    public float EndWidth
    {
        get { return endWidth; }
        set { endWidth = value; }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //Add and get lineRenderer, set material
        this.AddComponent<LineRenderer>();
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.material = myMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        //Update line start and end accordingly
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
        AnimateWidth();
    }

    void AnimateWidth()
    {
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / endTime;
        //Interpolate by t  
        float lineWidth = Mathf.Lerp(startWidth, endWidth, percentageComplete);
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }
}
