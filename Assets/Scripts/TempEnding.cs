using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TempEnding : MonoBehaviour
{
    public GameObject UI;
    public TextMeshProUGUI death;
    public TextMeshProUGUI time;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int totalSeconds = Mathf.FloorToInt(SaveManager.instance.PlayTime);
            int hours = totalSeconds / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            int seconds = totalSeconds % 60;
            death.text = $"죽은 횟수: {SaveManager.instance.DeathCount}번";
            time.text = $"플레이 시간: {hours:D2}시간 {minutes:D2}분 {seconds:D2}초";
            UI.SetActive(true);
        }
    }
}
