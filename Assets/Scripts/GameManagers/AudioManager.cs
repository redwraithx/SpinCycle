

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{

    public static AudioManager audioManager;
    
    
    #region Fields

    [SerializeField] private AudioMixerGroup masterAudioMixerGroup;
    [SerializeField] private AudioMixerGroup masterSfxMixerGroup;
    
        private  AudioSource _musicSource1;
        private AudioSource _musicSource2;
        private AudioSource _sfxSource;

        private bool _firstMusicSourceIsPlaying;
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

        _musicSource1 = this.gameObject.AddComponent<AudioSource>();
        _musicSource1.GetComponent<AudioSource>().outputAudioMixerGroup = masterAudioMixerGroup;
        
        _musicSource2 = this.gameObject.AddComponent<AudioSource>();
        _musicSource1.GetComponent<AudioSource>().outputAudioMixerGroup = masterAudioMixerGroup;
        
        
        _sfxSource = this.gameObject.AddComponent<AudioSource>();
        _sfxSource.GetComponent<AudioSource>().outputAudioMixerGroup = masterSfxMixerGroup;
        
        
        // music tracks are looped
        _musicSource1.loop = true;
        _musicSource2.loop = true;

    }


    
    public void PlayMusic(AudioClip musicClip)
    {
        AudioSource activeSource = (_firstMusicSourceIsPlaying) ? _musicSource1 : _musicSource2;
        
        activeSource.clip = musicClip;
        activeSource.volume = 1f;
        activeSource.Play();
    }
    

    public void PlayMusicWithFade(AudioClip newClip, float transitionTime = 1.0f)
    {
        AudioSource activeSource = (_firstMusicSourceIsPlaying) ? _musicSource1 : _musicSource2;

        StartCoroutine(UpdateMusicWithFade(activeSource, newClip, transitionTime));
    }
    

    public void PlayMusicWithCrossFade(AudioClip musicClip, float transitionTime = 1.0f)
    {
        AudioSource activeSource = (_firstMusicSourceIsPlaying) ? _musicSource1 : _musicSource2;
        AudioSource newSource = (_firstMusicSourceIsPlaying) ? _musicSource2 : _musicSource1;

        _firstMusicSourceIsPlaying = !_firstMusicSourceIsPlaying;

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
                //yield return new WaitForSeconds(transitionTime);
                yield return null;
            }
        }

        activeSource.Stop();
    }
    

    public void PlaySfx(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip);
    }
    
    
    public void PlaySfx(AudioClip clip, float volume)
    {
        _sfxSource.PlayOneShot(clip, volume);
    }
    

    public void SetMusicVolume(float volume)
    {
        _musicSource1.volume = volume;
        _musicSource2.volume = volume;
    }
    

    public void SetSfxVolume(float volume)
    {
        _sfxSource.volume = volume;
    }
    

    public void StopMusic()
    {
        AudioSource activeSource = (_firstMusicSourceIsPlaying) ? _musicSource1 : _musicSource2;
        
        StartCoroutine(FadeMusic(activeSource, 3f));
        
    }

    public void StopSfx()
    {
        if(_sfxSource.isPlaying)
            _sfxSource.Stop();
    }

}
