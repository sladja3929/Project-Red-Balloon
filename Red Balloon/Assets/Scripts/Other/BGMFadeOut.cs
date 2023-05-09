using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMFadeOut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SoundManager.Instance.StartCoroutine("BackGroundFadeOut");
        }
    }
}
