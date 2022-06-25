using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ResultDialogPresenter : MonoBehaviour
{
    [SerializeField]
    private ResultDialogView view;

    [SerializeField]
    private ResultDialogModel model;

    private void Start()
    {
        view.BackButton.OnClickAsObservable().Subscribe(async _ =>
        {
            await model.TransitionToTitleScene();
        }).AddTo(this);

        view.TryAgainButton.OnClickAsObservable().Subscribe(async _ =>
        {
            await model.ReloadMainScene();
        }).AddTo(this);

        model.IsActiveDialog.Subscribe(value =>
        {
            view.DialogContainer.SetActive(value);
        }).AddTo(this);
    }
}
