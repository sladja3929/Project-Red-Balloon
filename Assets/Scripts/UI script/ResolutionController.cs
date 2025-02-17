using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionController : MonoBehaviour
{
    //====================Resolution Setting====================
    private Resolution[] _resolutions;
    private int[] _refreshRates;
    private int _currentResolutionIndex = 0;
    private int _currentFrameRateIndex = 0;

    [Header("Resolution Setting")]
    public TMP_Text resolutionText;
    public Button rightResolutionButton;
    public Button leftResolutionButton;
    
    // Frame Rate 설정
    [Header("Frame Rate Setting")]
    public TMP_Text frameRateText;
    public Button rightFrameRateButton;
    public Button leftFrameRateButton;

    // Resolution 설정
    public void SetResolution(int resolutionIndex)
    {
        var resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, _fullScreen);

        // 텍스트 업데이트
        resolutionText.text = $"{resolution.width}x{resolution.height}";
    }

    // Frame Rate 설정
    public void SetFrameRate(int frameRateIndex)
    {
        int targetFrameRate = _refreshRates[frameRateIndex];
        if (frameRateIndex >= 0 && frameRateIndex < _refreshRates.Length)
        {
            targetFrameRate = _refreshRates[frameRateIndex];
        }
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;

        // 텍스트 업데이트
        frameRateText.text = $"{targetFrameRate} FPS";
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

    private void SetupResolution()
    {
        // 미리 정의된 해상도 배열
        _resolutions = new[]
        {
            // 4k
            new Resolution { width = 3840, height = 2160, refreshRate = 60 },

            // 1440p
            new Resolution { width = 2560, height = 1440, refreshRate = 60 },

            // 1080p
            new Resolution { width = 1920, height = 1080, refreshRate = 60 },

            // 720p
            new Resolution { width = 1280, height = 720, refreshRate = 60 },
        };

        _refreshRates = new[] { 30, 60, 120, 144, 240 }; // 예시 refresh rate들

        // 첫 해상도와 프레임 레이트를 설정
        SetResolution(_currentResolutionIndex);
        SetFrameRate(_currentFrameRateIndex);
    }

    //=======================Full Screen Setting=======================
    
    private bool _fullScreen;
    
    [Header("Full Screen Setting")]
    public TMP_Text fullScreenText;
    public Button fsRightButton;
    public Button fsLeftButton;

    public void PressRight_FullScreen()
    {
        SetFullScreen(!_fullScreen);
    }

    public void PressLeft_FullScreen()
    {
        SetFullScreen(!_fullScreen);
    }

    private void ReloadToggle(bool isFullScreen)
    {
        _fullScreen = isFullScreen;
        fullScreenText.text = isFullScreen ? "Full Screen" : "Windowed";
    }

    private void SetupFullScreen()
    {
        ReloadToggle(Screen.fullScreen);
    }

    public void SetFullScreen(bool arg0)
    {
        Debug.Log("SetFullScreen : " + arg0);

        _fullScreen = arg0;
        Screen.SetResolution(Screen.width, Screen.height, _fullScreen ?
            FullScreenMode.FullScreenWindow :
            FullScreenMode.Windowed);

        ReloadToggle(_fullScreen);
    }

    private void Awake()
    {
        SetupResolution();
        SetupFullScreen();

        // Button 클릭 이벤트 등록
        rightResolutionButton.onClick.AddListener(() => ChangeResolution(1));
        leftResolutionButton.onClick.AddListener(() => ChangeResolution(-1));
        rightFrameRateButton.onClick.AddListener(() => ChangeFrameRate(1));
        leftFrameRateButton.onClick.AddListener(() => ChangeFrameRate(-1));
        fsRightButton.onClick.AddListener(PressRight_FullScreen);
        fsLeftButton.onClick.AddListener(PressLeft_FullScreen);
    }

    private void Update()
    {
        // Check Full Screen 상태
        if (Screen.fullScreen != _fullScreen)
            SetupFullScreen();
    }
}
