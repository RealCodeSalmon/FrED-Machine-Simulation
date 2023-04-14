using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSphereVelocity : MonoBehaviour
{
    private Rigidbody rb;
    private float zVelocity;

    public Transform startP;
    public Transform endP;
    private float reelDis;

    public GameObject CircleGenerator;

    [SerializeField]private List<float> velList;
    private float popVelocity;
    private bool sampleReady;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        sampleReady = false;
        reelDis = Vector3.Distance(startP.position, endP.position);
    }

    // Update is called once per frame
    void Update()
    {
        zVelocity = Mathf.Abs(rb.velocity.z);
        velList.Add(zVelocity);
        
        if (velList.Count > 200)
        {
            SampleVelocities();
            sampleReady = true;
            velList.Clear();
        }

        if (sampleReady)
        {
            print("popular Velocity = " + popVelocity);
            float Ttime = reelDis / popVelocity;
            CircleGenerator.GetComponent<TestCircleGenerator>().AnimDuration = (float)Ttime / 38;
            sampleReady = false;
        }

    }

    void SampleVelocities()
    {
        int equalCount = 0;
        int popCount = 0;
        int popIndex = 0;

        for (int i = 0; i < velList.Count; i++)
        {
            for (int j = 0; j < velList.Count; j++)
            {
                if (CompareOnThreshold(velList[j], velList[i]))
                {
                    equalCount++;
                    if (equalCount > popCount)
                    {
                        popCount = equalCount;
                        popIndex = i;
                        if (i != 0)
                        {
                            break;
                        }
                    }
                }
            }
        }
        popVelocity = velList[popIndex];
    }

    private bool CompareOnThreshold(float n1, float n2)
    {
        if (n1 > n2 - 0.05 || n1 < n2 + 0.05)
        {
            return true;
        }
        return false;
    }
}
