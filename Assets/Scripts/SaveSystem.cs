using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(1)]
public class SaveSystem : MonoBehaviour
{
    public static UnityEvent SaveEvent = new UnityEvent();
    public static UnityEvent LoadEvent = new UnityEvent();
    public static SaveFile _SaveFile ;
    public static bool haveSave = false;
    private string path;
    
    private void Awake()
    {
        path = Application.persistentDataPath + "\\save.json";

        if (File.Exists(path))
        {
            haveSave = true;
            // JSON'u dosyadan oku
            string readJson = File.ReadAllText(path);
            // Okunan JSON'u objeye çevir
            _SaveFile = JsonUtility.FromJson<SaveFile>(readJson);
            
            LoadEvent.Invoke();
        }
        else
        {
            haveSave = false;
            _SaveFile = new SaveFile();
        }
    }

    private void OnApplicationPause(bool pauseStatus) //OYUNU KAYDEDIYOR
    {
        if (pauseStatus)
        {
            Debug.Log("OYUN KAYDEDILDI" + path);
            SaveEvent.Invoke();
            string json = JsonUtility.ToJson(_SaveFile, true);
            File.WriteAllText(path, json);
        }
    }

    // private void OnApplicationQuit()
    // {
    //     SaveEvent.Invoke();
    //     if (GameBase.Dilaver.level != 1)
    //     {
    //         string json = JsonUtility.ToJson(_saveFile, true);
    //         File.WriteAllText(path, json);
    //     }
    // }
}

public abstract class CanSave : MonoBehaviour, ISave
{
    public virtual void OnEnable()
    {
        SaveSystem.LoadEvent.AddListener(SaveLoad);
        SaveSystem.SaveEvent.AddListener(FileSave);
    }

    public virtual void OnDisable()
    {
        SaveSystem.LoadEvent.RemoveListener(SaveLoad);
        SaveSystem.SaveEvent.RemoveListener(FileSave);
    }

    public abstract void FileSave();
    public abstract void SaveLoad();
}

[Serializable]
public partial class SaveFile
{
    public int level = 1;
    public int score;
}