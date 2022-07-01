using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class Fader : MonoBehaviour
{
    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    private float durationSeconds;

    public async Task FadeIn() => await fadeImage.DOFade(0f, durationSeconds).AsyncWaitForCompletion();

    public async Task FadeOut() => await fadeImage.DOFade(1f, durationSeconds).AsyncWaitForCompletion();

    public async Task FadeIn(float durationSeconds) => await fadeImage.DOFade(0f, durationSeconds).AsyncWaitForCompletion();

    public async Task FadeOut(float durationSeconds) => await fadeImage.DOFade(1f, durationSeconds).AsyncWaitForCompletion();
}