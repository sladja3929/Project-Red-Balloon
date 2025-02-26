using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionController : MonoBehaviour
{
    private Resolution[] _resolutions;
    private int[] _refreshRates;
    private int _currentResolutionIndex = 0;
    private int _currentFrameRateIndex = 0;

    [Header("Resolution Setting")]
    public TMP_Text resolutionText;
    public Button rightResolutionButton;
    public Button leftResolutionButton;
    
    [Header("Frame Rate Setting")]
    public TMP_Text frameRateText;
    public Button rightFrameRateButton;
    public Button leftFrameRateButton;

    private FullScreenMode _fullScreen;

    [Header("Full Screen Setting")]
    public TMP_Text fullScreenText;
    public Button fsRightButton;
    public Button fsLeftButton;

    private void Awake()
    {
        LoadSettings();
        SetupResolution();
        SetupFullScreen();

        rightResolutionButton.onClick.AddListener(() => ChangeResolution(1));
        leftResolutionButton.onClick.AddListener(() => ChangeResolution(-1));
        rightFrameRateButton.onClick.AddListener(() => ChangeFrameRate(1));
        leftFrameRateButton.onClick.AddListener(() => ChangeFrameRate(-1));
        fsRightButton.onClick.AddListener(PressRight_FullScreen);
        fsLeftButton.onClick.AddListener(PressLeft_FullScreen);
    }

    private void SetupResolution()
    {
        _resolutions = Screen.resolutions;
        _refreshRates = new[] { 30, 60, 120, 144, 240 };

        _currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", _resolutions.Length - 1);
        _currentFrameRateIndex = PlayerPrefs.GetInt("FrameRateIndex", 1);

        SetResolution(_currentResolutionIndex);
        SetFrameRate(_currentFrameRateIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        var resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, _fullScreen);
        resolutionText.text = $"{resolution.width}x{resolution.height}";
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
    }

    public void SetFrameRate(int frameRateIndex)
    {
        int targetFrameRate = _refreshRates[frameRateIndex];
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
        frameRateText.text = $"{targetFrameRate} FPS";
        PlayerPrefs.SetInt("FrameRateIndex", frameRateIndex);
    }

    private void ChangeResolution(int direction)
    {
        _currentResolutionIndex = Mathf.Clamp(_currentResolutionIndex + direction, 0, _resolutions.Length - 1);
        SetResolution(_currentResolutionIndex);
    }

    private void ChangeFrameRate(int direction)
    {
        _currentFrameRateIndex = Mathf.Clamp(_currentFrameRateIndex + direction, 0, _refreshRates.Length - 1);
        SetFrameRate(_currentFrameRateIndex);
    }

    private void SetupFullScreen()
    {
        _fullScreen = (FullScreenMode)PlayerPrefs.GetInt("FullScreenMode", (int)FullScreenMode.FullScreenWindow);
        ReloadToggle(_fullScreen);
    }

    public void SetFullScreen(FullScreenMode mode)
    {
        Screen.fullScreenMode = mode;
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, mode);
        ReloadToggle(mode);
        PlayerPrefs.SetInt("FullScreenMode", (int)mode);
    }

    private void ReloadToggle(FullScreenMode mode)
    {
        _fullScreen = mode;
        fullScreenText.text = mode switch
        {
            FullScreenMode.ExclusiveFullScreen => "Borderless",
            FullScreenMode.FullScreenWindow => "FullScreen",
            FullScreenMode.Windowed => "Windowed",
            _ => "Unknown"
        };
    }

    public void PressRight_FullScreen()
    {
        int mode = (int)_fullScreen;
        mode = (mode + 1) % 3;
        SetFullScreen((FullScreenMode)mode);
    }

    public void PressLeft_FullScreen()
    {
        int mode = (int)_fullScreen;
        mode = (mode + 2) % 3;
        SetFullScreen((FullScreenMode)mode);
    }

    private void LoadSettings()
    {
        _currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        _currentFrameRateIndex = PlayerPrefs.GetInt("FrameRateIndex", 0);
        _fullScreen = (FullScreenMode)PlayerPrefs.GetInt("FullScreenMode", (int)FullScreenMode.FullScreenWindow);
    }
}
