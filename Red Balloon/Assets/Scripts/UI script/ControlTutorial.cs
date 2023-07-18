using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTutorial : MonoBehaviour
{
    [SerializeField] private GameObject image;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (image is null) image = GameObject.Find("Controll");
            image.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (image is null) image = GameObject.Find("Controll");
            image.SetActive(false);
        }
    }
}
