using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class LocalSaveSystem
{
    public static void SaveLocaldata()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(DatabaseManager._instance);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadLocalData()
    {
        string path = Application.persistentDataPath + "/player.sav";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = (GameData)formatter.Deserialize(stream);
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("No Savefile found");
            return null;
        }
    }
}
