using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainOption : MonoBehaviour
{
    [SerializeField] private GameObject backGround;
    [FormerlySerializedAs("pannels")] 
    [SerializeField] private GameObject[] panels;

    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    
    [Space(10)]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Slider camSensitivitySlider;

    public void OpenPauseMenu()
    {
        backGround.SetActive(true);
        foreach (GameObject panel in panels)
        {
            panel.SetActive(true);
        }

        sfxVolumeSlider.value = SoundManager.instance.GetSfxSoundVolume();
        musicVolumeSlider.value = SoundManager.instance.GetBackgroundVolume();
    }

    public void ClosePauseMenu()
    {
        backGround.SetActive(false);
        foreach (GameObject pannel in panels)
        {
            pannel.SetActive(false);
        }
    }

    public void SetSfxVolume()
    {
        float value = sfxVolumeSlider.value;
        SoundManager.instance.SetSfxSoundVolume(value);
    }

    public void SetBackgroundVolume()
    {
        float value = musicVolumeSlider.value;
        SoundManager.instance.SetBackgroundVolume(value);
    }

    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }

    private bool _initialized = false;
    private void OnEnable()
    {
        if (_initialized) return;
        _initialized = true;
        
        camSensitivitySlider.onValueChanged.AddListener(delegate { SetControllerSensitivity(); });
        mouseSensitivitySlider.onValueChanged.AddListener(delegate { SetMouseSensitivity(); });
        
        sfxVolumeSlider.onValueChanged.AddListener(delegate { SetSfxVolume(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { SetBackgroundVolume(); });
    }
    
    public void SetMouseSensitivity()
    {
        float value = mouseSensitivitySlider.value;
        StaticSensitivity.SetMouseSensitivity(value);
    }
    
    public void SetControllerSensitivity()
    {
        float value = camSensitivitySlider.value;
        StaticSensitivity.SetCamSensitivity(value);
    }
}
