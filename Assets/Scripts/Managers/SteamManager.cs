using System;
using Steamworks;
using UnityEngine;

[DefaultExecutionOrder(-5)]
public sealed class SteamStats
{
    public string Name { get; }
    
    /// <summary>
    /// int
    /// </summary>
    public static readonly SteamStats DeathCount = new SteamStats("DeathCount");
    
    /// <summary>
    /// int
    /// </summary>
    public static readonly SteamStats RestartCount = new SteamStats("RestartCount");
    
    /// <summary>
    /// int (-1 ~ 3)
    /// </summary>
    public static readonly SteamStats ClearStage = new SteamStats("ClearStage");
    
    /// <summary>
    /// int
    /// </summary>
    public static readonly SteamStats ClearCount = new SteamStats("ClearCount");
    
    /// <summary>
    /// float
    /// </summary>
    public static readonly SteamStats BestRecord = new SteamStats("BestRecord");
    
    public static readonly SteamStats[] All = { };

    private SteamStats(string name)
    {
        Name = name;
    }

    public override string ToString() => Name;
}

public sealed class SteamAchievements
{
    public string Name { get; }

    public static readonly SteamAchievements ACH_CLEAR_0 = new SteamAchievements("ACH_CLEAR_0");
    public static readonly SteamAchievements ACH_CLEAR_1 = new SteamAchievements("ACH_CLEAR_1");
    public static readonly SteamAchievements ACH_CLEAR_2 = new SteamAchievements("ACH_CLEAR_2");
    public static readonly SteamAchievements ACH_CLEAR_3 = new SteamAchievements("ACH_CLEAR_3");
    public static readonly SteamAchievements ACH_RESTART = new SteamAchievements("ACH_RESTART");
    public static readonly SteamAchievements ACH_EGG_1 = new SteamAchievements("ACH_EGG_1");
    public static readonly SteamAchievements ACH_SPEEDRUN_1 = new SteamAchievements("ACH_SPEEDRUN_1");
    
    public static readonly SteamAchievements[] All = { };

    private SteamAchievements(string name)
    {
        Name = name;
    }

    public override string ToString() => Name;
}

public class SteamManager : Singleton<SteamManager>
{
    private bool _isInitialized = false;
    public bool IsInitialized => _isInitialized;
    
    private bool _statsLoaded = false;
    public bool StatsLoaded => _statsLoaded;

    private string _steamLanguage;
    public string SteamLanguage => _steamLanguage;
    
    protected Callback<UserStatsReceived_t> userStatsReceived;
    protected new void Awake()
    {
        base.Awake();

        try
        {
            _isInitialized = SteamAPI.Init();
            if (!_isInitialized)
            {
                Debug.LogError("스팀API 초기화 실패. 스팀 클라이언트 실행 확인 필요");
                return;
            }

            _steamLanguage = SteamApps.GetCurrentGameLanguage()?.ToLower();
            SteamUserStats.RequestCurrentStats();
            Debug.Log("스팀API 초기화 성공.");
        }
        catch (Exception e)
        {
            Debug.LogError("스팀API 초기화 중 오류 발생: " + e.Message);
        }
    }

    private void Update()
    {
        if (_isInitialized)
        {
            SteamAPI.RunCallbacks();
        }
    }

    private void OnApplicationQuit()
    {
        if (_isInitialized)
        {
            SteamAPI.Shutdown();
        }
    }

    private bool ValiateAPI()
    {
        if (!_isInitialized || !SteamUser.BLoggedOn())
        {
            Debug.LogError("스팀API가 초기화되지 않았거나 사용자가 로그인하지 않았습니다.");
            return false;
        }

        return true;
    }
    
    // 도전과제 처리 ----------------------------------------------------------------------
    public bool UnlockAchievement(SteamAchievements id)
    {
        if (!ValiateAPI()) return false;
        
        bool unlocked = false;
        SteamUserStats.GetAchievement(id.ToString(), out unlocked);
        if (unlocked) return true;

        bool success = SteamUserStats.SetAchievement(id.ToString());
        if(success)
        {
            SteamUserStats.StoreStats();
            Debug.Log($"Achievement {id.ToString()} compelete");
        }

        return success;
    }

    // 통계 처리 ----------------------------------------------------------------------
    public bool SetStatInt(SteamStats name, int value)
    {
        if (!ValiateAPI()) return false;
        
        bool success = SteamUserStats.SetStat(name.ToString(), value);
        if (success)
        {
            SteamUserStats.StoreStats();
            Debug.Log($"Stat {name.ToString()} update: {value}");
        }

        return success;
    }
    
    public bool SetStatFloat(SteamStats name, float value)
    {
        if (!ValiateAPI()) return false;
        
        bool success = SteamUserStats.SetStat(name.ToString(), value);
        if (success)
        {
            SteamUserStats.StoreStats();
            Debug.Log($"Stat {name.ToString()} update: {value}");
        }

        return success;
    }

    public int GetStatInt(SteamStats name)
    {
        if (!ValiateAPI()) return -2;

        int value = -2;
        SteamUserStats.GetStat(name.ToString(), out value);
        return value;
    }
    
    public float GetStatFloat(SteamStats name)
    {
        if (!ValiateAPI()) return -2f;

        float value = -2f;
        SteamUserStats.GetStat(name.ToString(), out value);
        return value;
    }
    
    // 외부 호출 함수 ----------------------------------------------------------------------
    public void UpdateDeathCount()
    {
        int deathCount = GetStatInt(SteamStats.DeathCount);
        if(deathCount != -2) SetStatInt(SteamStats.DeathCount, deathCount + 1);
    }

    public void UpdateRestartCount()
    {
        int restartCount = GetStatInt(SteamStats.RestartCount);
        if (restartCount != -2)
        {
            SetStatInt(SteamStats.RestartCount, restartCount + 1);

            if (restartCount + 1 >= 100) UnlockAchievement(SteamAchievements.ACH_RESTART);
        }
    }

    public void UpdateClearStage(int stage)
    {
        int statStage = GetStatInt(SteamStats.ClearStage);
        if (statStage != -2 && statStage < stage)
        {
            SetStatInt(SteamStats.ClearStage, stage);

            switch (stage)
            {
                case 0: UnlockAchievement(SteamAchievements.ACH_CLEAR_0); break;
                case 1: UnlockAchievement(SteamAchievements.ACH_CLEAR_1); break;
                case 2: UnlockAchievement(SteamAchievements.ACH_CLEAR_2); break;
                case 3: UnlockAchievement(SteamAchievements.ACH_CLEAR_3); break;
            }
        }
    }

    public void UpdateClearCount()
    {
        int clearCount = GetStatInt(SteamStats.ClearCount);
        if(clearCount != -2) SetStatInt(SteamStats.ClearCount, clearCount + 1);
    }
    
    public void UpdateBestRecord(float time)
    {
        float statRecord = GetStatFloat(SteamStats.BestRecord);
        if (statRecord >= -2f)
        {
            if (statRecord > time)
                SetStatFloat(SteamStats.BestRecord, time);
        }
        
        if (7200f > Mathf.FloorToInt(time))
            UnlockAchievement(SteamAchievements.ACH_SPEEDRUN_1);
    }

    public string RefreshSteamLanguage()
    {
        _steamLanguage = "english";
        if (!ValiateAPI())
        {
            return _steamLanguage;
        }

        string newLang = SteamApps.GetCurrentGameLanguage();
        if (!string.IsNullOrEmpty(newLang))
        {
            _steamLanguage = newLang.ToLower();
            return _steamLanguage;
        }

        return _steamLanguage;
    }
}
