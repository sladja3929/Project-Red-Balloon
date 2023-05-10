using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GaugeSlider : MonoBehaviour
{
    private Slider _slider;

    private BalloonController balloon;

    void Awake()
    {
        _slider = GetComponent<Slider>();
        balloon = GameObject.FindWithTag("Player").GetComponent<BalloonController>();
    }

    // Update is called once per frame
    void Update()
    {
        _slider.value = balloon.GetChargeGauge();
    }
}
