
using UnityEngine;

public class ColState : CollisionEventSc
{
    public bool bResetActive;
    private string firTag;

    [SerializeField] bool temporaryExcHdl;

    private void Start()
    {
        if (o != null)
        {
            firTag = o.tag;
            firPos = o.transform.position;
            firScl = o.transform.localScale;
        }

        bInitSet = true;
        gameObject.SetActive(firstActive);
    }

    public override void CollisionFunc()
    {
        isWork = true;

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
        else if(id==200)
        {
            GameManager.Instance.mainManager.LastStage(false);
            o.SetActive(true);
        }
    }

    public override void ObsReset()
    {
        isWork = false;

        if(id!=30)
           o.SetActive(bResetActive);

        if (!temporaryExcHdl)
        {
            o.transform.position = firPos;
            o.transform.localScale = firScl;

            o.tag = firTag;
        }
        
        if (id == 20)
        {
            CancelInvoke("InvokeFunc");
            o.GetComponent<Rigidbody>().isKinematic = true;
            o.GetComponent<Rigidbody>().useGravity = false;
        }
        else if (id == 200)
        {
            GameManager.Instance.mainManager.LastStage(true);
        }

        gameObject.SetActive(firstActive);
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
        if (GameManager.Instance.player == null) return;

        if (other.gameObject == GameManager.Instance.player.gameObject)
        {
            CollisionFunc();
            gameObject.SetActive(false);
        }
    }
}
