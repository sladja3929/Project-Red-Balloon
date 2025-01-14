using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor.Rendering;
#endif

public class URPSettingsController : MonoBehaviour
{
    public UniversalRenderPipelineAsset currentAsset;
    
    [Space(20)]
    public UniversalRenderPipelineAsset highQualityAsset;
    public UniversalRenderPipelineAsset mediumQualityAsset;
    public UniversalRenderPipelineAsset lowQualityAsset;
    
    [Space(10)]
    [Header("UI")]
    public Button rightButton;
    public Button leftButton;
    public TMP_Text qualityText;

    private void Awake()
    {
        // URP Asset을 가져옵니다.
        currentAsset = GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset;
        
        rightButton.onClick.RemoveAllListeners();
        rightButton.onClick.AddListener(OnClickRightButton);
        
        leftButton.onClick.RemoveAllListeners();
        leftButton.onClick.AddListener(OnClickLeftButton);
        
        ReloadText();

        if (currentAsset == null)
        {
            Debug.LogError("URP Asset이 설정되지 않았습니다.");
        }
    }

    private void SetQualityLevel(QualityLevel qualityLevel)
    {
        GraphicsSettings.renderPipelineAsset = qualityLevel switch
        {
            QualityLevel.High => highQualityAsset,
            QualityLevel.Medium => mediumQualityAsset,
            QualityLevel.Low => lowQualityAsset,
            _ => throw new ArgumentOutOfRangeException(
                nameof(qualityLevel), qualityLevel, 
                "Error: Invalid Quality Level"
                )
        };
        currentAsset = (UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset;
        
        QualitySettings.renderPipeline = currentAsset;
        
        ReloadText();
    }

    private void OnClickRightButton()
    {
        if (currentAsset == lowQualityAsset)
        {
            SetQualityLevel(QualityLevel.Medium);
        }
        else if (currentAsset == mediumQualityAsset)
        {
            SetQualityLevel(QualityLevel.High);
        }
        
        Debug.Log("Current Asset : " + currentAsset);
    }

    private void OnClickLeftButton()
    {
        if (currentAsset == mediumQualityAsset)
        {
            SetQualityLevel(QualityLevel.Low);
        }
        else if (currentAsset == highQualityAsset)
        {
            SetQualityLevel(QualityLevel.Medium);
        }
        
        Debug.Log("Current Asset : " + currentAsset);
    }
    
    private void ReloadText()
    {
        qualityText.text = currentAsset == highQualityAsset ? "High" :
            currentAsset == mediumQualityAsset ? "Medium" : "Low";
    }
}

[Serializable]
public enum QualityLevel
{
    High,
    Medium,
    Low
}