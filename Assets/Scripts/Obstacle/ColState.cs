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
    }

    public override void ObsReset()
    {
        o.SetActive(bResetActive);

        o.transform.position = firPos;
        o.transform.localScale = firScl;
        o.tag = firTag;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && collision.gameObject == GameManager.Instance.mainManager.player.gameObject)
        {
            CollisionFunc();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && other.gameObject == GameManager.Instance.mainManager.player.gameObject)
        {
            CollisionFunc();
            gameObject.SetActive(false);
        }
    }
}
