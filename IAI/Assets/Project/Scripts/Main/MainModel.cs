using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityRandom = UnityEngine.Random;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class MainModel : MonoBehaviour
{
    private readonly BoolReactiveProperty isStart = new BoolReactiveProperty();
    private readonly ReactiveProperty<TimeSpan> elapsed = new ReactiveProperty<TimeSpan>();
    private readonly ReactiveProperty<TimeSpan> remainingTime = new ReactiveProperty<TimeSpan>();
    private readonly ReactiveProperty<bool?> result = new ReactiveProperty<bool?>(null);
    private readonly BoolReactiveProperty isSlash = new BoolReactiveProperty();
    private readonly ReactiveProperty<TimeSpan> timeLimit = new ReactiveProperty<TimeSpan>();

    private IDisposable intervalOvervable;
    private Dictionary<int, TimeSpan> timeLimitPerLevel;

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
    public IReactiveProperty<bool?> Result => result;

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyReactiveProperty<bool> IsSlash => isSlash;

    /// <summary>
    /// 制限時間
    /// </summary>
    public IReadOnlyReactiveProperty<TimeSpan> TimeLimit => timeLimit;

    private void Start()
    {
        // レベルごとの制限時間
        timeLimitPerLevel = new Dictionary<int, TimeSpan>()
        {
            { 1, TimeSpan.FromSeconds(3) },
            { 2, TimeSpan.FromSeconds(2) },
            { 3, TimeSpan.FromSeconds(1) },
            { 4, TimeSpan.FromSeconds(0.7) },
            { 5, TimeSpan.FromSeconds(0.6) },
            { 6, TimeSpan.FromSeconds(0.5) },
            { 7, TimeSpan.FromSeconds(0.4) },
            { 8, TimeSpan.FromSeconds(0.3) },
            { 9, TimeSpan.FromSeconds(0.2) },
            { 10, TimeSpan.FromSeconds(0.15) },
        };

        // 5～15秒待機してからゲーム開始
        TimeSpan dueTime = TimeSpan.FromSeconds(5 + UnityRandom.Range(0, 10 + 1));
        Observable.Timer(dueTime).SubscribeWithState(this, (_, myself) =>
        {
            if (!result.Value.HasValue)
            {
                // ゲーム開始を通知
                myself.isStart.Value = true;
            }
        }, myself =>
        {
            if (myself.isStart.Value)
            {
                // ゲーム開始

                // 制限時間設定
                TimeSpan timeLimit = myself.timeLimitPerLevel[GameManager.Instance.Level];
                myself.timeLimit.Value = timeLimit;
                myself.remainingTime.Value = timeLimit;

                // 時間計測用のObservableを発行
                myself.intervalOvervable = Observable.TimeInterval(Observable.EveryUpdate()).SubscribeWithState(this, (value, myself) =>
                {
                    // 経過時間は加算
                    myself.elapsed.Value += value.Interval;

                    // 残り時間は減算
                    myself.remainingTime.Value -= value.Interval;
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


        remainingTime.SubscribeWithState(this, (value, myself) =>
        {
            if (value.Ticks < 0)
            {
                // 制限時間経過でゲーム失敗
                myself.isStart.Value = false;
            }
        }).AddTo(this);

        result.SubscribeWithState(this, async (value, myself) =>
        {
            if (value.GetValueOrDefault())
            {
                GameManager.Instance.Level += 1;
            }
        }).AddTo(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isStart.Value || intervalOvervable is null)
            {
                // 時間計測が始まっていないときにボタンを押していたら失敗
                result.Value = false;
                return;
            }

            isSlash.Value = true;
            isSlash.Value = false;
        }
    }

    public void ReloadMainScene() => SceneManager.LoadScene(SceneNames.Main);
}
