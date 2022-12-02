using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isPause = false;
    
    [SerializeField] private Vector3 savePoint;

    private GameObject _balloonObj;
    private Rigidbody _balloonRigid;
    private Respawn _balloonSpawn;

    private void Awake()
    {
        _balloonObj = GameObject.FindWithTag("Player");

        _balloonRigid = _balloonObj.GetComponent<Rigidbody>();
        _balloonSpawn = _balloonObj.GetComponent<Respawn>();
    }

    public void SetSavePoint(Vector3 point) {
        savePoint = point;
    }
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
