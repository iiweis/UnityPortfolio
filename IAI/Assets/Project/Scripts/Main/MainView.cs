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

    [SerializeField]
    private Fader fader;

    public Fader Fader => fader;

    [SerializeField]
    private GameObject waraNormal;

    [SerializeField]
    private GameObject waraCutted;

    private async void Start()
    {
        SetLevel(GameManager.Instance.Level);
        await fader.FadeIn();
    }

    public void Slash()
    {
        Debug.DrawLine(target.transform.position, cut.transform.position);

        waraNormal.SetActive(false);
        waraCutted.SetActive(true);
        //(GameObject copyNormalside, _) = MeshCutter.Cut(target, target.transform.position, cut.transform.position);
        
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
}
