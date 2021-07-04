using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoSingleton<UIManager>
{
    public GameObject LoadingPanel;
    public Image RedPanel;
    public Text[] lobbyTexts;  //중간에 있는 버튼들(메인씬에선 설정에서의 중간버튼들)
    public InputField nameInput;
    public LoadingScript loadingScr;

    public Text systemTxt;
    public Color[] gameColors;
    private Color noColor;
    [SerializeField] private string[] startMsg;


    [SerializeField] private string[] lobbyTxtsStr;  //중간에 있는 버튼들(메인씬에선 설정에서의 중간버튼들)의 텍스트

    public Text[] UITxts;
    [SerializeField] private string[] UITxtsStr;

    public Text[] UIcvsWldSpcTxts;
    [SerializeField] private string[] cvsWStxtStr;

    private void Awake()
    {
        noColor = new Color(0, 0, 0, 0);
    }

    public void LoadingFade(float r=0, float g=0, float b=0, float a=0,float t=1.8f ,bool active=false)  //매개변수로 Color타입으로 받아도 됨
    {
        LoadingPanel.GetComponent<Image>().DOColor(new Color(r, g, b, a), t);
        StartCoroutine(ActiveCo(LoadingPanel, active, t+0.1f));

        if (loadingScr != null) //Main만 있음
        {
            loadingScr.FillImgFade();

            if(startMsg.Length!=0)
               ShowSystemMsg(startMsg[(int)GameManager.Instance.savedData.option.language], 1, 3.5f, 1);
        }
    }

    public IEnumerator ActiveCo(GameObject o,bool active, float time)
    {
        yield return new WaitForSeconds(time);
        o.SetActive(active);
    }

    public void OnPointerEnterText(int index)
    {
        lobbyTexts[index].transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.5f);
    }
    public void OnPointerExitText(int idx)
    {
        lobbyTexts[idx].transform.DOScale(new Vector3(1, 1, 1), 0.5f);
    }

    public void EndEditNameInput()
    {
        GameManager.Instance.savedData.userInfo.nickName = nameInput.text;
    }

    public void ShowSystemMsg(string msg,float fadeInTime=0.7f, float time=2.5f, float fadeOutTime=0.5f ,int index=0, int _fontSize=35)
    {
        systemTxt.color = noColor;
        systemTxt.text = msg;
        systemTxt.fontSize = _fontSize;
        systemTxt.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(systemTxt.DOColor(gameColors[index], fadeInTime));
        seq.AppendInterval(time);
        seq.Append(systemTxt.DOColor(noColor, fadeOutTime));
        seq.AppendCallback(() =>
        {
            systemTxt.gameObject.SetActive(false);
        });
        seq.Play();
    }

    public void DamageEffect()
    {
        RedPanel.color = noColor;
        RedPanel.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(RedPanel.DOColor(gameColors[2], 0.4f));
        seq.Append(RedPanel.DOColor(noColor, 0.3f));
        seq.AppendCallback(() => { RedPanel.gameObject.SetActive(false); });

        seq.Play();
    }

    public void LanguagePatch(Language lang)
    {
        int i;
        for(i=0; i<lobbyTexts.Length; i++)
        {
            lobbyTexts[i].text = lobbyTxtsStr[i + lobbyTexts.Length*(int)lang];
        }

        for(i=0; i<UITxts.Length; i++)
        {
            UITxts[i].text = UITxtsStr[i + UITxts.Length * (int)lang];
        }
        
        if(GameManager.Instance.scState==ScState.MAIN)
        {
            for (i = 0; i < UIcvsWldSpcTxts.Length; i++)
            {
                UIcvsWldSpcTxts[i].text = cvsWStxtStr[i + UIcvsWldSpcTxts.Length * (int)lang];
            }
        }

       /* if(lang==Language.English)
        {

        }
        else if(lang==Language.Korean)
        {
            
        }*/
    }
}
