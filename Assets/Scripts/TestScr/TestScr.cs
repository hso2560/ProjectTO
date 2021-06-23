using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestScr : MonoBehaviour  //테스트용 코드
{
     void Update()
     {
        if(id==4)
         transform.Rotate(new Vector3(0, 90, 0)*Time.deltaTime);
        else if(id==5)
        {
            transform.Translate(Vector3.right * Mathf.Sin(Time.time) * 5 * Time.deltaTime);
        }
        else if(id==100)
        {
            if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                Enemy1 e = PoolManager.GetItem<Enemy1>();
                e.InitData(GameManager.Instance.player.transform.position + Vector3.forward * 3, false);
            }
        }
        /*else if(id==1000)
        {
            Debug.Log("u1000");
        }*/
     }
    public int id;

    private void Start()
    {
        
        if(id==0)
          transform.DOMove(Vector3.up * 5, 3f).SetLoops(-1,LoopType.Yoyo);
        else if (id == 1)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOScaleY(3,3f));
            seq.Append(transform.DOShakePosition(3f, 2f));
            seq.Play();
        }
        else if(id==2)
        {
            transform.DOScale(new Vector3(1, 1, 1), 3f);
            transform.DOMove(Vector3.forward * 2f, 2f);
        }

        //StartCoroutine("TestCo");
        /*else if(id==1000)
        {
            Debug.Log("1000");
        }*/
    }
    /*private void Awake()
    {
        if (id == 1000)
        {
            Debug.Log("a1000");
        }
    }

    private void OnEnable()
    {
        Debug.Log("oe1000");
    }

    private void OnDestroy()
    {
        Debug.Log("odst1000");
    }

    private void OnDisable()
    {
        Debug.Log("od1000");
    }*/
    
    /*IEnumerator TestCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Debug.Log(Time.time);
        }
    }*/
}
