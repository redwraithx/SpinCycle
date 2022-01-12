using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public AudioMixer SFX;
    public AudioMixer Music;
    public float sfxVol;
    public float musicVol;
    public Slider sfxSlide;
    public Slider musicSlide;
    public AudioSource theme1;
    private int settingSceneIndex = 2; // Setting Menu Scene

    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    public Button ApplyButton;

    private void Start()
    {
        if (theme1 != null)
        {
            theme1.Play();
        }

        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();
            resolutions = Screen.resolutions;
            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " +
                         resolutions[i].height;
                options.Add(option);
                if (resolutions[i].width == Screen.currentResolution.width
                      && resolutions[i].height == Screen.currentResolution.height)
                    currentResolutionIndex = i;
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.RefreshShownValue();
            LoadSettings(currentResolutionIndex);
        }
        else
        {
            LoadSettings2();
        }

        sfxSlide.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    public void SetSFXVolume(float volume)
    {
        SFX.SetFloat("SFXVolume", volume);
        sfxVol = volume;
    }

    public void SetMusicVolume(float volume)
    {
        Music.SetFloat("Volume", volume);
        musicVol = volume;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,
                  resolution.height, Screen.fullScreen);
    }

    public void SaveSettings()
    {
        if (SceneManager.GetActiveScene().buildIndex == settingSceneIndex)
        {
            PlayerPrefs.SetInt("ResolutionPreference",
           resolutionDropdown.value);
        }

        PlayerPrefs.SetFloat("SFXVolumePreference",
           sfxSlide.value);
        PlayerPrefs.SetFloat("MusicVolumePreference",
           musicSlide.value);
    }

    public void LoadSettings(int currentResolutionIndex)
    {
        //Debug.Log("loadsettings");
        if (SceneManager.GetActiveScene().buildIndex == settingSceneIndex)
        {
            if (PlayerPrefs.HasKey("ResolutionPreference"))
                resolutionDropdown.value =
                             PlayerPrefs.GetInt("ResolutionPreference");
            else
                resolutionDropdown.value = currentResolutionIndex;
        }

        if (PlayerPrefs.HasKey("SFXVolumePreference"))
        {
            sfxSlide.value =
                        PlayerPrefs.GetFloat("SFXVolumePreference");
            SetSFXVolume(sfxSlide.value);
            GameManager.audioManager.UpdateSfxVolume(sfxSlide.value);
        }
        else
            sfxSlide.value =
                        PlayerPrefs.GetFloat("SFXVolumePreference");
        if (PlayerPrefs.HasKey("MusicVolumePreference"))
        {
            musicSlide.value = PlayerPrefs.GetFloat("MusicVolumePreference");

            SetMusicVolume(musicSlide.value);
        }
        else
            musicSlide.value =
                        PlayerPrefs.GetFloat("MusicVolumePreference");
    }

    public void LoadSettings2()
    {
        //Debug.Log("loadsettings2");
        if (PlayerPrefs.HasKey("SFXVolumePreference"))
        {
            sfxSlide.value =
                        PlayerPrefs.GetFloat("SFXVolumePreference");
            SetSFXVolume(sfxSlide.value);
        }
        else
            sfxSlide.value =
                        PlayerPrefs.GetFloat("SFXVolumePreference");
        if (PlayerPrefs.HasKey("MusicVolumePreference"))
        {
            musicSlide.value = PlayerPrefs.GetFloat("MusicVolumePreference");

            SetMusicVolume(musicSlide.value);
        }
        else
            musicSlide.value =
                        PlayerPrefs.GetFloat("MusicVolumePreference");
    }

    public void ValueChangeCheck()
    {
        AudioClip runningSound = Resources.Load<AudioClip>("AudioFiles/SoundFX/Player/WalkingSound/laser");
        GameManager.audioManager.PlaySettingSfx(runningSound);
    }

    public void ResetPlayerPrefs() => PlayerPrefs.DeleteAll();
}