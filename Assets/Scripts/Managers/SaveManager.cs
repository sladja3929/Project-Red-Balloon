using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

/*
 * SaveManager는 게임의 저장을 담당하는 클래스입니다.
 * 예상 버그 목록
 *    - 저장한것과 다른 이상한 스테이지가 로드되는데요? ==> 저장할때 buildIndex기반합니다. 지켜주세요
 *    - ㅇ
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
    
    public bool IsNewSave()
    {
        return CheckFlag(SaveFlag.NewSave);
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

    public void ResetSave()
    {
        File.Delete(SAVE_PATH);
        
        curInfo = new SaveInfo
        {
            flagInfo = SaveFlag.NewSave,
            deathCount = 0
        };
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
    
    public void RemoveFlag(SaveFlag flag)
    {
        curInfo.flagInfo &= ~flag;
    }
    
    // ==================== Developer Function ====================
    
    [ContextMenu("Save Immediately")]
    public void SaveImmediately()
    {
        Save();
    }
}

[System.Flags, System.Serializable]
public enum SaveFlag
{
    NewSave        = 1 << 0,
    CutsceneStage1 = 1 << 1,
    CutsceneStage2 = 1 << 2,
    CutsceneStage3 = 1 << 3,
    
    Scene2Bloom   = 1 << 4,
    Scene2Volcano = 1 << 5,
    Scene2AmbientLight = 1 << 6,
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
