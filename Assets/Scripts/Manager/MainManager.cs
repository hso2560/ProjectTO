using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public Camera mainCam;

    private void Start()
    {
        GameManager.Instance.mainManager = this;
    }
}
