using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

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
                model.TransitionToGameScene();
            }).AddTo(this);
    }
}
