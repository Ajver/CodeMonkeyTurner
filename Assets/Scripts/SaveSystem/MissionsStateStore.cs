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

    public static void Load()
    {
        MissionsSaveData data = SaveLoadUtil.TryLoadMissionData();

        if (data != null)
        {
            missionsEnabled = data.missionEnabled;
        }
        
        Debug.Log("Loaded!");

        foreach (bool enabled in missionsEnabled)
        {
            Debug.Log($"> {enabled}");
        }
            
        Debug.Log("------------");
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
