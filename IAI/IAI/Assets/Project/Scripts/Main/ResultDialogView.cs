using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultDialogView : MonoBehaviour
{
    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Button tryAgainButton;

    [SerializeField]
    private GameObject dialogContainer;

    public Button BackButton => backButton;
    public Button TryAgainButton => tryAgainButton;
    public GameObject DialogContainer => dialogContainer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
