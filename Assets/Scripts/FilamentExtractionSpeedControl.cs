using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilamentExtractionSpeedControl : MonoBehaviour
{
    //Extraction Speed needs to be measured using the total line length and line animation duration

    //Slider
    public Slider exSpeedSlider;
    public TMP_InputField ipText;
    public Text percentage;

    //Speed min and max 
    public float minSpeed = 0f;   // 0 mm/s
    public float maxSpeed = 5f;  // 5 mm/s 

    //Speed value
    [SerializeField] private float exSpeed;

    // Start is called before the first frame update
    void Start()
    {
        exSpeedSlider.value = minSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        exSpeed = exSpeedSlider.value * (maxSpeed / 100);
    }

    //Done on EDITOR Slider.OnValueChanged
    public void FillTextBox()
    {
        ipText.text = exSpeed.ToString("0.0"); //Format to decimal
    }

    //Done on EDITOR ipText.OnDeselect
    public void UpdateSlider()
    {
        //Set diameter to value set on TextBox
        float slideval = float.Parse(ipText.text, CultureInfo.InvariantCulture.NumberFormat);
        slideval = Mathf.Clamp(slideval, minSpeed, maxSpeed);
        exSpeed = slideval;
        exSpeedSlider.SetValueWithoutNotify((slideval / maxSpeed) * 100f);
    }
}