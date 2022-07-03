using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityRandom = UnityEngine.Random;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class MainModel : MonoBehaviour
{
    private readonly BoolReactiveProperty isStart = new BoolReactiveProperty();
    private readonly ReactiveProperty<TimeSpan> elapsed = new ReactiveProperty<TimeSpan>();
    private readonly ReactiveProperty<TimeSpan> remainingTime = new ReactiveProperty<TimeSpan>();
    private readonly ReactiveProperty<ResultState> resultState = new ReactiveProperty<ResultState>(global::ResultState.None);
    private readonly ReactiveProperty<TimeSpan> timeLimit = new ReactiveProperty<TimeSpan>();

    private IDisposable intervalOvervable;

    /// <summary>
    /// ゲームが開始しているかどうか
    /// </summary>
    public IReactiveProperty<bool> IsStart => isStart;

    /// <summary>
    /// ゲーム開始からの経過時間
    /// </summary>
    public IReadOnlyReactiveProperty<TimeSpan> Elapsed => elapsed;

    /// <summary>
    /// 残り時間
    /// </summary>
    public IReactiveProperty<TimeSpan> RemainingTime => remainingTime;

    /// <summary>
    /// 結果
    /// </summary>
    public IReactiveProperty<ResultState> ResultState => resultState;

    /// <summary>
    /// 制限時間
    /// </summary>
    public IReadOnlyReactiveProperty<TimeSpan> TimeLimit => timeLimit;

    private void Start()
    {
        // 5～15秒待機してからゲーム開始
        TimeSpan dueTime = TimeSpan.FromSeconds(5 + UnityRandom.Range(0, 10 + 1));
        Observable.Timer(dueTime).SubscribeWithState(this, (_, myself) =>
        {
            if (resultState.Value != global::ResultState.None)
            {
                // 既にゲームが終わっている場合は何もしない
            }

            // ゲーム開始を通知
            myself.isStart.Value = true;
        }, myself =>
        {
            if (myself.isStart.Value)
            {
                // ゲーム開始
                GameManager gameManager = GameManager.Instance;

                // 制限時間設定
                TimeSpan timeLimit = gameManager.GetTimeLimit(gameManager.Level);
                myself.timeLimit.Value = timeLimit;
                myself.remainingTime.Value = timeLimit;

                // 時間計測用のObservableを発行
                myself.intervalOvervable = Observable.TimeInterval(Observable.EveryUpdate()).SubscribeWithState(this, (value, myself) =>
                {
                    // 経過時間は加算
                    TimeSpan elapsed = myself.elapsed.Value + value.Interval;
                    if (elapsed > myself.timeLimit.Value)
                    {
                        // 制限時間を超えていたら制限時間に置き換え
                        elapsed = myself.timeLimit.Value;
                    }
                    myself.elapsed.Value = elapsed;

                    // 残り時間は減算
                    TimeSpan remainingTime = myself.remainingTime.Value - value.Interval;
                    if (remainingTime < TimeSpan.Zero)
                    {
                        // 0秒を下回っていたら0秒に置き換え
                        remainingTime = TimeSpan.Zero;
                    }
                    myself.remainingTime.Value = remainingTime;

                    // 制限時間切れならゲーム終了
                    if (remainingTime == TimeSpan.Zero)
                    {
                        myself.resultState.Value = global::ResultState.Failure;
                    }
                }).AddTo(this);
            }
        }).AddTo(this);

        isStart.SubscribeWithState(this, (value, myself) =>
        {
            if (!value)
            {
                // ゲーム終了後は経過時間計測用のObservableは必要ないので破棄
                myself.intervalOvervable?.Dispose();
            }
        }).AddTo(this);
    }

    /// <summary>
    /// シーンをリロードする。
    /// </summary>
    public void ReloadScene() => SceneManager.LoadScene(SceneNames.Main);

    /// <summary>
    /// タイトルシーンに遷移する。
    /// </summary>
    public void TransitionToTitleScene() => SceneManager.LoadScene(SceneNames.Title);

    /// <summary>
    /// レベルなどをリセットし、ゲームを初めからプレイし直す。
    /// </summary>
    public void PlayAgain()
    {
        GameManager.Instance.Reset();
        ReloadScene();
    }

    /// <summary>
    /// 反応する。
    /// </summary>
    public void Reaction()
    {
        if (resultState.Value != global::ResultState.None)
        {
            // 既にゲーム終了していたら何もしない
            return;
        }

        if (intervalOvervable is null)
        {
            // 時間計測が始まっていないときにボタンを押していたらゲーム失敗
            resultState.Value = global::ResultState.Failure;
            return;
        }

        // 残り時間より前に反応出来ていたらゲーム成功
        bool success = remainingTime.Value.Ticks >= 0;
        if (success)
        {
            // ベストタイムを記録する
            GameManager.Instance.UpdateBestTime(elapsed.Value);
        }

        // ゲーム結果通知
        resultState.Value = success ? global::ResultState.Success : global::ResultState.Failure;

        // ゲームは終了したので経過時間計測用のObservableは破棄
        intervalOvervable?.Dispose();
    }
}