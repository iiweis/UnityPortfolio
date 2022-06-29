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
        model.Result.Subscribe(value =>
        {
            if (value is bool result)
            {
                if (result)
                {
                    // ƒQ[ƒ€¬Œ÷
                }
                else
                {
                    // ƒQ[ƒ€Ž¸”s
                    dialogModel.IsActiveDialog.Value = true;
                }
            }
            model.IsStart.Value = false;
        }).AddTo(this);

        model.IsSlash.Subscribe(value =>
        {
            if (value)
            {
                view.Slash();
                model.IsStart.Value = false;
                dialogModel.IsActiveDialog.Value = true;
            }
        }).AddTo(this);

        model.IsStart.Subscribe(value => view.SetStartActive(value)).AddTo(this);
        model.Elapsed.Subscribe(value => view.SetElapsedText(value)).AddTo(this);

        model.TimeLimit.Subscribe(value =>
        {
            view.SetRemainingTimeSliderRange(0, value.Ticks);
            view.SetRemainingTimeSliderValue(value.Ticks);
        }).AddTo(this);

        model.RemainingTime.Subscribe(value =>
        {
            view.SetRemainingTimeSliderValue(value.Ticks);
        }).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
