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
        if (GameManager.Instance.savedData.option.language == Language.Korean)
        {
            if (isClear)
                bestTxt.text = string.Format("�ְ���: {0}�ð� {1}�� {2}��", (int)bestTime / 3600, ((int)bestTime / 60) % 60, (int)bestTime % 60);
            else
                bestTxt.text = "�ְ���: Ŭ���� ��� ����";
        }
        else if(GameManager.Instance.savedData.option.language == Language.English)
        {
            if (isClear)
                bestTxt.text = string.Format("Highest Record: {0}h {1}m {2}s", (int)bestTime / 3600, ((int)bestTime / 60) % 60, (int)bestTime % 60);
            else
                bestTxt.text = "Highest Record: There is no clear record.";
        }
    }
}
