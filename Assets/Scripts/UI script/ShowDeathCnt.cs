using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ShowDeathCnt : MonoBehaviour
{
    public TextMeshPro deathCountText;
    
    private int deathCount;
    
    // Start is called before the first frame update
    private void Start()
    {
        ShowDeathCount();
        GameManager.instance.onBalloonDead.AddListener(ShowDeathCount);
    }

    private void ShowDeathCount()
    {
        deathCount = SaveManager.instance.DeathCount;
        deathCountText.text = $"<sprite=0> × {deathCount}";
    }

    private void OnDestroy()
    {
        GameManager.instance.onBalloonDead.RemoveListener(ShowDeathCount);
    }
}
