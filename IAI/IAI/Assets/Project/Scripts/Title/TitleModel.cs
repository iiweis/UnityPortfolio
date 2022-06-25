using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleModel : MonoBehaviour
{
    public async Task TransitionToGameScene()
    {
        await SceneManager.LoadSceneAsync(SceneNames.Main);
    }
}