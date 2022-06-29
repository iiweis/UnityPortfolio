using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private GameObject cut;

    [SerializeField]
    private GameObject info;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private GameObject exclamation;

    [SerializeField]
    private TextMeshProUGUI elapsedText;

    [SerializeField]
    private Slider remainingTimeSlider;

    public void Slash()
    {
        Debug.DrawLine(target.transform.position, cut.transform.position);

        (GameObject copyNormalside, _) = MeshCutter.Cut(target, target.transform.position, cut.transform.position);
        
        //copyNormalside.transform.position = target.transform.position;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public void SetInfoActive(bool value) => info.SetActive(value);
    public void SetStartActive(bool value)
    {
        exclamation.SetActive(value);
        elapsedText.gameObject.SetActive(value);
        remainingTimeSlider.gameObject.SetActive(value); 
    }

    public void SetLevel(int value) => levelText.text = $"Level : {value}";

    public void SetElapsedText(System.TimeSpan elapsed) => elapsedText.text = $"{elapsed:ss\\:fff}s";

    public void SetRemainingTimeSliderValue(float value) => remainingTimeSlider.value = value;
    public void SetRemainingTimeSliderRange(float minValue, float maxValue)
    {
        remainingTimeSlider.minValue = minValue;
        remainingTimeSlider.maxValue = maxValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
