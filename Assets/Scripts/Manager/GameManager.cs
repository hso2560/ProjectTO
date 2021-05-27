using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private SaveData saveData;
    public SaveData savedData { get { return saveData; } }
    private readonly string SaveFileName = "SaveFile";
    private string filePath;
    private string savedJson;

    public MainManager mainManager;

    private void Awake()
    {
        saveData = new SaveData();
        filePath = string.Concat(Application.persistentDataPath, "/", SaveFileName);

        Load();
        Init();
    }

    void Save()
    {
        savedJson = JsonUtility.ToJson(saveData);
        byte[] bytes = Encoding.UTF8.GetBytes(savedJson);
        string code = Convert.ToBase64String(bytes);
        File.WriteAllText(filePath, code);
    }

    private void Load()
    {
        if(File.Exists(filePath))
        {
            string code = File.ReadAllText(filePath);
            byte[] bytes = Convert.FromBase64String(code);
            savedJson = Encoding.UTF8.GetString(bytes);
            saveData = JsonUtility.FromJson<SaveData>(savedJson);
            SetData();
        }
        else
        {
            Screen.SetResolution(1280, 720, true);
        }
    }

    public void SetData()
    {
        Screen.SetResolution(1280, 720, saveData.option.isFullScr);
    }

    public void Init()
    {

    }

    private void Update()
    {
        _Input();
    }

    private void _Input()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }

    public void SceneChange(string scName)
    {
        Save();
        SceneManager.LoadScene(scName);
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            Save();
        }
    }
}
