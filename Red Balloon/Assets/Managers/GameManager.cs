using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isPause = false;
    
    [SerializeField] private Vector3 savePoint;

    public void SetSavePoint(Vector3 point)
    {
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
