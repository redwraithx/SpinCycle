using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
public class Settings : MonoBehaviour
{
    public AudioMixer SFX;
    public AudioMixer Music;
    public float sfxVol;
    public float musicVol;
    public Slider sfxSlide;
    public Slider musicSlide;
    public AudioSource theme1;


    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    public Button ApplyButton;
    // Start is called before the first frame update
    void Start()
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
    }

    // Update is called once per frame
    void Update()
    {

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
        PlayerPrefs.SetInt("ResolutionPreference",
           resolutionDropdown.value);
        PlayerPrefs.SetFloat("SFXVolumePreference",
           sfxSlide.value);
        PlayerPrefs.SetFloat("MusicVolumePreference",
           musicSlide.value);
    }

    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("ResolutionPreference"))
            resolutionDropdown.value =
                         PlayerPrefs.GetInt("ResolutionPreference");
        else
            resolutionDropdown.value = currentResolutionIndex;
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

    public void LoadSettings2()
    {

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


    public void SaveSettings2()
    {
        PlayerPrefs.SetFloat("SFXVolumePreference",
           sfxSlide.value);
        PlayerPrefs.SetFloat("MusicVolumePreference",
           musicSlide.value);
    }
}
