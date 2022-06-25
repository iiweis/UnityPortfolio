using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class MainPresenter : MonoBehaviour
{
    [SerializeField]
    private MainView view;

    [SerializeField]
    private MainModel model;

    [SerializeField]
    private ResultDialogModel dialogModel;

    // Start is called before the first frame update
    void Start()
    {
        view.UpdateAsObservable().Subscribe(_ =>
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                view.Slash();
                dialogModel.IsActiveDialog.Value = true;
            };
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
