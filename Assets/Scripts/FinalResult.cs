using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalResult : MonoBehaviour
{
    public TextMeshProUGUI death;
    public TextMeshProUGUI time;
    
    private void OnEnable()
    {
        int totalSeconds = Mathf.FloorToInt(SaveManager.instance.PlayTime);
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;
        death.text = $"Pop Count: {SaveManager.instance.DeathCount}";
        time.text = $"Play Time: {hours:D2}h {minutes:D2}m {seconds:D2}s";
    }
}
