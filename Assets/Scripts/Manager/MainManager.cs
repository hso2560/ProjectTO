using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainManager : MonoBehaviour
{
    public Camera mainCam;
    public CamMove cam;
    public PlayerScript player;
    public Text timeTxt, deathTxt;
    public Color rayTxtColor;
    public Text rayText;
    public ObjectManager objManager;
    public GameObject diePanel;
    public GameObject userListPanel;
    public Color[] gameColors;
    private bool isGoal = false;
    public GameObject soundPrefab, enemyPrefab;
    public Light mainLight;

    [SerializeField] private GameObject lastSaveObj;
    [SerializeField] private GameObject saveObj;
    [SerializeField] private float playTime=0.0f;
    [SerializeField] private int enemySpawnCount = 30;
    private int deathCount = 0;
    private int h, m, s;
    private GameObject loadingPanel;
    private bool bCursor = false;
    private Color noColor;

    public short saveCnt = 0;
    public short maxSaveCnt = 3;
    public bool isSave = false;

    public GameObject ExhaustPile;
    public bool isLast = false;
    [SerializeField] private Text lastTxt;
    [SerializeField] private GameObject lastStageMap;

    public Vector3 devVec;

    Sequence seq1;

    private void Awake()
    {
        noColor = new Color(0, 0, 0, 0);
        loadingPanel = UIManager.Instance.LoadingPanel;
        Cursor.lockState = CursorLockMode.Locked;
        deathTxt.text = string.Format("사망: <color=#962323>{0}</color>회", deathCount);
        CreatePool();
        InitData();
    }

    private void CreatePool()
    {
        PoolManager.CreatePool<SoundPrefab>(soundPrefab, GameManager.Instance.soundPoolParent, 10);
        PoolManager.CreatePool<Enemy1>(enemyPrefab, GameManager.Instance.enemyPoolParent, enemySpawnCount);
    }

    private void InitData()
    {
        lastStageMap.SetActive(false);
        Transform tr = GameManager.Instance.enemyPoolParent;
        for(int i=0; i<enemySpawnCount; i++)
        {
            objManager.enemys.Add(tr.GetChild(i).GetComponent<Enemy1>());
        }
    }

    private void Update()
    {
        FlowTime();
        _Input();
    }

    private void FlowTime()
    {
        if (loadingPanel.activeSelf) return;

        if (!isGoal)
        {
            playTime += Time.deltaTime;
            h = (int)playTime / 3600;
            m = ((int)playTime / 60) % 60;
            s = (int)playTime % 60;
            timeTxt.text = string.Format("{0}:{1}:{2}", h, m, s);
        }
    }

    void _Input()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            Cursor.lockState = !bCursor ? CursorLockMode.Confined : CursorLockMode.Locked;
            bCursor = !bCursor;
        }

        userListPanel.SetActive(Input.GetKey(KeyCode.Tab));
    }

    public void TxtDOTw(string s)
    {
        if (rayText.text == s) return;

        rayText.text = s;
        rayText.gameObject.SetActive(true);
        rayText.DOColor(rayTxtColor, 0.3f);
    }
    public void TxtOff()
    {
        //rayText.DOColor(new Color(0, 0, 0, 0), 0.3f);
        if (player.scanObj != null)
        {
            seq1 = DOTween.Sequence()
                .Append(rayText.DOColor(new Color(0, 0, 0, 0), 0.3f))
                .AppendCallback(() =>
                {
                    rayText.text = "";
                    rayText.gameObject.SetActive(false);
                });
        }
        seq1.Play();
    }

    public void Die(string cause)
    {
        diePanel.SetActive(true);
        diePanel.transform.GetChild(0).GetComponent<Text>().text = "사인: " + cause;
        diePanel.GetComponent<Image>().DOColor(gameColors[0], 0.8f);
        diePanel.transform.GetChild(0).GetComponent<Text>().DOColor(gameColors[1], 0.7f);
        diePanel.transform.GetChild(1).DOScale(new Vector3(1, 1, 1), 1);
        diePanel.transform.GetChild(2).GetComponent<Text>().DOColor(gameColors[1], 0.6f);

        Cursor.lockState = CursorLockMode.Confined;

        deathCount++;
        deathTxt.text = string.Format("사망: <color=#962323>{0}</color>회", deathCount);

        if (isSave && saveCnt == 0)
        {
            isSave = false;
            NetManager.instance.firstPos = NetManager.instance.v;
            saveObj.SetActive(true);
            if(isLast)
            {
                isLast = false;
                LastEffect(false);
                lastStageMap.SetActive(false);
            }
        }
    }

    public void Respawn()
    {
        diePanel.GetComponent<Image>().color = noColor;
        diePanel.transform.GetChild(0).GetComponent<Text>().color = noColor;
        diePanel.transform.GetChild(1).localScale = new Vector3(0.1f, 0.1f, 0.1f);
        diePanel.transform.GetChild(2).GetComponent<Text>().color = noColor;

        //DOTween.KillAll();  모든 다트윈 실행을 끔.
        Cursor.lockState = CursorLockMode.Locked;

        diePanel.SetActive(false);
        objManager.ObsReset();
        player.Respawn();

        if(isSave)
        {
            saveCnt--;
        }
    }

    public void PlayerTfSave(GameObject so)
    {
        if (saveObj != null) saveObj.SetActive(true);

        saveObj = so;
        so.gameObject.SetActive(false);
        isSave = true;
        saveCnt = maxSaveCnt;

        if(saveObj==lastSaveObj)
        {
            isLast = true;
            LastEffect(true);
            lastStageMap.SetActive(true);
        }    
    }

    public void LastStage(bool isReset)
    {
        if (isLast) return;
        lastTxt.text = !isReset ? "?????" : "길 없음";
    }

    public void LastEffect(bool b)
    {
        if (b)
        {
            GameManager.Instance.bgmAudio.DOPitch(-1.3f, 3); 
            mainCam.DOShakePosition(0.5f, 2);
            mainLight.DOIntensity(0.2f, 3);  
            mainLight.DOColor(gameColors[2], 0.5f);
            SoundManager.Instance.PlaySoundEffect(0);
            cam.camGlich.enabled = true;
            Invoke("OffGlich", 0.4f);
        }
        else
        {
            GameManager.Instance.bgmAudio.pitch=1;
            mainLight.intensity = 0.8f;
            mainLight.color = gameColors[3];
        }
    }

    void OffGlich()
    {
        cam.camGlich.enabled = false;
    }
}

[System.Serializable]
public class Message
{
    public GameObject o;

    public int myAct;
    public int otherAct;

    public int iValue;
    public float fValue;
    public bool bValue;
    public string sValue;

    public Vector3 v;
}