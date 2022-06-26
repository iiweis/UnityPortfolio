using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TitlePresenter : MonoBehaviour
{
    [SerializeField]
    private TitleModel model;

    [SerializeField]
    private TitleView view;

    void Start()
    {
        view.UpdateAsObservable().Subscribe(async _ => await model.TransitionToGameScene());
    }
}
