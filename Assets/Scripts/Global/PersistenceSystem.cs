using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PersistenceSystem : MonoBehaviour
{
    string fileLocation;
    Dictionary<string, object> persistentData = new Dictionary<string, object>();

    public void AddPersistentObject(string key, object value)
    {
        persistentData.Add(key, value);
    }

    void Start() {
        Debug.Log("Starting PersistenceSystem!!!");
        string filename;
        if (Application.isEditor)
        {
            filename = "GameDataEditor.";
        } else
        {
            filename = "GameData.";
        }
        fileLocation = Application.persistentDataPath + Path.DirectorySeparatorChar + filename;
        Refresh();
    }

    public void Refresh()
    {
        ReadGameData();
    }

    private void ReadGameData()
    {
        StreamReader reader = null;
        string json;
        try
        {
            foreach(string key in persistentData.Keys)
            {
                Debug.Log("Reading data for key " + key);
                object data = persistentData[key];
                string targetFile = fileLocation + key + ".json";
                if (!File.Exists(targetFile))
                {
                    Debug.Log("No data file found");
                    return;
                }
                Debug.Log("Reading data file " + targetFile);
                reader = new StreamReader(targetFile);
                json = reader.ReadToEnd();
                JsonUtility.FromJsonOverwrite(json, data);
                Debug.Log("Assigned data");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed loading game data: " + e.Message);
        }
        finally
        {
            if (reader != null) reader.Close();
        }
    }

    public void Save()
    {
        StreamWriter writer = null;
        try
        {
            foreach (string key in persistentData.Keys)
            {
                Debug.Log("Writing data for key " + key);
                string targetFile = fileLocation + key + ".json";

                string json = JsonUtility.ToJson(persistentData[key]);
                writer = new StreamWriter(targetFile);
                writer.Write(json);
                Debug.Log("Wrote json: " + json);
                Debug.Log("Wrote data to: " + targetFile);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed writing data: " + e.Message);
        }
        finally
        {
            if (writer != null) writer.Close();
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
