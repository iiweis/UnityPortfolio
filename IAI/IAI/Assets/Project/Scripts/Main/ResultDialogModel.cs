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

    public async Task TransitionToTitleScene()
    {
        await SceneManager.LoadSceneAsync(SceneNames.Title);
    }

    public async Task ReloadMainScene() => await SceneManager.LoadSceneAsync(SceneNames.Main);
}
