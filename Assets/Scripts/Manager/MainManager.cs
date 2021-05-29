using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Camera mainCam;
    public CamMove cam;
    public Vector3 startPos;
    public PlayerScript player;
    public Text timeTxt;

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
}
