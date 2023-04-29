using UnityEngine;

public class LinealGuideScript : MonoBehaviour
{
    private GameObject refSphere;

    // Start is called before the first frame update
    void Start()
    {
        refSphere = GameObject.FindGameObjectWithTag("Sphere");
    }

    // Update is called once per frame
    void Update()
    {
        //Mirror the sphere z position on the linear guide 
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, refSphere.transform.position.z);
    }
}
