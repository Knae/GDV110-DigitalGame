using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    //Declaring Variables for functions
    public AudioMixer audioMixer;
    Resolution[] resolutions;
    public Dropdown ResolutionDropdown;

    //Start function to configue players Screen Resolution Options
    private void Start()
    {
        int CurrentResolutionIndex = 0;
        resolutions = Screen.resolutions;

        ResolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string Option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(Option);

            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
            {
                CurrentResolutionIndex = i;
            }
        }

        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = CurrentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    //Sets resolution of game
    public void SetResolution(int ResolutionIndex)
    {
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    //Sets volume of game
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    //Sets Graphics Quality of game
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    //Toggles Fullscreen option
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}