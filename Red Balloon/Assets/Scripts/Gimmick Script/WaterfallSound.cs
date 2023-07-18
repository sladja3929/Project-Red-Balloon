using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfallSound : MonoBehaviour
{
    [SerializeField] private bool isBig;

    private AudioSource _waterFallSound;
    private void Awake()
    {
        _waterFallSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(isBig) _waterFallSound.volume = SoundManager.instance.GetSfxSoundVolume();
        else _waterFallSound.volume = SoundManager.instance.GetSfxSoundVolume() * 0.7f;
    }
}
