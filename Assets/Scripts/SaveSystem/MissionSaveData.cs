using System;

[Serializable]
public class MissionsSaveData
{

    public bool[] missionEnabled;

    public MissionsSaveData(bool[] missionEnabled)
    {
        this.missionEnabled = missionEnabled;
    }
    
}