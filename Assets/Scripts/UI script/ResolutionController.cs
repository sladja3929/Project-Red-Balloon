using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    //====================Resolution Setting====================
    private Resolution[] _resolutions;
    private List<Resolution> _filteredResolutions;

    private float _currentRefreshRate;
    private int _currentResolutionIndex = 0;

    public void SetResolution(int resolutionIndex)
    {
        var resolution = _filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, _fullScreen);
    }

    public void SetupResolution()
    {
        
        _resolutions = Screen.resolutions;
        _filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        _currentRefreshRate = Screen.currentResolution.refreshRate;

        foreach (var resolution in _resolutions)
        {
            Debug.Log("Resolution : " + resolution);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (resolution.refreshRate == _currentRefreshRate)
            {
                _filteredResolutions.Add(resolution);
            }
        }

        var options = new List<string>();
        if (options == null) throw new ArgumentNullException(nameof(options));

        for (var index = 0; index < _filteredResolutions.Count; index++)
        {
            var t = _filteredResolutions[index];
            var resolutionOption = t.width + "x" +
                                   t.height + " " +
                                   t.refreshRate + "Hz";
            options.Add(resolutionOption);
            if (t.width == Screen.width && t.height == Screen.height)
            {
                _currentResolutionIndex = index;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = _currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    //=======================Full Screen Setting=======================
    [Header("Full Screen Setting")]
    private bool _fullScreen;
    public Button fullScreenToggle;
    public Button windowedToggle;

    [Space(10)]
    public Sprite fullScreenOn;
    public Sprite fullScreenOff;
    public Image fullScreenImage;
    
    [Space(10)]
    public Sprite windowedOn;
    public Sprite windowedOff;
    public Image windowedImage;

    private void ReloadToggle(bool isFullScreen)
    {
        fullScreenToggle.interactable = !isFullScreen;
        windowedToggle.interactable = isFullScreen;
        
        fullScreenImage.sprite = isFullScreen ? fullScreenOn : fullScreenOff;
        windowedImage.sprite = isFullScreen ? windowedOff : windowedOn;
    }

    private void SetupFullScreen()
    {
        ReloadToggle(Screen.fullScreen);
    }

    public void SetFullScreen(bool arg0)
    {
        Debug.Log("SetFullScreen : " + arg0);
        
        _fullScreen = arg0;
        Screen.SetResolution(Screen.width, Screen.height, _fullScreen ? FullScreenMode.FullScreenWindow :  FullScreenMode.Windowed);

        ReloadToggle(_fullScreen);
    }

    private void Awake()
    {
        SetupResolution();
        SetupFullScreen();
    }
}
