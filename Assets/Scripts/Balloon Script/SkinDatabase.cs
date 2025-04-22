using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinDatabase", menuName = "Game/SkinDataBase")]
public class SkinDatabase : ScriptableObject
{
    public Material[] MainSkinMaterials;
    public Material[] ManSkinMaterials;

    public Material GetMainSkin(int index)
    {
        if (index >= 0 && index < MainSkinMaterials.Length)
            return MainSkinMaterials[index];

        Debug.LogWarning("Invalid skin index (Main)");
        return null;
    }
    
    public Material GetManSkin(int index)
    {
        if (index >= 0 && index < ManSkinMaterials.Length)
            return ManSkinMaterials[index];

        Debug.LogWarning("Invalid skin index (Man)");
        return null;
    }
}
