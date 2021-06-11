using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public TextMeshProUGUI titleTxt;

    public GameObject soundPref;
    public Transform soundTfmPar;

    private void Awake()
    {
        PoolManager.CreatePool<SoundPrefab>(soundPref, soundTfmPar, 6);
    }
    private void Start()
    {
        titleTxt.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 2).SetLoops(-1, LoopType.Yoyo);
    }

    public void BtnMouseUp(int index)
    {

    }
}
