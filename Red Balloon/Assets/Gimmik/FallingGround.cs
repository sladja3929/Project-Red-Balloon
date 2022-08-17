using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FallingGround : MonoBehaviour
{
    private Rigidbody _rigid;

    public GameObject groundThatWillFall;
    public float respownTime;
    public float fallTime;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        groundThatWillFall = gameObject;
    }


    public bool isBreaking;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPrefab && !isBreaking)
        {
            isBreaking = true;
            Invoke(nameof(Fall), fallTime);
        }
    }

    private void Fall()
    {
        isBreaking = true;
        
        Instantiate(groundThatWillFall, transform.position, Quaternion.identity)
            .GetComponent<FallingGround>().SetPrefabMode();
        gameObject.SetActive(false);
        
        Invoke(nameof(Respawn), respownTime);
    }

    private void Respawn()
    {
        gameObject.SetActive(true);

        isBreaking = false;
    }


    public bool isPrefab = false;
    public float destroyPrefabTime;
    public void SetPrefabMode()
    {
        isPrefab = true;
        _rigid.isKinematic = false;
        GetComponent<Collider>().isTrigger = false;

        Invoke(nameof(DeletePrefab), destroyPrefabTime);
    }

    private void DeletePrefab()
    {
        Destroy(gameObject);
    }
}
