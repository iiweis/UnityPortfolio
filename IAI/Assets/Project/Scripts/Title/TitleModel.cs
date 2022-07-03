using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleModel : MonoBehaviour
{
    /// <summary>
    /// ゲームシーンに遷移する。
    /// </summary>
    public void TransitionToGameScene() => SceneManager.LoadScene(SceneNames.Main);
}