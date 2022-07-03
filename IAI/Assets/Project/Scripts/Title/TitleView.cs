using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading.Tasks;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class TitleView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI pressEnterText;

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private Fader fader;

    /// <summary>
    /// フェーダーを取得する。
    /// </summary>
    public Fader Fader => fader;

    /// <summary>
    /// スタート時の動作を取得または設定する。
    /// </summary>
    public Action StartAction { get; set; }

    private async void Start()
    {
        // Enterキー押下または左クリックを監視
        this.UpdateAsObservable().
            Where(_ => Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)).
            First().
            SubscribeWithState(this, (_, myself) => myself.OnPressEnter()).
            AddTo(this);

        // フェードイン
        await fader.FadeIn();

        // テキスト点滅
        pressEnterText.DOFade(0.0f, 1.5f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// スタート時のSEを鳴らす。
    /// </summary>
    /// <returns></returns>
    public async Task PlayStartSoundAsync() => await audioManager.PlayOneShotAsync("Enter");


    private void OnPressEnter() => StartAction?.Invoke();
}