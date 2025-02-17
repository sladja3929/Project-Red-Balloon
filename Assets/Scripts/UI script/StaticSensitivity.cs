using UnityEngine;

public static class StaticSensitivity
{
    private static float mouseSensitivity = 1f;
    private const float MAX_MOUSE_SENSITIVITY = 50f;
    private const float MIN_MOUSE_SENSITIVITY = 1f;
    
    private static float camSensitivity = 1f;
    private const float MAX_CAM_SENSITIVITY = 10;
    private const float MIN_CAM_SENSITIVITY = 0.1f;
    
    public static float MouseSensitivity => 
        PlayerPrefs.HasKey("MouseSensitivity") ? PlayerPrefs.GetFloat("MouseSensitivity") : mouseSensitivity;
    public static float CamSensitivity => 
        PlayerPrefs.HasKey("CamSensitivity") ? PlayerPrefs.GetFloat("CamSensitivity") : camSensitivity;
    
    public static void SetMouseSensitivity(float rate)
    {
        mouseSensitivity = Mathf.Lerp(MIN_MOUSE_SENSITIVITY, MAX_MOUSE_SENSITIVITY, rate);
        
        //save to playerprefs
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);
    }
    
    public static void SetCamSensitivity(float rate)
    {
        camSensitivity = Mathf.Lerp(MIN_CAM_SENSITIVITY, MAX_CAM_SENSITIVITY, rate);
        
        //save to playerprefs
        PlayerPrefs.SetFloat("CamSensitivity", camSensitivity);
    }
    
    public static float GetMouseSensitivityRate()
    {
        return (mouseSensitivity - MIN_MOUSE_SENSITIVITY) / (MAX_MOUSE_SENSITIVITY - MIN_MOUSE_SENSITIVITY);
    }
    
    public static float GetCamSensitivityRate()
    {
        return (camSensitivity - MIN_CAM_SENSITIVITY) / (MAX_CAM_SENSITIVITY - MIN_CAM_SENSITIVITY);
    }
}