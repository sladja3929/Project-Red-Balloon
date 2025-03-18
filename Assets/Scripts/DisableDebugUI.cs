using UnityEngine;
using UnityEngine.Rendering;

public class DisableDebugUI : MonoBehaviour
{
    void Awake()
    {
        if (DebugManager.instance != null)
        {
            DebugManager.instance.enableRuntimeUI = false;
        }
    }
}