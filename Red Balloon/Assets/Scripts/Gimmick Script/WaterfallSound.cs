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
        if(isBig) waterFallSound.volume = SoundManager.Instance.GetSfxSoundVolume();
        else waterFallSound.volume = SoundManager.Instance.GetSfxSoundVolume() * 0.7f;
    }
}
