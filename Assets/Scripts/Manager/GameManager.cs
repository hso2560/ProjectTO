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

public enum ExceptionType
{
    MENU
}

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private SaveData saveData;
    public SaveData savedData { get { return saveData; } }
    private readonly string SaveFileName = "SaveFile";
    private string filePath;
    private string savedJson;

    public PlayerScript player=null;
    public MainManager mainManager;
    public LobbyManager lobbyManager;
    public ScState scState;

    public GameObject SystemPanel;
    public Text SystemText;
    public Color[] gameColors;
    public Toggle[] gameToggles;  //0: 전체화면
    public Slider[] gameSliders;  //0: BGM,  1: 효과음

    public GameObject[] mainObjs;
    [SerializeField] List<GameObject> UIObjs;
    public Transform soundPoolParent;
    public AudioSource bgmAudio;
    //public GameObject settingsPanel;
    [SerializeField] private int uiSoundIdx;

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
            lobbyManager.SetBestScore(saveData.userInfo.isClear, saveData.userInfo.bestTime);
        }

        gameToggles[0].isOn = saveData.option.isFullScr;
        gameSliders[0].value = saveData.option.bgmSize;
        gameSliders[1].value = saveData.option.soundEffect;
        Screen.SetResolution(1280, 720, saveData.option.isFullScr);

        bgmAudio.volume = saveData.option.bgmSize;
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
                if (UIObjs.Count == 0 && mainManager != null)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    player.bCompulsoryIdle = false;
                }
                SoundManager.Instance.PlaySoundEffect(uiSoundIdx);
            }
            else
            {
                if(scState==ScState.MAIN)
                {
                    BtnPanelOnOff(0);
                    Cursor.lockState = CursorLockMode.Confined;
                }
                else if (scState == ScState.LOBBY)
                {
                    BtnPanelOnOff(2);
                }
            }
        }
    }

    /*public void FadePanel()
    {
        Sequence seq = DOTween.Sequence();
        bool isActive = !settingsPanel.activeSelf;
        
        if(isActive)
        {
            settingsPanel.SetActive(isActive);
        }

        seq.Append(settingsPanel.GetComponent<CanvasGroup>().DOFade(isActive?1:0, 0.4f));
        seq.AppendCallback(() =>
        {
            settingsPanel.GetComponent<CanvasGroup>().blocksRaycasts = isActive;
            settingsPanel.GetComponent<CanvasGroup>().interactable = isActive;
            if(!isActive)
            {
                settingsPanel.SetActive(isActive);
            }
        });
        seq.Play();
    }*/

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

        PoolManager.ClearItem<SoundPrefab>();
        Save();
        SceneManager.LoadScene(scName);
    }

    public void BtnPanelOnOff(int idx)
    {
        if(ExceptionHandling(ExceptionType.MENU,idx))
        {
            return;
        }

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
        SoundManager.Instance.PlaySoundEffect(uiSoundIdx);
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

    private bool ExceptionHandling(ExceptionType et, int intValue=-1001)
    {
        if (et == ExceptionType.MENU)
        {
            if(scState==ScState.MAIN)
            {
                if(intValue==0)
                {
                    if (!mainObjs[0].activeSelf)
                    {
                        bool b = player.IsJumping || player.MoveVec != Vector3.zero;
                        player.bCompulsoryIdle = !b;
                        return b;
                    }
                    else
                    {
                        player.bCompulsoryIdle = false;
                        return false;
                    }
                }
                return false;
            }
            return false;
        }
        return false;
    }

    public void ToggleClick(int num)
    {
        if(num==0)
        {
            saveData.option.isFullScr = gameToggles[num].isOn;
            Screen.fullScreen = saveData.option.isFullScr;
        }
    }
    public void ChangeSliderValue(int num)
    {
        if (num == 0)
        {
            saveData.option.bgmSize = gameSliders[num].value;
            bgmAudio.volume = saveData.option.bgmSize;

            //if (bgmAudio.volume == 0) bgmAudio.Stop();
            //else if (bgmAudio.volume > 0 && !bgmAudio.isPlaying) bgmAudio.Play();
        }
        else if(num==1)
        {
            saveData.option.soundEffect = gameSliders[num].value;
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
