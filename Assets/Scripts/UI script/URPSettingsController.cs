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
    public Image currentQualityImage;
    
    // Quality Images[0] = High, [1] = Medium, [2] = Low
    // 해당 내용을 설명하는 텍스트가 유니티 Editor에서 표시되도록 하는 Attribute
    [Tooltip("Quality Images[0] = High, [1] = Medium, [2] = Low")]
    public Sprite[] qualityImages;

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
        
        if (qualityImages.Length != 3)
        {
            Debug.LogError("Quality Images의 개수가 3개가 아닙니다.");
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
        if (currentAsset == highQualityAsset)
        {
            currentQualityImage.sprite = qualityImages[0];
        }
        else if (currentAsset == mediumQualityAsset)
        {
            currentQualityImage.sprite = qualityImages[1];
        }
        else if (currentAsset == lowQualityAsset)
        {
            currentQualityImage.sprite = qualityImages[2];
        }
    }
}

[Serializable]
public enum QualityLevel
{
    High,
    Medium,
    Low
}