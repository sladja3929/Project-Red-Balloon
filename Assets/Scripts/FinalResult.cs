using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalResult : MonoBehaviour
{
    public TextMeshProUGUI death;
    public TextMeshProUGUI time;

    private float finalTime;

    public float FinalTime
    {
        set => finalTime = value;
        get => finalTime;
    }

    private int finalCount;

    public int FinalCount
    {
        set => finalCount = value;
        get => finalCount;
    }

    private void OnEnable()
    {
        int totalSeconds = Mathf.FloorToInt(finalTime);
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;
        death.text = $"Pop Count: {finalCount}";
        time.text = $"Play Time: {hours:D2}h {minutes:D2}m {seconds:D2}s";
    }
}
