using System.Collections;
using UnityEngine;

public class TestCircleGenerator : MonoBehaviour
{
    //Declare circlePrefab and refPoints
    public GameObject circlePrefab;
    public Transform SpawnPoint;
    public Transform refPos;

    //Declare Animation parameters
    public float animDuration = 2f;
    public int nCircles;
    public float deltaZ;
    public float deltaR;

    //Circle width control
    private float circleWidth;
    public float CircleWidht
    {
        get { return circleWidth; }
        set { circleWidth = value; }
    }

    //Declare newSpawnPosition counter and radius
    private Vector3 newSpawnPosition;
    [SerializeField] private int counter;
    [SerializeField] private float radius = 10f;
    
    //public accesor change animation duration
    public float AnimDuration
    {
        get { return animDuration;}
        set { animDuration = value;} 
    }

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        circleWidth = 1f;
        newSpawnPosition = SpawnPoint.position;
        StartCoroutine(DelayedSpawn(animDuration));
    }

    // Update is called once per frame
    void Update()
    {
        //Ref point follows spawn point
        Vector3 refPoint = new Vector3(SpawnPoint.position.x, SpawnPoint.position.y + radius, SpawnPoint.position.z - deltaZ);
        refPos.position = refPoint;

        //When all circles are drawn direction is changed and radius incremented;
        if (counter >= nCircles)
        {
            deltaZ = - deltaZ;
            radius = radius + 1.25f; 
            counter = 0;
            StartCoroutine(DelayedSpawn(animDuration));
        }
    }

    IEnumerator DelayedSpawn(float animT)
    {
        if (counter >= nCircles)
            yield break;

        yield return new WaitForSeconds(animT);
        print("newCircleCalled");

        //Create a new circle Prefab
        GameObject currentCircle = Instantiate(circlePrefab, SpawnPoint.position, Quaternion.identity);
        
        //Change radius
        currentCircle.GetComponent<DrawAnimatedCircle>().Radius = radius;

        //Change width 
        currentCircle.GetComponent<LineRenderer>().startWidth = circleWidth;
        currentCircle.GetComponent<LineRenderer>().endWidth = circleWidth;
        
        //Call draw function and animation
        currentCircle.GetComponent<DrawAnimatedCircle>().GenerateCirclePoints();
        StartCoroutine(currentCircle.GetComponent<DrawAnimatedCircle>().AnimateCircle(animT));
        
        //Change next circle position
        newSpawnPosition = new Vector3(newSpawnPosition.x, newSpawnPosition.y, newSpawnPosition.z + deltaZ);
        SpawnPoint.position = newSpawnPosition;
        
        //Increment counter
        counter++;
        StartCoroutine(DelayedSpawn(animDuration));
    }
}
