using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject backGround;
    [FormerlySerializedAs("pannels")] 
    [SerializeField] private GameObject[] panels;

    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    
    [Space(10)]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Slider camSensitivitySlider;
    
    private DragRotation _dragRotation;
    private CameraController _cameraController;

    private void OpenPauseMenu()
    {
        backGround.SetActive(true);
        foreach (GameObject panel in panels)
        {
            panel.SetActive(true);
        }

        GameManager.IsPause = true;

        sfxVolumeSlider.value = SoundManager.instance.GetSfxSoundVolume();
        musicVolumeSlider.value = SoundManager.instance.GetBackgroundVolume();
        
        //mouseSensitivitySlider.value = _dragRotation.GetRotationSpeedRate();
        //camSensitivitySlider.value = _cameraController.GetDpiRate();
    }

    public void ClosePauseMenu()
    {
        backGround.SetActive(false);
        foreach (GameObject pannel in panels)
        {
            pannel.SetActive(false);
        }

        GameManager.IsPause = false;
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

    public void BackToMainMenu()
    {
        GameManager.GoToMainMenu();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (backGround.activeSelf)
                ClosePauseMenu();
            else 
                OpenPauseMenu();
        }
    }
    
    private void Awake()
    {
        ClosePauseMenu();
        
        _dragRotation = FindObjectOfType<DragRotation>();
        _cameraController = FindObjectOfType<CameraController>();
        
        camSensitivitySlider.onValueChanged.AddListener(delegate { SetControllerSensitivity(); });
        mouseSensitivitySlider.onValueChanged.AddListener(delegate { SetMouseSensitivity(); });
    }
    
    public void SetMouseSensitivity()
    {
        float value = mouseSensitivitySlider.value;
        //_dragRotation.SetRotationSpeedRate(value);
    }
    
    public void SetControllerSensitivity()
    {
        float value = camSensitivitySlider.value;
        //_cameraController.SetDpiRate(value);
    }
}
