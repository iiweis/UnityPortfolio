using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultDialogModel : MonoBehaviour
{
    public BoolReactiveProperty IsActiveDialog { get; } = new BoolReactiveProperty();

    public void TransitionToTitleScene() => SceneManager.LoadScene(SceneNames.Title);

    public void ReloadMainScene() => SceneManager.LoadScene(SceneNames.Main);
}
