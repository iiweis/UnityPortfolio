using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private List<Audio> audios = new List<Audio>();

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 指定した名前のオーディオを一度だけ再生する。
    /// </summary>
    /// <param name="name"></param>
    public void PlayOneShot(string name)
    {
        Audio audio = audios.Find(audio => audio.Name == name);
        if (audio is null)
        {
            Util.ThrowInvalidOperationException("指定した名前のオーディオが存在しないため、再生できません。");
        }

        audioSource.PlayOneShot(audio.AudioClip);
    }

    /// <summary>
    /// 指定した名前のオーディオを非同期で一度だけ再生する。
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task PlayOneShotAsync(string name)
    {
        PlayOneShot(name);
        await UniTask.WaitWhile(() => audioSource.isPlaying);
    }

    [System.Serializable]
    public class Audio
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private AudioClip audioClip;

        public string Name => name;

        public AudioClip AudioClip => audioClip;
    }
}