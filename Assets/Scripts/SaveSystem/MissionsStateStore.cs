using UnityEngine;

public static class MissionsStateStore
{
    
    private static bool[] missionsEnabled = {
        // By default enable only the first level
        true, 
        false, 
        false
    };
    
    private static int currentMissionIdx;

    // Flag used to aboard editing source file, if wasn't loaded initially
    // This may not be loaded, when a mission scene was loaded directly, without going through MainMenu scene
    private static bool loaded = false;
    
    public static void Load()
    {
        loaded = true;
        
        MissionsSaveData data = SaveLoadUtil.TryLoadMissionData();

        if (data != null)
        {
            missionsEnabled = data.missionEnabled;
        }
    }

    public static void SetMissionEnabled(int missionIdx, bool enabled)
    {
        // Check if it's setting to different value, so it doesn't save if nothing changed
        if (missionsEnabled[missionIdx] != enabled)
        {
            missionsEnabled[missionIdx] = enabled;
            Save();
        }
    }
    
    public static bool GetMissionEnabled(int missionIdx)
    {
        return missionsEnabled[missionIdx];
    }

    public static void SetCurrentMissionIdx(int idx)
    {
        currentMissionIdx = Mathf.Clamp(idx, 0, missionsEnabled.Length-1);
    }
    
    private static void Save()
    {
        if (!loaded)
        {
            // Don't save to file, if wasn't loaded from this file originally,
            // to avoid overriding changes
            return;
        }
        
        MissionsSaveData data = new MissionsSaveData(missionsEnabled);
        SaveLoadUtil.SaveMissionsData(data);
    }

    public static void EnableNextMission()
    {
        if (currentMissionIdx < missionsEnabled.Length - 1)
        {
            SetMissionEnabled(currentMissionIdx + 1, true);
        }
    }
}
