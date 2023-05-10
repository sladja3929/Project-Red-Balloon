using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTutorial : MonoBehaviour
{
    [SerializeField] GameObject Image;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Image.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Image.SetActive(false);
        }
    }
}
