using UnityEngine;

public static class StaticSensitivity
{
    private static float mouseSensitivity = 6f;
    private const float MAX_MOUSE_SENSITIVITY = 20f;
    private const float MIN_MOUSE_SENSITIVITY = 0.5f;
    
    private static float camSensitivity = 1.625f;
    private const float MAX_CAM_SENSITIVITY = 6f;
    private const float MIN_CAM_SENSITIVITY = 0.5f;

    static StaticSensitivity()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", mouseSensitivity);
        camSensitivity = PlayerPrefs.GetFloat("CamSensitivity", camSensitivity);
    }
    
    public static float MouseSensitivity => mouseSensitivity;
    public static float CamSensitivity => camSensitivity;
    
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