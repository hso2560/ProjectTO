using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public TextMeshProUGUI titleTxt;
    public Text bestTxt;
    [SerializeField] Text testText;
    public GameObject soundPref;

    [SerializeField] private bool isTestMode;

    private void Awake()
    {
        testText.gameObject.SetActive(isTestMode);
        Cursor.lockState = CursorLockMode.None;
        PoolManager.CreatePool<SoundPrefab>(soundPref, GameManager.Instance.soundPoolParent, 6);
    }
    private void Start()
    {
        titleTxt.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 2).SetLoops(-1, LoopType.Yoyo);
    }

    public void SetBestScore(bool isClear, float bestTime)
    {
        if (isClear)
            bestTxt.text = string.Format("최고기록: {0}시간 {1}분 {2}초", (int)bestTime / 3600, ((int)bestTime / 60) % 60, (int)bestTime % 60);
        else
            bestTxt.text = "최고기록: 클리어 기록 없음";
    }
}
