using System;
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
    private const string SAVE_PATH = "save.whs"; // .whs 확장자로 변경

    [SerializeField]
    private SaveInfo curInfo;

    public int BuildIndex
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

    public float PlayTime
    {
        get => curInfo.playTime;
        set => curInfo.playTime = value;
    }

    private new void Awake()
    {
        base.Awake();
        curInfo = Load();
    }

    public bool IsNewSave()
    {
        return !File.Exists(SAVE_PATH) || CheckFlag(SaveFlag.NewSave);
    }

    // 🔹 Binary Save (JSON 대신 바이너리로 저장)
    public void Save()
    {
        using (FileStream fs = new FileStream(SAVE_PATH, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(fs))
        {
            writer.Write(curInfo.stage);
            writer.Write(curInfo.position.x);
            writer.Write(curInfo.position.y);
            writer.Write(curInfo.position.z);
            writer.Write((int)curInfo.flagInfo);
            writer.Write(curInfo.deathCount);
            writer.Write(curInfo.playTime);
            writer.Write((int)curInfo.skinData);
        }
    }

    public SaveInfo GetSaveInfo()
    {
        return curInfo;
    }

    public void ResetSave()
    {
        //File.Delete(SAVE_PATH); 스팀판은 삭제하면 안됨

        curInfo = new SaveInfo
        {
            flagInfo = SaveFlag.NewSave,
            deathCount = 0,
            playTime = 0,
            position = new Vector3(0, 0, 0),
            stage = 0
        };

        Save();
    }

    // 🔹 Binary Load (JSON 대신 바이너리로 불러오기)
    private static SaveInfo Load()
    {
        if (!File.Exists(SAVE_PATH))
        {
            return new SaveInfo
            {
                flagInfo = SaveFlag.NewSave,
                deathCount = 0,
                playTime = 0
            };
        }

        using (FileStream fs = new FileStream(SAVE_PATH, FileMode.Open))
        using (BinaryReader reader = new BinaryReader(fs))
        {
            long fileLength = reader.BaseStream.Length;
            
            SaveInfo loaded = new SaveInfo
            {
                stage = reader.ReadInt32(),
                position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()),
                flagInfo = (SaveFlag)reader.ReadInt32(),
                deathCount = reader.ReadInt32(),
                playTime = reader.ReadSingle(),
                skinData = (SkinFlag)1
            };

            //파일 길이가 더 길면 스킨 데이터 추가로 읽기
            if (reader.BaseStream.Position + sizeof(int) <= fileLength)
            {
                loaded.skinData = (SkinFlag)reader.ReadInt32();
            }
            
            return loaded;
        }
    }

    private void OnApplicationQuit()
    {
        Save();
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
    
    // ==================== Skin 관련 함수 ====================

    public bool CheckSkin(SkinFlag flag)
    {
        return (curInfo.skinData & flag) == flag;
    }

    public void UnlockSkin(SkinFlag flag)
    {
        curInfo.skinData |= flag;
    }

    public void LockSkin(SkinFlag flag)
    {
        curInfo.skinData &= ~flag;
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

[System.Flags, System.Serializable]
public enum SkinFlag
{
    Red = 1 << 0,
    Smile = 1 << 1,
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
    public float playTime;
    public SkinFlag skinData;
}
