using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoSingleton<UIManager>
{
    public GameObject LoadingPanel;
    public Text rayText;

    public void LoadingFade(float r=0, float g=0, float b=0, float a=0,float t=1.3f ,bool active=false)
    {
        LoadingPanel.GetComponent<Image>().DOColor(new Color(r, g, b, a), t);
        StartCoroutine(ActiveCo(LoadingPanel, active, t+0.1f));
    }

    public IEnumerator ActiveCo(GameObject o,bool active, float time)
    {
        yield return new WaitForSeconds(time);
        o.SetActive(active);
    }
}
