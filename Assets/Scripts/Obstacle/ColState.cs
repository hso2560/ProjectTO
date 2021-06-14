using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColState : CollisionEventSc
{
    public bool bResetActive;
    private string firTag;

    private void Start()
    {
        firTag = o.tag;
        firPos = o.transform.position;
        firScl = o.transform.localScale;
    }

    public override void CollisionFunc()
    {
        if (id == 10)
        {
            o.SetActive(true);
        }
        else if(id==20)
        {
            Invoke("InvokeFunc", posT);
        }
        else if (id == 30)
        {
            o.transform.position = pos;
        }
    }

    public override void ObsReset()
    {
        if(id!=30)
           o.SetActive(bResetActive);

        o.transform.position = firPos;
        o.transform.localScale = firScl;
        o.tag = firTag;

        if (id == 20)
        {
            CancelInvoke("InvokeFunc");
            o.GetComponent<Rigidbody>().isKinematic = true;
            o.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    private void InvokeFunc()
    {
        o.GetComponent<Rigidbody>().isKinematic = false;
        o.GetComponent<Rigidbody>().useGravity = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameManager.Instance.player.gameObject)
        {
            CollisionFunc();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player.gameObject)
        {
            CollisionFunc();
            gameObject.SetActive(false);
        }
    }
}
