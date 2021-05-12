

<<<<<<< HEAD
using System;
=======
>>>>>>> main
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{

    public static AudioManager audioManager;
    
    
    #region Fields

    [SerializeField] private AudioMixerGroup masterAudioMixerGroup;
    [SerializeField] private AudioMixerGroup masterSfxMixerGroup;
    
<<<<<<< HEAD
        private  AudioSource _musicSource1;
        private AudioSource _musicSource2;
        private AudioSource _sfxSource;

        private bool _firstMusicSourceIsPlaying;
=======
        private  AudioSource m_MusicSource1;
        private AudioSource m_MusicSource2;
        private AudioSource m_SfxSource;

        private bool m_FirstMusicSourceIsPlaying;
>>>>>>> main
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

<<<<<<< HEAD
        _musicSource1 = this.gameObject.AddComponent<AudioSource>();
        _musicSource1.GetComponent<AudioSource>().outputAudioMixerGroup = masterAudioMixerGroup;
        
        _musicSource2 = this.gameObject.AddComponent<AudioSource>();
        _musicSource1.GetComponent<AudioSource>().outputAudioMixerGroup = masterAudioMixerGroup;
        
        
        _sfxSource = this.gameObject.AddComponent<AudioSource>();
        _sfxSource.GetComponent<AudioSource>().outputAudioMixerGroup = masterSfxMixerGroup;
        
        
        // music tracks are looped
        _musicSource1.loop = true;
        _musicSource2.loop = true;
=======
        m_MusicSource1 = this.gameObject.AddComponent<AudioSource>();
        m_MusicSource1.GetComponent<AudioSource>().outputAudioMixerGroup = masterAudioMixerGroup;
        
        m_MusicSource2 = this.gameObject.AddComponent<AudioSource>();
        m_MusicSource1.GetComponent<AudioSource>().outputAudioMixerGroup = masterAudioMixerGroup;
        
        
        m_SfxSource = this.gameObject.AddComponent<AudioSource>();
        m_SfxSource.GetComponent<AudioSource>().outputAudioMixerGroup = masterSfxMixerGroup;
        
        
        // music tracks are looped
        m_MusicSource1.loop = true;
        m_MusicSource2.loop = true;
>>>>>>> main

    }


    
    public void PlayMusic(AudioClip musicClip)
    {
<<<<<<< HEAD
        AudioSource activeSource = (_firstMusicSourceIsPlaying) ? _musicSource1 : _musicSource2;
=======
        AudioSource activeSource = (m_FirstMusicSourceIsPlaying) ? m_MusicSource1 : m_MusicSource2;
>>>>>>> main
        
        activeSource.clip = musicClip;
        activeSource.volume = 1f;
        activeSource.Play();
    }
    

    public void PlayMusicWithFade(AudioClip newClip, float transitionTime = 1.0f)
    {
<<<<<<< HEAD
        AudioSource activeSource = (_firstMusicSourceIsPlaying) ? _musicSource1 : _musicSource2;
=======
        AudioSource activeSource = (m_FirstMusicSourceIsPlaying) ? m_MusicSource1 : m_MusicSource2;
>>>>>>> main

        StartCoroutine(UpdateMusicWithFade(activeSource, newClip, transitionTime));
    }
    

    public void PlayMusicWithCrossFade(AudioClip musicClip, float transitionTime = 1.0f)
    {
<<<<<<< HEAD
        AudioSource activeSource = (_firstMusicSourceIsPlaying) ? _musicSource1 : _musicSource2;
        AudioSource newSource = (_firstMusicSourceIsPlaying) ? _musicSource2 : _musicSource1;

        _firstMusicSourceIsPlaying = !_firstMusicSourceIsPlaying;
=======
        AudioSource activeSource = (m_FirstMusicSourceIsPlaying) ? m_MusicSource1 : m_MusicSource2;
        AudioSource newSource = (m_FirstMusicSourceIsPlaying) ? m_MusicSource2 : m_MusicSource1;

        m_FirstMusicSourceIsPlaying = !m_FirstMusicSourceIsPlaying;
>>>>>>> main

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
<<<<<<< HEAD
                //yield return new WaitForSeconds(transitionTime);
=======
                
>>>>>>> main
                yield return null;
            }
        }

        activeSource.Stop();
    }
    

    public void PlaySfx(AudioClip clip)
    {
<<<<<<< HEAD
        _sfxSource.PlayOneShot(clip);
=======
        m_SfxSource.PlayOneShot(clip);
>>>>>>> main
    }
    
    
    public void PlaySfx(AudioClip clip, float volume)
    {
<<<<<<< HEAD
        _sfxSource.PlayOneShot(clip, volume);
=======
        m_SfxSource.PlayOneShot(clip, volume);
>>>>>>> main
    }
    

    public void SetMusicVolume(float volume)
    {
<<<<<<< HEAD
        _musicSource1.volume = volume;
        _musicSource2.volume = volume;
=======
        m_MusicSource1.volume = volume;
        m_MusicSource2.volume = volume;
>>>>>>> main
    }
    

    public void SetSfxVolume(float volume)
    {
<<<<<<< HEAD
        _sfxSource.volume = volume;
=======
        m_SfxSource.volume = volume;
>>>>>>> main
    }
    

    public void StopMusic()
    {
<<<<<<< HEAD
        AudioSource activeSource = (_firstMusicSourceIsPlaying) ? _musicSource1 : _musicSource2;
=======
        AudioSource activeSource = (m_FirstMusicSourceIsPlaying) ? m_MusicSource1 : m_MusicSource2;
>>>>>>> main
        
        StartCoroutine(FadeMusic(activeSource, 3f));
        
    }

    public void StopSfx()
    {
<<<<<<< HEAD
        if(_sfxSource.isPlaying)
            _sfxSource.Stop();
=======
        if(m_SfxSource.isPlaying)
            m_SfxSource.Stop();
>>>>>>> main
    }

}
