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
            image.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            image.SetActive(false);
        }
    }
}
