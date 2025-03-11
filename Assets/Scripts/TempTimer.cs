using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TempTimer : MonoBehaviour
{
    private TextMeshProUGUI time;

    private void Start()
    {
        time = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        int totalSeconds = Mathf.FloorToInt(SaveManager.instance.PlayTime);
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;
        time.text = $"{hours:D2} : {minutes:D2} : {seconds:D2}";
    }
}
