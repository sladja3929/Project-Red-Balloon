using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class StoneFall : Gimmick
{
    // Shoot Ray cast, and if it hits the player, set rigidbody's kinematic to false
    
    public GameObject stone;
    
    public float squareHeight = 10f;
    public float squareWidth = 1f;
    public float force = 1f;
    
    private Rigidbody rb;
    private bool isActive = false;
    
    private void Start()
    {
        rb = stone.GetComponent<Rigidbody>();
    }
    
    public void Update()
    {
        if (!isActive && DetectBalloon())
        {
            isActive = true;
            rb.isKinematic = false;
            rb.AddForce(-transform.up * force, ForceMode.Impulse);
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        //kill player
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.KillBalloon();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //kill player
        if (other.CompareTag("Player"))
        {
            GameManager.instance.KillBalloon();
        }
    }
    
    public bool DetectBalloon()
    {
        //detect by OnSquare    
        Vector3 pos = transform.position;
        
        Vector3 size = new Vector3(squareWidth, squareHeight, squareWidth);
        Vector3 halfSize = size / 2;
        Vector3 halfSizeWithHeight = new Vector3(halfSize.x, halfSize.y + squareHeight, halfSize.z);
        
        var colliders = Physics.OverlapBox(pos, halfSizeWithHeight, Quaternion.identity);
        return colliders.Any(col => col.CompareTag("Player"));
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position,  
            new Vector3(squareWidth, squareHeight, squareWidth)
            );
    }
}
