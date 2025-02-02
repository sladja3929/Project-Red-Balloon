using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Random = UnityEngine.Random;

public class Volcano : Gimmick
{
    private const float FALLING_TIME = 3.2f;
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private ParticleSystem gasEffect;
    
    [Space(10)]
    [SerializeField] private float degree;
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float fallingDelay;
    
    [SerializeField] private float spawnInterval;
    [SerializeField] private float CameraShakeAmount;
    
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip shakeGroundSound;

    private Vector3 balloonPos;
    
    private bool isSpawning = false;
    
    private float t;

    private void Start()
    {
        //isGimmickEnable = SaveManager.instance.CheckFlag(SaveFlag.Scene2Volcano);
        isGimmickEnable = false;
        t = 0;
    }
    
    private void Update()
    {
        if (!isGimmickEnable) return;
        if (isSpawning) return;
        
        t += Time.deltaTime;
        if (t >= spawnInterval && isGimmickEnable)
        {
            isSpawning = true;
            
            ShakeCamera();
            GasEffect();
            Invoke(nameof(SpawnSingleStone), fallingDelay);
        }
    }

    private void ShakeCamera()
    {
        CinemachineVirtualCamera virtualCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
        if (virtualCamera != null)
        {
            StartCoroutine(ShakeCamera(virtualCamera.transform, CameraShakeAmount, 1.0f)); // Adjust the amount and time as needed
        }
        
        balloonPos = GameManager.instance.GetBalloonPosition();
        
        SoundManager.instance.SfxPlay("Volcano sound", explosionSound, transform, 1f, int.MaxValue, int.MaxValue);
        //SoundManager.instance.SfxPlay("Shake ground sound", shakeGroundSound, transform, int.MaxValue, int.MaxValue);
    }
    
    private void GasEffect()
    {
        gasEffect.Play();
    }

    [ContextMenu("돌떨어져유")]
    private void SpawnSingleStone()
    {
        Vector3 volcanoPosition = transform.position;

        // balloon과 화산의 좌표 연산해서 날아오는 방향 계산하기
        Vector3 direction = balloonPos - volcanoPosition;
        direction.y = 0;
        direction.Normalize();

        // 해당 방향에서 <변경 가능한 변수> 각도만큼 기울어진체로 방향 생성
        Vector3 normalVector = Vector3.Cross(Vector3.up, direction).normalized;
        Quaternion rotation = Quaternion.AngleAxis(degree, normalVector);
        direction = rotation * direction;

        float length = fallingSpeed * FALLING_TIME;
        Vector3 stoneSpawnPoint = balloonPos - direction * length;

        VolcanicStone stone = Instantiate(stonePrefab, stoneSpawnPoint, Quaternion.identity).GetComponent<VolcanicStone>();
        stone.Fall(direction, fallingSpeed);
        
        isSpawning = false;
        t = 0;
    }

    private IEnumerator ShakeCamera(Transform cameraTransform, float amount, float time)
    {
        Vector3 originalPos = cameraTransform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < time)
        {
            float x = Random.Range(-1f, 1f) * amount;
            float y = Random.Range(-1f, 1f) * amount;

            cameraTransform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraTransform.localPosition = originalPos;
    }
    
    public override void GimmickOn()
    {
        isGimmickEnable = true;
        
        SaveManager.instance.SetFlag(SaveFlag.Scene2Volcano);
        SaveManager.instance.Save();
    }

    public void CutSceneEvent()
    {
        ShakeCamera();
        GasEffect();
    }
}