using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Application = UnityEngine.Application;
using System;
using UnityEngine.Events;

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

    private void Update()
    {
        // if not playing continue
        if (Time.timeScale == 0) return;
        if (_balloonObj == null) return;
        if (SceneManager.GetActiveScene().name == "MainMenu") return;
        
        if(!IsCinematic) SaveManager.instance.PlayTime += Time.deltaTime;
    }
    
    private void SetBalloon(Scene arg0, LoadSceneMode arg1)
    {
        _balloonObj = GameObject.FindWithTag("Player");
        if (_balloonObj == null) return;

        _balloonRigid = _balloonObj.GetComponent<Rigidbody>();
        _balloonSpawn = _balloonObj.GetComponent<Respawn>();
        _balloonController = _balloonObj.GetComponent<BalloonController>();
        _balloonDrag = _balloonObj.GetComponent<DragRotation>();
    }

    public static bool IsPause
    {
        get => Time.timeScale == 0;
        set => Time.timeScale = value ? 0 : 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private bool invertAim = true;
    public bool InvertAim
    {
        get
        {
            if (_balloonDrag != null) invertAim = _balloonDrag.isReverse;
            return invertAim;
        }

        set
        {
            if (_balloonDrag != null) _balloonDrag.isReverse = value;
            invertAim = value;
        }
    }
    
    //현재 Spawn Point를 디버깅하기 위한 변수
    [SerializeField] private Vector3 savePoint;
    
    private GameObject _balloonObj;
    private Rigidbody _balloonRigid;
    private Respawn _balloonSpawn;
    private BalloonController _balloonController;
    private DragRotation _balloonDrag;
    
    public void SetSavePoint(Vector3 point)
    {
        savePoint = point;
        SaveManager.instance.BuildIndex = SceneManager.GetActiveScene().buildIndex;
        SaveManager.instance.Position = point;
        SaveManager.instance.RemoveFlag(SaveFlag.NewSave);
        SaveManager.instance.Save();
    }

    public Vector3 GetSavePoint()//기존 세이브포인트 반환
    {
        return savePoint;
    }

    private bool canDie = true;
    public bool CanDie
    {
        get { return canDie; }
        set { canDie = value; }
    }
    
    public void KillBalloon()
    {
        if(canDie)
        _balloonSpawn.Die();
    }
    
    public static void GoToMainMenu()
    {
        SceneChangeManager.instance.LoadSceneAsync("MainMenu");
        //
        // SceneChangeManager.instance.StartCoroutine(nameof(FinishGame), "MainMenu");
        IsPause = false;
    }

    public Vector3 GetBalloonPosition()
    {
        return _balloonObj.transform.position;
    }

    public bool CanBalloonMove()
    {
        if (_balloonController.GetBalloonState() == BalloonController.BalloonState.Aim ||
            _balloonController.GetBalloonState() == BalloonController.BalloonState.Charge)
        {
            return true;
        }
        else return false;
    }
    
    /// <summary>
    /// 플랫폼에서 떨어지지 않을 경우 FallToAimForced 함수와 같이 사용할것
    /// </summary>
    public void AimToFallForced()
    {
        _balloonController.SetOnPlatform(false);
        _balloonController.SetBasicState();
    }

    public void FallToAimForced()
    {
        _balloonController.SetOnPlatform(true);
    }
    
    public bool IsCinematic = false;
    
    public void CinematicMode()
    {
        _balloonController.SetCinematicState();
        IsCinematic = true;
    }
    
    public void FreezeBalloon()
    {
        _balloonController.SetFreezeState();
    }

    [HideInInspector]
    public UnityEvent onBalloonDead;

    public void BalloonDeadEvent()
    {
        onBalloonDead?.Invoke();
    }
    
    [HideInInspector]
    public UnityEvent onBalloonRespawn;

    public void BalloonRespawnEvent()
    {
        onBalloonRespawn?.Invoke();
    }
}
