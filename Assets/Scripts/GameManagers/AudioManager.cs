﻿

using System.Collections;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{

    public static AudioManager audioManager;
    
    
    #region Fields

    [SerializeField] private AudioMixerGroup masterAudioMixerGroup;
    [SerializeField] private AudioMixerGroup masterSfxMixerGroup;
    
        private  AudioSource m_MusicSource1;
        private AudioSource m_MusicSource2;
        private AudioSource m_SfxSource;

        private bool m_FirstMusicSourceIsPlaying;
    #endregion // Fields


    private void Awake()
    {
        if (GameManager.audioManager)
        {
            DestroyImmediate(gameObject);
        }

        GameManager.audioManager = this;

        // make sure this instance does not get destroyed until the game is closed
        DontDestroyOnLoad(this.gameObject);

        m_MusicSource1 = this.gameObject.AddComponent<AudioSource>();
        m_MusicSource1.GetComponent<AudioSource>().outputAudioMixerGroup = masterAudioMixerGroup;
        
        m_MusicSource2 = this.gameObject.AddComponent<AudioSource>();
        m_MusicSource1.GetComponent<AudioSource>().outputAudioMixerGroup = masterAudioMixerGroup;
        
        
        m_SfxSource = this.gameObject.AddComponent<AudioSource>();
        m_SfxSource.GetComponent<AudioSource>().outputAudioMixerGroup = masterSfxMixerGroup;
        
        
        // music tracks are looped
        m_MusicSource1.loop = true;
        m_MusicSource2.loop = true;

    }


    
    public void PlayMusic(AudioClip musicClip)
    {
        AudioSource activeSource = (m_FirstMusicSourceIsPlaying) ? m_MusicSource1 : m_MusicSource2;
        
        activeSource.clip = musicClip;
        activeSource.volume = 1f;
        activeSource.Play();
    }
    

    public void PlayMusicWithFade(AudioClip newClip, float transitionTime = 1.0f)
    {
        AudioSource activeSource = (m_FirstMusicSourceIsPlaying) ? m_MusicSource1 : m_MusicSource2;

        StartCoroutine(UpdateMusicWithFade(activeSource, newClip, transitionTime));
    }
    

    public void PlayMusicWithCrossFade(AudioClip musicClip, float transitionTime = 1.0f)
    {
        AudioSource activeSource = (m_FirstMusicSourceIsPlaying) ? m_MusicSource1 : m_MusicSource2;
        AudioSource newSource = (m_FirstMusicSourceIsPlaying) ? m_MusicSource2 : m_MusicSource1;

        m_FirstMusicSourceIsPlaying = !m_FirstMusicSourceIsPlaying;

        newSource.clip = musicClip;
        newSource.Play();

        StartCoroutine(UpdateMusicWithFade(activeSource, newSource, transitionTime));
    }
    
    
    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip newClip, float transitionTime)
    {
        if (!activeSource.isPlaying)
            activeSource.Play();

        float fadeTime = 0.0f;

        // fade music out
        for (fadeTime = 0f; fadeTime < transitionTime; fadeTime += Time.deltaTime)
        {
            activeSource.volume = (1 - (fadeTime / transitionTime));
            yield return null;
        }

        activeSource.Stop();
        activeSource.clip = newClip;
        activeSource.Play();

        // fade music in
        for (fadeTime = 0f; fadeTime < transitionTime; fadeTime += Time.deltaTime)
        {
            activeSource.volume = ((fadeTime / transitionTime));
            yield return null;
        }
    }
    

    private IEnumerator UpdateMusicWithFade(AudioSource original, AudioSource newSource, float transitionTime)
    {
        float fadeTime = 0.0f;

        for (fadeTime = 0;  fadeTime < transitionTime; fadeTime += Time.deltaTime)
        {
            original.volume = (1 - (fadeTime / transitionTime));
            newSource.volume = (transitionTime / transitionTime);

            yield return null;
        }

        original.Stop();
    }
    

    private IEnumerator FadeMusic(AudioSource activeSource, float transitionTime)
    {
        if (!activeSource.isPlaying)
        {
            yield return null;
        }
        else
        {
            float fadeTime = 0.0f;

            // fade music out
            for (fadeTime = 0f; fadeTime < transitionTime; fadeTime += Time.deltaTime)
            {
                activeSource.volume = (1 - (fadeTime / transitionTime));
                
                yield return null;
            }
        }

        activeSource.Stop();
    }
    

    public void PlaySfx(AudioClip clip)
    {
        m_SfxSource.PlayOneShot(clip);
    }
    
    
    public void PlaySfx(AudioClip clip, float volume)
    {
        m_SfxSource.PlayOneShot(clip, volume);
    }
    

    public void SetMusicVolume(float volume)
    {
        m_MusicSource1.volume = volume;
        m_MusicSource2.volume = volume;
    }
    

    public void SetSfxVolume(float volume)
    {
        m_SfxSource.volume = volume;
    }
    

    public void StopMusic()
    {
        AudioSource activeSource = (m_FirstMusicSourceIsPlaying) ? m_MusicSource1 : m_MusicSource2;
        
        StartCoroutine(FadeMusic(activeSource, 3f));
        
    }

    public void StopSfx()
    {
        if(m_SfxSource.isPlaying)
            m_SfxSource.Stop();
    }

}
