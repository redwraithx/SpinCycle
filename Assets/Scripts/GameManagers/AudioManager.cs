

using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    //public Sound[] sounds;
    //public static AudioManager instance;

    public static AudioManager audioManager;
    public int netWorkLobbySceneIndex;



    #region Fields

    [SerializeField] private AudioMixerGroup masterAudioMixerGroup;
    [SerializeField] private AudioMixerGroup masterSfxMixerGroup;

    [SerializeField] private AudioSource m_MusicSource1;
    [SerializeField] private AudioSource m_MusicSource2;
    
    [SerializeField] private AudioSource m_SfxSource;
    [SerializeField] private AudioSource menuSettingTestSFX;

    // public AudioSource[] m_SfxSource = new AudioSource[6];

    private bool m_FirstMusicSourceIsPlaying;
    #endregion // Fields


    private void Awake()
    {

        /*if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        if (GameManager.audioManager)
        {
            DestroyImmediate(gameObject);

            return;
        }*/

        GameManager.audioManager = this;

        // make sure this instance does not get destroyed until the game is closed
        DontDestroyOnLoad(this.gameObject);

        m_MusicSource1 = this.gameObject.AddComponent<AudioSource>();
        m_MusicSource1.outputAudioMixerGroup = masterAudioMixerGroup;

        m_MusicSource2 = this.gameObject.AddComponent<AudioSource>();
        m_MusicSource2.outputAudioMixerGroup = masterAudioMixerGroup;

        m_SfxSource = this.gameObject.AddComponent<AudioSource>();
        m_SfxSource.outputAudioMixerGroup = masterSfxMixerGroup;

        menuSettingTestSFX = this.gameObject.AddComponent<AudioSource>();
        menuSettingTestSFX.outputAudioMixerGroup = masterSfxMixerGroup;



        // music tracks are looped
        m_MusicSource1.loop = true;
        m_MusicSource2.loop = true;

    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == netWorkLobbySceneIndex)
        {
            Debug.Log("Not Stopingasdjkahdskajhdsj");


            if (GameManager.audioManager.IsMusicPlaying())
            {
                GameManager.audioManager.StopMusic();
            }
        }
    }

    /* public void Play (string name)
     {
         Sound s = Array.Find(sounds, sound => sound.name == name);
         if (s == null)
             return;
         s.source.Play();
     }*/
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

        for (fadeTime = 0; fadeTime < transitionTime; fadeTime += Time.deltaTime)
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

    public void PlaySettingSfx(AudioClip clip)
    {
        if (menuSettingTestSFX.isPlaying)
            return;
        else
            menuSettingTestSFX.clip = null;


        menuSettingTestSFX.clip = clip;
        menuSettingTestSFX.Play();
    }

    public void UpdateSfxVolume(float volume)
    {
        if (menuSettingTestSFX.isPlaying)
            menuSettingTestSFX.volume = volume;

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

    public bool IsMusicPlaying()
    {
        AudioSource activeSource = (m_FirstMusicSourceIsPlaying) ? m_MusicSource1 : m_MusicSource2;
        if (activeSource)
            return true;

        return false;
    }
    public void StopMusic()
    {
        AudioSource activeSource = (m_FirstMusicSourceIsPlaying) ? m_MusicSource1 : m_MusicSource2;

        StartCoroutine(FadeMusic(activeSource, 3f));

    }

    public void StopSfx()
    {

        if (menuSettingTestSFX.isPlaying)
            menuSettingTestSFX.Stop();
        if(m_SfxSource.isPlaying)
            m_SfxSource.Stop();
        
    }



}