using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using Slider = UnityEngine.UI.Slider;

public class SettingsController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private Slider _volumeSlider;
    private float _currentVolume;
    private Resolution[] _resolutions;

    private void Start()
    {
        _resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        _resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);
            if (_resolutions[i].width == Screen.currentResolution.width 
                && _resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
        
        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
        AudioListener.volume = _currentVolume;
    }

    public void SetVolume()
    {
        
        _currentVolume = _volumeSlider.value;
        AudioListener.volume = _currentVolume;
    }

    public void SetFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void SetResolution()
    {
        Resolution resolution = _resolutions[_resolutionDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionPreference", 
            _resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", 
            Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetFloat("VolumePreference", 
            _currentVolume); 
    }
    
    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("ResolutionPreference"))
            _resolutionDropdown.value = 
                PlayerPrefs.GetInt("ResolutionPreference");
        else
            _resolutionDropdown.value = currentResolutionIndex;
        if (PlayerPrefs.HasKey("FullscreenPreference"))
            Screen.fullScreen = 
                Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        else
            Screen.fullScreen = true;
        if (PlayerPrefs.HasKey("VolumePreference"))
            _volumeSlider.value = 
                PlayerPrefs.GetFloat("VolumePreference");
        else
            _volumeSlider.value = 
                PlayerPrefs.GetFloat("VolumePreference");
    }
}
