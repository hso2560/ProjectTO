using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScript : MonoBehaviour
{
    public Image fillBack, fillImg, fillBlack;
    public Color fillBackColor, fillImgColor;
    private Color tc;

    private void Awake()
    {
        StartCoroutine(LoadingFill());
    }

    private IEnumerator LoadingFill()
    {
        while(gameObject.activeSelf)
        {
            fillImg.fillAmount += Time.deltaTime;
            yield return null;
            if(fillImg.fillAmount==1)
            {
                tc = fillBack.color;
                fillBack.color = fillImg.color;
                fillImg.fillAmount = 0;
                fillImg.color = tc;
            }
        }
    }

    public void FillImgFade()
    {
        fillBlack.DOColor(new Color(0, 0, 0, 0), 0.5f);
        fillBack.DOColor(new Color(0, 0, 0, 0), 0.5f);
        fillImg.DOColor(new Color(0, 0, 0, 0), 0.5f);
    }
}
