using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DirectionalLight : Singleton<DirectionalLight>
{
    private Light _light;

    private new void Awake()
    {
        base.Awake();

        _light = GetComponent<Light>();
    }
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Stage1")
        {
            _light.color = Color.white;
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
