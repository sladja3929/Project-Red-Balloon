using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Application = UnityEngine.Application;
using System;

public class GameManager : Singleton<GameManager>
{
    private new void Awake()
    {
        base.Awake();
        _balloonObj = GameObject.FindWithTag("Player");
        SetBalloon(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        //records = new List<float>();

        SceneManager.sceneLoaded += SetBalloon;
    }
    
    private void SetBalloon(Scene arg0, LoadSceneMode arg1)
    {
        _balloonObj = GameObject.FindWithTag("Player");
        if (_balloonObj == null) return;

        _balloonRigid = _balloonObj.GetComponent<Rigidbody>();
        _balloonSpawn = _balloonObj.GetComponent<Respawn>();
        Debug.Log("asd");
    }

    public static bool isPause
    {
        get => Time.timeScale == 0;
        set => Time.timeScale = value ? 0 : 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    //현재 Spawn Point를 디버깅하기 위한 변수
    [SerializeField] private Vector3 savePoint;

    private GameObject _balloonObj;
    private Rigidbody _balloonRigid;
    private Respawn _balloonSpawn;
    public void SetSavePoint(Vector3 point) 
    {
        savePoint = point;
        _balloonSpawn.SetSavePoint(point);
    }

    public void KillBalloon()
    {
        _balloonSpawn.Die();
    }

    [SerializeField] private float startTime;
    //public List<float> records;
    public string record;
    public GameObject endCanvas;
    public void StartGame()
    {
        startTime = Time.time;
    }

    public void FinishGame()
    {
        //records.Add(Time.time - startTime);
        //records.Sort();
        TimeSpan t = TimeSpan.FromSeconds(Time.time - startTime);

        record = "score: " + $"{t.Hours:D2} h {t.Minutes:D2} m {t.Seconds:D2} s";

        isPause = true;
        endCanvas.SetActive(true);
    }
    public static void GoToMainMenu()
    {
        SceneChangeManager.instance.LoadSceneAsync("MainMenu");
        //
        // SceneChangeManager.instance.StartCoroutine(nameof(FinishGame), "MainMenu");
        isPause = false;
    }

    public Vector3 GetBalloonPosition()
    {
        return _balloonObj.transform.position;
    }
}