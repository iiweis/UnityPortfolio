using UnityEngine;

public class TitlePresenter : MonoBehaviour
{
    [SerializeField]
    private TitleModel model;

    [SerializeField]
    private TitleView view;

    private void Start()
    {
        view.PressEnterAction += async () =>
        {
            // フェードアウトしつつゲームシーンに遷移
            await view.PlayStartSoundAsync();
            await view.Fader.FadeOut(1f);
            model.TransitionToGameScene();
        };
    }
}