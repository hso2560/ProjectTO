using DG.Tweening;
using UnityEngine;

public class ColTransform : CollisionEventSc
{
    Sequence seq;

    private void Start()
    {
        firPos = o.transform.position;
        firScl = o.transform.localScale;
    }

    public override void CollisionFunc()
    {
        seq = DOTween.Sequence();
        
        if(id==10)
        {
            seq.Append(o.transform.DOMove(new Vector3(pos.x,pos.y,pos.z),posT));
            seq.Append(o.transform.DOScale(new Vector3(scl.x, scl.y, scl.z), sclT));
            seq.Play();
        }
        else if(id==15)
        {
            o.SetActive(true);
        }
    }

    public override void ObsReset()
    {
        seq.Kill();
        o.SetActive(true);
        
        if (id <=20)
        {
            o.transform.position = firPos;
            o.transform.localScale = firScl;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject==GameManager.Instance.mainManager.player.gameObject)
        {
            CollisionFunc();

            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && collision.gameObject == GameManager.Instance.mainManager.player.gameObject)
        {
            CollisionFunc();
        }
    }
}
