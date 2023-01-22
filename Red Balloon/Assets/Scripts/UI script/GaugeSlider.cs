using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeSlider : MonoBehaviour
{
    private Slider _slider;

    public BalloonController balloon;

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
