using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UniRx;

public class ResultDialogView : MonoBehaviour
{
    [SerializeField]
    private Button playAgainButton;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private GameObject dialogContainer;

    [SerializeField]
    private TextMeshProUGUI clearLevelText;

    [SerializeField]
    private TextMeshProUGUI bestTimeText;


    /// <summary>
    /// Play againボタンを押したときの動作を取得または設定する。
    /// </summary>
    public Action ClickPlayAgainButtonAction { get; set; }

    /// <summary>
    /// Back to titleボタンを押したときの動作を取得または設定する。
    /// </summary>
    public Action ClickBackButtonAction { get; set; }

    private void Start()
    {
        HideDialog();

        playAgainButton.OnClickAsObservable().
            First().
            SubscribeWithState(this, (_, myself) => myself.ClickPlayAgainButtonAction?.Invoke()).
            AddTo(this);

        backButton.OnClickAsObservable().
            First().
            SubscribeWithState(this, (_, myself) => myself.ClickBackButtonAction?.Invoke()).
            AddTo(this);
    }

    /// <summary>
    /// リザルトダイアログを表示する。
    /// </summary>
    public void ShowDialog()
    {
        // 結果表示
        GameManager gameManager = GameManager.Instance;
        SetClearLevel(gameManager.Level == GameManager.MinLevel ? null : gameManager.Level);
        SetBestTime(gameManager.BestTime);

        gameObject.SetActive(true);
    }

    /// <summary>
    /// リザルトダイアログを非表示にする。
    /// </summary>
    public void HideDialog() => dialogContainer.SetActive(false);

    /// <summary>
    /// クリアレベルを設定する。
    /// </summary>
    /// <param name="value"></param>
    private void SetClearLevel(int? value)
    {
        if (value is int clearLevel)
        {
            clearLevelText.text = $"Clear Level : {clearLevel}";
        }
        else
        {
            clearLevelText.text = $"Clear Level : --";
        }
    }

    /// <summary>
    /// ベストタイムを設定する。
    /// </summary>
    /// <param name="value"></param>
    private void SetBestTime(System.TimeSpan? value)
    {
        if (value is System.TimeSpan bestTime)
        {
            bestTimeText.text = $"Best Time : {bestTime:s\\.fff}s";
        }
        else
        {
            bestTimeText.text = "Best Time : -.---s";
        }
    }
}