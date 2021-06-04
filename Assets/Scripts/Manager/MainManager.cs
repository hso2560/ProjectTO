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
    public Text timeTxt;
    public Color rayTxtColor;
    public Text rayText;
    public ObjectManager objManager;
    public GameObject diePanel;
    public GameObject userListPanel;
    public Color[] gameColors;
    private bool isGoal = false;

    [SerializeField] private float playTime=0.0f;
    private int h, m, s;
    private GameObject loadingPanel;
    private bool bCursor = false;

    private void Awake()
    {
        loadingPanel = UIManager.Instance.LoadingPanel;
        Cursor.lockState = CursorLockMode.Locked;
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
        rayText.text = s;
        rayText.DOColor(rayTxtColor, 0.3f);
    }
    public void TxtOff()
    {
        if(rayText.color==rayTxtColor)
           rayText.DOColor(new Color(0, 0, 0, 0), 0.3f);
    }

    public void Die(string cause)
    {
        diePanel.SetActive(true);
        diePanel.transform.GetChild(0).GetComponent<Text>().text = "����: " + cause;
        diePanel.GetComponent<Image>().DOColor(gameColors[0], 0.8f);
        diePanel.transform.GetChild(0).GetComponent<Text>().DOColor(gameColors[1], 0.7f);
        diePanel.transform.GetChild(1).DOScale(new Vector3(1, 1, 1), 1);
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Respawn()
    {
        diePanel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        diePanel.transform.GetChild(0).GetComponent<Text>().color = new Color(0, 0, 0, 0);
        diePanel.transform.GetChild(1).localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Cursor.lockState = CursorLockMode.Locked;
        diePanel.SetActive(false);
        objManager.ObsReset();
        player.Respawn();
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