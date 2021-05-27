using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject LoadingPanel;

    private void Awake()
    {
        instance = this;
    }
}
