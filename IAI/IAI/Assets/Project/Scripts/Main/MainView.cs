using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    [SerializeField]
    private GameObject uiContainer;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private GameObject exclamation;

    [SerializeField]
    private TextMeshProUGUI elapsedText;

    [SerializeField]
    private Slider remainingTimeSlider;

    [SerializeField]
    private Fader fader;

    [SerializeField]
    private GameObject waraNormal;

    [SerializeField]
    private GameObject waraCutted;

    [SerializeField]
    private AudioManager audioManager;

    /// <summary>
    /// フェーダーを取得する。
    /// </summary>
    public Fader Fader => fader;

    /// <summary>
    /// Spaceキー押下時の動作を取得または設定する。
    /// </summary>
    public Action PressSpaceAction { get; set; }

    private async void Start()
    {
        SetExclamationActive(false);
        SetElapsedTextActive(false);
        SetRemainingTimeSliderActive(false);

        // Spaceキー押下または左クリックを監視
        this.UpdateAsObservable().
            Where(_ => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)).
            SubscribeWithState(this, (_, myself) => myself.OnPressSpace()).
            AddTo(this);

        // テキスト初期値設定
        SetLevel(GameManager.Instance.Level);
        SetElapsed(TimeSpan.Zero);

        // フェードイン
        await fader.FadeIn();
    }

    /// <summary>
    /// 居合斬りを行う。
    /// </summary>
    public void Slash()
    {
        // SEを鳴らし、巻き藁を切れているように見せる
        var _ = 　PlaySlashSoundAsync();
        waraNormal.SetActive(false);
        waraCutted.SetActive(true);
    }

    /// <summary>
    /// UI全体を非表示にする。
    /// </summary>
    public void HideUI() => uiContainer.SetActive(false);

    /// <summary>
    /// ビックリマークの活性状態を設定する。
    /// </summary>
    /// <param name="value"></param>
    public void SetExclamationActive(bool value) => exclamation.SetActive(value);

    /// <summary>
    /// 経過時間テキストの活性状態を設定する。
    /// </summary>
    /// <param name="value"></param>
    public void SetElapsedTextActive(bool value) => elapsedText.gameObject.SetActive(value);

    /// <summary>
    /// 残り時間スライドバーの活性状態を設定する。
    /// </summary>
    /// <param name="value"></param>
    public void SetRemainingTimeSliderActive(bool value) => remainingTimeSlider.gameObject.SetActive(value);

    /// <summary>
    /// 現在のレベルを設定する。
    /// </summary>
    /// <param name="value"></param>
    public void SetLevel(int value) => levelText.text = $"Level : {value}";

    /// <summary>
    /// 現在の経過時間を設定する。
    /// </summary>
    /// <param name="elapsed"></param>
    public void SetElapsed(System.TimeSpan elapsed) => elapsedText.text = $"{elapsed:s\\.fff}s";

    /// <summary>
    /// 残り時間のスライダーの範囲及び値を設定する。
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <param name="value"></param>
    public void SetRemainingTimeSlider(float minValue, float maxValue, float value)
    {
        remainingTimeSlider.minValue = minValue;
        remainingTimeSlider.maxValue = maxValue;
        SetRemainingTimeSliderValue(value);
    }

    /// <summary>
    /// 残り時間のスライダーの値を設定する。
    /// </summary>
    /// <param name="value"></param>
    public void SetRemainingTimeSliderValue(float value) => remainingTimeSlider.value = value;

    /// <summary>
    /// 居合斬り時の音を鳴らす。
    /// </summary>
    /// <returns></returns>
    public async Task PlaySlashSoundAsync() => await audioManager.PlayOneShotAsync("Slash");

    /// <summary>
    /// 警告音(ビックリマークを表示するときの音)を鳴らす。
    /// </summary>
    /// <returns></returns>
    public async Task PlayAlarmSoundAsync() => await audioManager.PlayOneShotAsync("Alarm");

    /// <summary>
    /// ビープ音を鳴らす。
    /// </summary>
    /// <returns></returns>
    public async Task PlayBeepSoundAsync() => await audioManager.PlayOneShotAsync("Beep");

    /// <summary>
    /// ボタンクリック時の音を鳴らす。
    /// </summary>
    /// <returns></returns>
    public async Task PlayButtonClickSound() => await audioManager.PlayOneShotAsync("Click");

    private void OnPressSpace() => PressSpaceAction?.Invoke();
}
