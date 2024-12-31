using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

/*
 * SaveManager는 게임의 저장을 담당하는 클래스입니다.
 * 예상 버그 목록
 *    - 저장한것과 다른 이상한 스테이지가 로드되는데요? ==> 저장할때 buildIndex기반합니다. 지켜주세요
 *    
 */

public class SaveManager : Singleton<SaveManager>
{
    private const string SAVE_PATH = "save.json";

    [SerializeField]
    private SaveInfo curInfo;

    public int Stage
    {
        get => curInfo.stage;
        set => curInfo.stage = value;
    }
    
    public Vector3 Position
    {
        get => curInfo.position;
        set => curInfo.position = value;
    }

    public int DeathCount
    {
        get => curInfo.deathCount;
        set => curInfo.deathCount = value;
    }

    private new void Awake()
    {
        base.Awake();
        
        curInfo = Load();
    }
    
    // save Info를 JsonData로 저장합니다.
    public void Save()
    { 
        string file = JsonUtility.ToJson(curInfo);
        File.WriteAllText(SAVE_PATH, file);
    }
    
    public SaveInfo GetSaveInfo()
    {
        return curInfo;
    }

    public static void ResetSave()
    {
        File.Delete(SAVE_PATH);
    }

    private static SaveInfo Load()
    {
        if (!File.Exists(SAVE_PATH))
        {
            SaveInfo ret = new SaveInfo
            {
                flagInfo = SaveFlag.NewSave,
                deathCount = 0
            };

            return ret;
        }
        
        string file = File.ReadAllText(SAVE_PATH);
        return JsonUtility.FromJson<SaveInfo>(file);
    }
    
    // ==================== Flag 관련 함수 ====================
    
    public bool CheckFlag(SaveFlag flag)
    {
        return (curInfo.flagInfo & flag) == flag;
    }

    public void SetFlag(SaveFlag flag)
    {
        curInfo.flagInfo |= flag;
    }
    
    public void ResetFlag(SaveFlag flag)
    {
        curInfo.flagInfo &= ~flag;
    }
}

[System.Flags, System.Serializable]
public enum SaveFlag
{
    CutsceneStage1 = 1 << 0,
    CutsceneStage2 = 1 << 1,
    CutsceneStage3 = 1 << 2,
    
    Scene2Bloom = 1 << 3,
    
    NewSave = 1 << 4,
    //. .
    //. .
    //. .
}

[System.Serializable]
public struct SaveInfo
{
    public int stage;
    public Vector3 position;
    public SaveFlag flagInfo;
    public int deathCount;
}
