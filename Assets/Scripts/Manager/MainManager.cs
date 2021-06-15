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

    [SerializeField] private GameObject saveObj;
    [SerializeField] private float playTime=0.0f;
    private int deathCount = 0;
    private int h, m, s;
    private GameObject loadingPanel;
    private bool bCursor = false;
    private Color noColor;

    public short saveCnt = 0;
    public short maxSaveCnt = 3;
    public bool isSave = false;

    Sequence seq1;

    private void Awake()
    {
        noColor = new Color(0, 0, 0, 0);
        loadingPanel = UIManager.Instance.LoadingPanel;
        Cursor.lockState = CursorLockMode.Locked;
        deathTxt.text = string.Format("사망: <color=#962323>{0}</color>회", deathCount);
        CreatePool();
    }

    private void CreatePool()
    {
        PoolManager.CreatePool<SoundPrefab>(soundPrefab, GameManager.Instance.soundPoolParent, 10);
        PoolManager.CreatePool<Enemy1>(enemyPrefab, GameManager.Instance.enemyPoolParent, 15);
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