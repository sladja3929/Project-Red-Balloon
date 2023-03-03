using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Awake()
    {
        _balloonObj = GameObject.FindWithTag("Player");

        _balloonRigid = _balloonObj.GetComponent<Rigidbody>();
        _balloonSpawn = _balloonObj.GetComponent<Respawn>();
    }
    
    public bool isPause = false;

    public void Pause()
    {
        isPause = true;
        Time.timeScale = 0;
    }
    public void Continue()
    {
        isPause = false;
        Time.timeScale = 1f;
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
}
