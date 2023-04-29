using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LineGuideVelocityTest : MonoBehaviour
{
    private Transform tf;
    public Transform startP;
    public Transform endP;
    
    private float previousPosZ;
    [SerializeField]private float reelDis;

    private int velIter;
    [SerializeField]private List<float> velocityList;
    private float popVelocity;
    private bool sampleReady;

    private GameObject CircleGenerator;

    // Start is called before the first frame update
    void Start()
    {
        CircleGenerator = GameObject.FindGameObjectWithTag("CircleGenerator");
        tf = this.GetComponent<Transform>();
        previousPosZ = tf.position.z;
        sampleReady = false;
        reelDis = Vector3.Distance(startP.position, endP.position);
        StartCoroutine(SampleVelocity());
    }

    // Update is called once per frame
    private void Update()
    {
        if (sampleReady)
        {
            print(popVelocity);
            float Ttime = reelDis / popVelocity;
            CircleGenerator.GetComponent<TestCircleGenerator>().AnimDuration = (float)Ttime / 38;
        }
    }

    //This will be used to determine the velocity of the line guide on model simulation

    IEnumerator SampleVelocity()
    {
        yield return new WaitForSeconds(0.08f);
        float currentPositionZ = tf.position.z;
        float velocity = Mathf.Abs(currentPositionZ - previousPosZ) / 0.08f;
        previousPosZ = currentPositionZ;
        velocityList.Add(velocity);

        if (velocityList.Count > 100)
        {
            int equalCount = 0;
            int popCount = 0;
            int popIndex = 0;

            for (int i = 0; i< velocityList.Count; i++)
            {
                for(int j = 0; j< velocityList.Count; j++)
                {
                    if (CompareOnThreshold(velocityList[j], velocityList[i]))
                    {
                        equalCount++;
                        if(equalCount > popCount)
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

            popVelocity = velocityList[popIndex];
            sampleReady = true;
            
            velocityList.Remove(velocityList[0]);
        }
        StartCoroutine(SampleVelocity());
    }

    private bool CompareOnThreshold(float n1, float n2)
    {
        if(n1 > n2 - 0.1 || n1 < n2 + 0.1)
        {
            return true;
        }
        return false;
    }
}



