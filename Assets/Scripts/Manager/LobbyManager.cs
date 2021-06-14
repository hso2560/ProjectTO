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
    public GameObject soundPref;

    private void Awake()
    {
        PoolManager.CreatePool<SoundPrefab>(soundPref, GameManager.Instance.soundPoolParent, 6);
    }
    private void Start()
    {
        titleTxt.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 2).SetLoops(-1, LoopType.Yoyo);
    }

    public void SetBestScore(bool isClear, float bestTime)
    {
        if (isClear)
            bestTxt.text = string.Format("�ְ���: {0}�ð� {1}�� {2}��", (int)bestTime / 3600, ((int)bestTime / 60) % 60, (int)bestTime % 60);
        else
            bestTxt.text = "�ְ���: Ŭ���� ��� ����";
    }
}
