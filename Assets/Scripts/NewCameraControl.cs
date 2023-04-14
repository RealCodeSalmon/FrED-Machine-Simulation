using UnityEngine;

public class NewCameraControl : MonoBehaviour
{
    // Start is called before the first frame update

    private Transform camTransform;
    private Transform pivotTransform;

    private Vector3 camRotation;
    private float camDistance = 490f;

    public float mouseSensitivity = 4f;
    public float scrollSensitivity = 2f;

    public float OrbitSpeed = 10f;
    public float scrollSpeed = 6f;

    public bool camDisabled = false;

    void Start()
    {
        camTransform = this.transform;
        pivotTransform = this.transform.parent;
        
    }

    // Update is called once per frame
    private void LateUpdate()
    {

        //Left Shift Key disables and enables the camera controls

        if (Input.GetMouseButton(2))
        {
            //Camera rotation based on mouse position
            if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                camRotation.x += Input.GetAxis("Mouse X") * mouseSensitivity;
                camRotation.y -= Input.GetAxis("Mouse Y") * mouseSensitivity;

                //Limit the rotation so camera does not go over the top
                camRotation.y = Mathf.Clamp(camRotation.y, 0f, 90f);
            }
        }

        //Zoom in and out using scroll wheel
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;

            //As the camera gets closer to the model the zoom speed reduces
            scrollAmount *= (this.camDistance * 0.3f);
            this.camDistance += scrollAmount * -1f;

            //Limit the distance between the object and the camera
            this.camDistance = Mathf.Clamp(this.camDistance, 10f, 2000f);
        }

        //Set camera rotation and distance
        Quaternion QT = Quaternion.Euler(camRotation.y, camRotation.x, 0);
        this.pivotTransform.rotation = Quaternion.Lerp(this.pivotTransform.rotation, QT, Time.deltaTime * OrbitSpeed);

        //Only if changes on the zoom have occurred via input we update the camera distance 
        if(this.camTransform.localPosition.z != this.camDistance * -1f)
        {
            this.camTransform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this.camTransform.localPosition.z, this.camDistance * -1f, Time.deltaTime * scrollSpeed));
        }
    }
}
