using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfallSound : MonoBehaviour
{
    private AudioSource waterFallSound;
    private void Awake()
    {
        waterFallSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        waterFallSound.volume = SoundManager.Instance.GetSfxSoundVolume();
    }
}
