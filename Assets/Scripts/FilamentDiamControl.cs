using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilamentDiamControl : MonoBehaviour
{
    //Slider
    public Slider diamSlider;
    public TMP_InputField ipText;
    public Text percentage;

    //Diameter min and max 
    public float minDiam = 0.2f;   // 0.2mm
    public float maxDiam = 3f;  // 3mm 

    //Diameter value
    [SerializeField] private float diameter;

    //LineGenerator Reference
    public GameObject LineGenerator;

    //NEW LINE OBJECT REFERENCE 
    public GameObject LineObject;

    // Start is called before the first frame update
    void Start()
    {
        diamSlider.value = 33f;
        diameter = minDiam;
    }

    // Update is called once per frame
    void Update()
    {
        diameter = diamSlider.value * (maxDiam / 100);
        percentage.text = diamSlider.value.ToString("0.0") + "%";
    }

    //Done on EDITOR Slider.OnValueChanged
    public void FillTextBox()
    {
        
        //Set textbox to diameter current value
        ipText.text = diameter.ToString("0.0"); //Format to decimal

        //NEW IMPLEMENTATION 
        LineObject.GetComponent<NewAnimateLine>().EndW = diameter;

    }

    //Done on EDITOR ipText.OnDeselect
    public void UpdateSlider()
    {
        //Set diameter to value set on TextBox
        float slideval = float.Parse(ipText.text, CultureInfo.InvariantCulture.NumberFormat);
        slideval = Mathf.Clamp(slideval, minDiam, maxDiam);
        diameter = slideval;
        
        //Set diameter slider to corresponding position
        diamSlider.SetValueWithoutNotify((slideval / maxDiam) * 100f);

        //NEW IMPLEMENTATION 
        LineObject.GetComponent<NewAnimateLine>().EndW = diameter;
    }
}