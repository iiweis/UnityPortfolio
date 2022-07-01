using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI pressEnterText;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Fader fader;

    public Fader Fader => fader;

    private async void Start()
    {
        // フェードイン
        await fader.FadeIn();

        // テキスト点滅
        pressEnterText.DOFade(0.0f, 1.5f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo);
    }

    public async Task PlayStartSound()
    {
        audioSource.PlayOneShot(audioSource.clip);
        await UniTask.WaitWhile(() => audioSource.isPlaying);
    }
}
