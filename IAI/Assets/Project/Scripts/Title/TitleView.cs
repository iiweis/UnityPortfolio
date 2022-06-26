using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI pressEnterText;

    private void Start()
    {
        // テキスト点滅
        pressEnterText.DOFade(0.0f, 1.5f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo);
    }
}
