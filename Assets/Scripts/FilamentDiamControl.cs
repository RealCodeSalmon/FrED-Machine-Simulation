using System.Collections;
using System.Collections.Generic;
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
        //Setup newStartWidth and EndWidth 
        float newStartWidth = LineGenerator.GetComponent<SubdivideLine>().EndWidth;
        LineGenerator.GetComponent<SubdivideLine>().StartWidth = newStartWidth;
        LineGenerator.GetComponent<SubdivideLine>().EndWidth = diameter;
        LineGenerator.GetComponent<SubdivideLine>().EndTime = this.GetComponent<FilamentSpoolingControl>().SegmentTime;
        
        //Set textbox to diameter current value
        ipText.text = diameter.ToString("0.0"); //Format to decimal
    }

    //Done on EDITOR ipText.OnDeselect
    public void UpdateSlider()
    {
        //Set diameter to value set on TextBox
        float slideval = float.Parse(ipText.text, CultureInfo.InvariantCulture.NumberFormat);
        slideval = Mathf.Clamp(slideval, minDiam, maxDiam);
        diameter = slideval;
        
        //Setup newStartWidth and EndWidth 
        float newStartWidth = LineGenerator.GetComponent<SubdivideLine>().EndWidth;
        LineGenerator.GetComponent<SubdivideLine>().StartWidth = newStartWidth;
        LineGenerator.GetComponent<SubdivideLine>().EndWidth = diameter;
        LineGenerator.GetComponent<SubdivideLine>().EndTime = this.GetComponent<FilamentSpoolingControl>().SegmentTime;

        diamSlider.SetValueWithoutNotify((slideval / maxDiam) * 100f);
    }
}