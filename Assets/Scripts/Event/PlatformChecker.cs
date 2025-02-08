using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformChecker : MonoBehaviour
{
    [SerializeField] private float checkDistance = 10.0f;
    [SerializeField] private float deathTimer = 10.0f;
    [SerializeField] private Color coldColor;
    [SerializeField] private float colorChangeRate = 0.5f;
    
    private GameObject balloonObj;
    private Renderer balloonRenderer;
    private Color originalColor;
    private LayerMask platformLayerMask;
    private bool isDead;
    private float t;
    
    void Start()
    {
        platformLayerMask = LayerMask.GetMask("Platform");
        balloonObj = GameObject.FindWithTag("Player");
        balloonRenderer = balloonObj.GetComponent<Renderer>();
        originalColor = balloonRenderer.material.color;
        isDead = false;
    }

    void Update()
    {
        Vector3 origin = balloonObj.transform.position;
        Vector3 direction = Vector3.down;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, checkDistance, platformLayerMask))
        {
            if (t > 0)
            {
                t -= Time.deltaTime * 5;
                float LerpFactor = (Mathf.Clamp(t, 0, deathTimer) / deathTimer) * colorChangeRate;
                Color newColor = Color.Lerp(originalColor, coldColor, LerpFactor);
                balloonRenderer.material.color = newColor;
            }

            else
            {
                t = 0;
                balloonRenderer.material.color = originalColor;
            }
            
            isDead = false;
            
            // Optional: Draw a green line in the Scene view for debugging
            Debug.DrawRay(origin, direction * hit.distance, Color.magenta);
        }
        else
        {
            t += Time.deltaTime;
            float LerpFactor = (Mathf.Clamp(t, 0, deathTimer) / deathTimer) * colorChangeRate;
            Color newColor = Color.Lerp(originalColor, coldColor, LerpFactor);
            balloonRenderer.material.color = newColor;
            
            // Optional: Draw a red line in the Scene view for debugging
            Debug.DrawRay(origin, direction * checkDistance, Color.red);
            
            if (t >= deathTimer && !isDead)
            {
                GameManager.instance.KillBalloon();
                isDead = true;
                t = 0;
            }
        }
    }
    
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    public bool isDebug;
    private void Awake()
    {
        if (isDebug) deathTimer = 9999f;
    }
#endif
}
