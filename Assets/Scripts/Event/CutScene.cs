using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CutScene : MonoBehaviour
{
    [Serializable]
    protected struct CameraMovement
    {
        public CinemachineVirtualCamera cutSceneCamera;
        public CinemachineDollyCart dollyCart;
        public float timeToMove;
    }

    [SerializeField] protected CameraMovement[] cameraMovements;
    [SerializeField] protected ParticleSystem[] particleObject;
    [SerializeField] protected AudioClip[] soundEffect;
    [SerializeField] protected bool hasToStay;

    protected WaitUntil waitingFadeFinish;
    
    protected float _curTime;
    protected Vector3[] _curvePoints;
    protected bool _isRotation;
    
    protected Coroutine myCoroutine;
    protected bool hasExecuted;

    protected GameObject balloon;
    
    protected virtual void Awake()
    {
        waitingFadeFinish = new WaitUntil(SceneChangeManager.instance.FinishFade);
        foreach (var movement in cameraMovements)
        {
            if (movement.dollyCart != null)
            {
                movement.dollyCart.m_Speed = 1f / movement.timeToMove;
                movement.dollyCart.enabled = false;
            }
            
        }
        hasExecuted = false;
    }

    // Start is called before the first frame update
    protected virtual void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && hasToStay && !hasExecuted)
        {
            if (GameManager.instance.CanBalloonMove())
            {
                balloon = other.gameObject;
                GameManager.instance.FreezeBalloon();
                hasExecuted = true;
                Debug.Log("parent");
            }
        }
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !hasToStay)
        {
            balloon = other.gameObject;
            hasExecuted = true;
        }
    }
    
    enum CameraState
    {
        Stop,
        Moving,
        AlmostFinish,
    }
    
    [SerializeField] private CameraState _cutSceneCameraState;
}
