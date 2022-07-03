using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleModel : MonoBehaviour
{
    /// <summary>
    /// ゲームシーンに遷移する。
    /// </summary>
    public void TransitionToGameScene()
    {
        // 前回の記録が残っている可能性があるのでリセットしてから遷移する
        GameManager.Instance.Reset();
        SceneManager.LoadScene(SceneNames.Main);
    }
}