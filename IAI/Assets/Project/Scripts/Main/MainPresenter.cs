using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

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
        model.Result.Subscribe(async value =>
        {
            if (value is bool result)
            {
                if (result)
                {
                    // ゲーム成功
                    await UniTask.Delay(2000);
                    await view.Fader.FadeOut(1f);
                    model.ReloadMainScene();
                }
                else
                {
                    // ゲーム失敗
                    dialogModel.IsActiveDialog.Value = true;
                }
            }
            model.IsStart.Value = false;
        }).AddTo(this);

        model.IsSlash.Subscribe(value =>
        {
            if (value)
            {
                model.Result.Value = (model.TimeLimit.Value - model.Elapsed.Value).Ticks >= 0;
                model.IsStart.Value = false;
                view.Slash();
                //dialogModel.IsActiveDialog.Value = true;
            }
        }).AddTo(this);

        model.IsStart.Subscribe(value =>
        {
            if (!model.Result.Value.HasValue)
            {
                view.SetStartActive(value);
            }
        }).AddTo(this);
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
