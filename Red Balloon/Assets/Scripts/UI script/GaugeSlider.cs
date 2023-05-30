using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GaugeSlider : MonoBehaviour
{
    private Slider _slider;

    private BalloonController _balloon;

    void Awake()
    {
        _slider = GetComponent<Slider>();
        _balloon = GameObject.FindWithTag("Player").GetComponent<BalloonController>();
    }

    // Update is called once per frame
    void Update()
    {
        _slider.value = _balloon.GetChargeGauge();
    }
}
