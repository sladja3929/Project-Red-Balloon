using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfallSound : MonoBehaviour
{
    [SerializeField] private bool isBig;

    private AudioSource waterFallSound;
    private void Awake()
    {
        waterFallSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(isBig) waterFallSound.volume = SoundManager.instance.GetSfxSoundVolume();
        else waterFallSound.volume = SoundManager.instance.GetSfxSoundVolume() * 0.7f;
    }
}
