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
        time.text = $"플레이 시간: {hours:D2}시간 {minutes:D2}분 {seconds:D2}초";
    }
}
