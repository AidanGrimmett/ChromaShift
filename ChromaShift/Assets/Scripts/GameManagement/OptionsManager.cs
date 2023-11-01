using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Toggle fullscreenCheck;

    [SerializeField] private AudioManager audioManager;

    private void OnEnable()
    {
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");

        switch (PlayerPrefs.GetInt("Fullscreen"))
        {
            case 0:
                fullscreenCheck.isOn = false;
                break;
            case 1:
                fullscreenCheck.isOn = true;
                break;
        }

        soundSlider.onValueChanged.AddListener(OnSoundSliderValueChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
        fullscreenCheck.onValueChanged.AddListener(OnToggleValueChanged);
    }

    public void OnSoundSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat("SoundVolume", value);
    }
    public void OnMusicSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);

        audioManager.ChangeVolume(Array.Find(audioManager.sounds, sound => sound.name == "music"));
    }

    public void OnToggleValueChanged(bool value)
    {
        PlayerPrefs.SetInt("Fullscreen", value ? 1:0);

        Screen.fullScreen = value;
    }
}
