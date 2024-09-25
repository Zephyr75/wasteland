using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public static class SaveSystem
{
    public static string directory = "/SaveData/";
    public static string fileName = "MyData.txt";

    public static void SaveData(SaveData data)
    {
        string path = Application.persistentDataPath + directory;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings(){ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
        File.WriteAllText(path + fileName, json);
    }

    public static SaveData LoadData()
    {
        string path = Application.persistentDataPath + directory + fileName;
        SaveData data = new SaveData();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonConvert.DeserializeObject<SaveData>(json);
        }
        else
        {
            Debug.Log("null");
            return null;
        }
        return data;
    }
}
