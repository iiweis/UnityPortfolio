using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleModel : MonoBehaviour
{
    private BoolReactiveProperty press = new BoolReactiveProperty();

    public IReadOnlyReactiveProperty<bool> Press => press;

    private void Start()
    {
        Observable.EveryUpdate().
            Where(_ => Input.GetKey(KeyCode.Return)).
            Subscribe(_ => {
              press.Value = true;
            }).AddTo(this);
    }

    public async Task TransitionToGameScene()
    {
        await SceneManager.LoadSceneAsync(SceneNames.Main);
    }
}
