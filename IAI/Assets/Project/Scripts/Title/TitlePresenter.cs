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
        model.Press.Where(value => value).
            Subscribe(async _ =>
            {
                await view.PlayStartSound();
                await view.Fader.FadeOut(1f);
                await model.TransitionToGameScene();
            }).AddTo(this);
    }
}
