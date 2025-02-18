using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvertAim : MonoBehaviour
{
    [Header("UI")]
    public Button rightButton;
    public Button leftButton;
    
    public TMP_Text aimText;

    private void Awake()
    {
        rightButton.onClick.RemoveAllListeners();
        rightButton.onClick.AddListener(OnClickButton);
        
        leftButton.onClick.RemoveAllListeners();
        leftButton.onClick.AddListener(OnClickButton);
    }

    void Start()
    { 
        GameManager.instance.InvertAim = PlayerPrefs.GetInt("InvertAim", 1) == 1;
        
        ReloadText();
    }

    private void OnClickButton()
    {
        GameManager.instance.InvertAim = !GameManager.instance.InvertAim;
        
        int value = GameManager.instance.InvertAim ? 1 : 0;
        PlayerPrefs.SetInt("InvertAim", value);
        
        ReloadText();
    }
    
    private void ReloadText()
    {
        aimText.text = GameManager.instance.InvertAim ? "On" : "Off";
    }
}
