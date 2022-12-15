using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Application = UnityEngine.Device.Application;

public static class SaveLoadUtil
{
    private const string SAVE_FILE_NAME = "missions.bin";

    public static void SaveMissionsData(MissionsSaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = GetFullFilePath();

        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static MissionsSaveData TryLoadMissionData()
    {
        string path = GetFullFilePath();

        Debug.Log($"Loading data from: {path}");
        
        if (!File.Exists(path))
        {
            // No save data
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        MissionsSaveData data = formatter.Deserialize(stream) as MissionsSaveData;
        stream.Close();
        return data;
    }
    
    private static string GetFullFilePath()
    {
        return Application.persistentDataPath + "/" + SAVE_FILE_NAME;
    }
}
