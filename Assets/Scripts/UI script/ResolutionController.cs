using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResolutionController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    //Resolution Setting
    private Resolution[] _resolutions;
    private List<Resolution> _filteredResolutions;

    private float _currentRefreshRate;
    private int _currentResolutionIndex = 0;

    // Start is called before the first frame update
    private void Start()
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
    }

    public void SetResolution(int resolutionIndex)
    {
        var resolution = _filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, _fullScreen);
    }

    private bool _fullScreen;
    public void SetFullScreen(bool fullScreenToggle)
    {
        _fullScreen = fullScreenToggle;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
