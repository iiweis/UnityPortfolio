using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class MainPresenter : MonoBehaviour
{
    [SerializeField]
    private MainView view;

    [SerializeField]
    private ResultDialogView dialogView;

    [SerializeField]
    private MainModel model;

    private void Start()
    {
        ConfigureView();
        ConfigureModel();
    }

    private void ConfigureView()
    {
        // Spaceキーを押したとき
        view.PressSpaceAction += () => model.Reaction();

        // リザルトダイアログのBackボタンを押したとき
        dialogView.ClickBackButtonAction += async () =>
        {
            await view.PlayButtonClickSound();
            await view.Fader.FadeOut(1f);
            model.TransitionToTitleScene();
        };

        // リザルトダイアログのPlayagainボタンを押したとき
        dialogView.ClickPlayAgainButtonAction += async () =>
        {
            await view.PlayButtonClickSound();
            await view.Fader.FadeOut(1f);
            model.PlayAgain();
        };
    }

    private void ConfigureModel()
    {
        // ゲームの結果が出たとき
        model.ResultState.Where(s => s != ResultState.None).
            SubscribeWithState(this, async (s, myself) =>
            {
                if (s == ResultState.Success)
                {
                    var _ = view.PlaySlashSoundAsync();
                    view.Slash();
                    await UniTask.Delay(2000);

                    if (GameManager.Instance.LevelUp() == GameManager.MaxLevel)
                    {
                        // 最大レベルに到達しているのでリザルトダイアログを表示してゲーム終了
                        view.HideUI();
                        dialogView.ShowDialog();
                    }
                    else
                    {
                        // 次のゲームへ
                        await view.Fader.FadeOut(1f);
                        model.ReloadScene();
                    }
                }
                else
                {
                    // ゲーム失敗なのでリザルトダイアログを表示してゲーム終了
                    var _ = view.PlayBeepSoundAsync();
                    view.HideUI();
                    dialogView.ShowDialog();
                }
            }).AddTo(this);

        // 制限時間の設定
        model.TimeLimit.SubscribeWithState(this, (value, myself) => myself.view.SetRemainingTimeSlider(minValue: 0, maxValue: value.Ticks, value: value.Ticks)).AddTo(this);

        // 経過時間の設定
        model.Elapsed.SubscribeWithState(this, (value, myself) => myself.view.SetElapsed(value)).AddTo(this);

        // 残り時間の設定
        model.RemainingTime.SubscribeWithState(this, (value, myself) => myself.view.SetRemainingTimeSliderValue(value.Ticks)).AddTo(this);

        // ゲーム開始時
        model.IsStart.Where(value => value).SubscribeWithState(this, (_, myself) =>
        {
            var __ = myself.view.PlayAlarmSoundAsync();
            myself.view.SetExclamationActive(true);
            myself.view.SetElapsedTextActive(true);
            myself.view.SetRemainingTimeSliderActive(true);
        }).AddTo(this);
    }
}