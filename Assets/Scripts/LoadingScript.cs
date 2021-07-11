using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScript : MonoBehaviour
{
    public Image fillBack, fillImg, fillBlack;
    public Color fillBackColor, fillImgColor;
    private Color tc, noColor;

    private void Awake()
    {
        noColor = new Color(0, 0, 0, 0);
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
        fillBlack.DOColor(noColor, 0.5f);
        fillBack.DOColor(noColor, 0.5f);
        fillImg.DOColor(noColor, 0.5f);
    }

    //팁 텍스트 넣으면 좋음, 몇 초 동안 로딩 시 인터넷 연결 확인해달라는 메시지 띄우면 좋음
}
