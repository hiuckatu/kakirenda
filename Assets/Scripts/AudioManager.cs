 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource seAudioSource;   // 効果音用
    [SerializeField] private AudioSource bgmAudioSource;  // BGM用

    [SerializeField] private AudioClip startSE;           // 開始時効果音
    [SerializeField] private AudioClip ResultSE;

    [SerializeField] private AudioClip clicksound;

    [SerializeField] private AudioClip bgmClip;           // ゲーム中BGM
    [SerializeField] private AudioClip OpeningSE;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGameSound()
    {
        if (seAudioSource != null && startSE != null)
        {
            seAudioSource.PlayOneShot(startSE);
        }

        if (bgmAudioSource != null && bgmClip != null)
        {
            bgmAudioSource.clip = bgmClip;
            bgmAudioSource.loop = true;
            bgmAudioSource.PlayDelayed(startSE != null ? startSE.length : 0f);
        }
    }

    public void EndGameSoundStop()
    {
        if (bgmAudioSource != null && bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Stop();
        }
    }

    public void PlayResultSound()
    {
        seAudioSource.PlayOneShot(ResultSE);
    }

    public void PlayOpeningSound()
    {
        seAudioSource.PlayOneShot(OpeningSE);
    }
    public void PlayOpeningBGM()
    {
        seAudioSource.PlayOneShot(bgmClip);
    }

    public void PlayEatClickSound()
    {
        seAudioSource.PlayOneShot(clicksound);
    }
}
