using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SteamManager.instance.UnlockAchievement(SteamAchievements.ACH_EGG_1);
        }
    }
}
