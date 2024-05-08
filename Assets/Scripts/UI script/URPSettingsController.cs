using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class UrpSettingsController : MonoBehaviour
{
    public UniversalRenderPipelineAsset currentAsset;
    
    public UniversalRenderPipelineAsset highQualityAsset;
    public UniversalRenderPipelineAsset mediumQualityAsset;
    public UniversalRenderPipelineAsset lowQualityAsset;

    void Start()
    {
        // URP Asset을 가져옵니다.
        currentAsset = GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset;

        if (currentAsset == null)
        {
            Debug.LogError("URP Asset이 설정되지 않았습니다.");
        }
    }
    
    public void SetQualityLevel(QualityLevel qualityLevel)
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
    }
}

[Serializable]
public enum QualityLevel
{
    High,
    Medium,
    Low
}