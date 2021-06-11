using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public enum ScState 
{
    LOBBY=0,
    MAIN=1
}

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private SaveData saveData;
    public SaveData savedData { get { return saveData; } }
    private readonly string SaveFileName = "SaveFile";
    private string filePath;
    private string savedJson;

    public MainManager mainManager;
    public LobbyManager lobbyManager;
    public ScState scState;

    public GameObject SystemPanel;
    public Text SystemText;
    public Color[] gameColors;
    public Toggle[] gameToggles;  //0: 전체화면

    public GameObject[] mainObjs;
    [SerializeField] List<GameObject> UIObjs;
    public Transform soundPoolParent;

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
        }
        SetData();
    }

    public void SetData()
    {
        if(scState==ScState.LOBBY)
        {
            UIManager.Instance.nameInput.text = saveData.userInfo.nickName;
            if (saveData.userInfo.isFirstStart)
            {
                saveData.userInfo.isFirstStart = false;
                PopupPanel("닉네임을 설정해주세요.");
            }
        }
        else
        {

        }

        gameToggles[0].isOn = saveData.option.isFullScr;
        Screen.SetResolution(1280, 720, saveData.option.isFullScr);
    }

    public void Init()
    {
        if (scState == ScState.LOBBY)
        {
            UIManager.Instance.LoadingFade();
        }
    }

    private void Update()
    {
        _Input();
    }

    private void _Input()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(UIObjs.Count>0)
            {
                UIObjs[UIObjs.Count - 1].SetActive(false);
                UIObjs.Remove(UIObjs[UIObjs.Count - 1]);
            }
        }
    }

    public void SceneChange(string scName)
    {
        if(scState==ScState.LOBBY)
        {
            if (UIManager.Instance.nameInput.text.Trim() == "")
            {
                PopupPanel("닉네임이 공백일 수 없습니다.");
                return;
            }
        }

        Save();
        SceneManager.LoadScene(scName);
    }

    public void BtnPanelOnOff(int idx)
    {
        if (!mainObjs[idx].activeSelf)
        {
            UIObjs.Add(mainObjs[idx]);
            mainObjs[idx].transform.localScale = Vector3.zero;
            mainObjs[idx].SetActive(true);
            mainObjs[idx].transform.DOScale(new Vector3(1, 1, 1), 0.5f);
        }
        else
        {
            UIObjs.Remove(mainObjs[idx]);
            mainObjs[idx].SetActive(false);
        }
    }
    public void PopupPanel(string msg)
    {
        SystemPanel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        UIObjs.Add(SystemPanel);
        SystemPanel.SetActive(true);
        SystemPanel.transform.DOScale(new Vector3(1, 1, 1), 0.3f);

        SystemText.color = new Color(0, 0, 0, 0);
        SystemText.text = msg;
        SystemText.DOColor(gameColors[0], 0.6f);
    }

    public void ToggleClick(int num)
    {
        if(num==0)
        {
            saveData.option.isFullScr = gameToggles[num].isOn;
            Screen.fullScreen = saveData.option.isFullScr;
        }
    }

    public void GameQuit() => Application.Quit();

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
