using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

[Serializable]
public static class SaveSystem
{
    public static void SaveGame (GameManager gameManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/peacock.dork";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(gameManager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadData()
    {
        string path = Application.persistentDataPath + "/peacock.dork";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }

    public static bool SaveFileExists()
    {
        if (File.Exists(Application.persistentDataPath + "/peacock.dork"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void DeleteData()
    {
        File.Delete(Application.persistentDataPath + "/peacock.dork");
    }
}
