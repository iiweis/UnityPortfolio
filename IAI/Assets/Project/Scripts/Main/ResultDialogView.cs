using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultDialogView : MonoBehaviour
{
    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Button tryAgainButton;

    [SerializeField]
    private GameObject dialogContainer;

    [SerializeField]
    private TextMeshProUGUI resultTimeText;

    [SerializeField]
    private Fader fader;

    public Button BackButton => backButton;
    public Button TryAgainButton => tryAgainButton;
    public GameObject DialogContainer => dialogContainer;
    public TextMeshProUGUI ResultTimeText => resultTimeText;
    public Fader Fader => fader;

    public void SetResultTime(System.TimeSpan value) => resultTimeText.text = value.ToString(@"ss\:fff");
}
