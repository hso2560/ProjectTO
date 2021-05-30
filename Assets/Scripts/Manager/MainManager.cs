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

    [SerializeField] private float playTime=0.0f;
    private int h, m, s;
    private GameObject loadingPanel;
    private bool bCursor = false;

    private void Awake()
    {
        loadingPanel = UIManager.Instance.LoadingPanel;
    }

    private void Update()
    {
        FlowTime();
        _Input();
    }

    private void FlowTime()
    {
        if (loadingPanel.activeSelf) return;

        playTime += Time.deltaTime;
        h = (int)playTime / 3600;
        m = ((int)playTime / 60)%60;
        s = (int)playTime % 60;
        timeTxt.text = string.Format("{0}:{1}:{2}", h, m, s);
    }

    void _Input()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            Cursor.lockState = !bCursor ? CursorLockMode.Confined : CursorLockMode.Locked;
            bCursor = !bCursor;
        }
    }

    public void TxtDOTw(string s)
    {
        rayText.text = s;
        rayText.DOColor(rayTxtColor, 0.3f);
    }
    public void TxtOff()
    {
        rayText.DOColor(new Color(0, 0, 0, 0), 0.3f);
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