using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColSpawn : CollisionEventSc
{
    public Transform[] spawnTr;
    public bool[] bs;

    public override void CollisionFunc()  
    {
        if(id==0)
        {
            for(int i=0; i<spawnTr.Length; i++)
            {
                Enemy1 e1 = PoolManager.GetItem<Enemy1>();  
                e1.InitData(spawnTr[i].position,bs[0]);  
            }
        }
        else if(id==10)
        {
            Invoke("InvokeFunc", posT);
        }
    }

    public override void ObsReset()
    {
        if(id==10)
        {
            CancelInvoke("InvokeFunc");
        }

        gameObject.SetActive(firstActive);
    }

    private void InvokeFunc()
    {
        switch(id)
        {
            case 10:
                for (int i = 0; i < spawnTr.Length; i++)
                {
                    Enemy1 e1 = PoolManager.GetItem<Enemy1>();
                    e1.InitData(spawnTr[i].position, bs[0]);
                }
                break;

        }
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
