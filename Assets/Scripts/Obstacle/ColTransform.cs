using DG.Tweening;
using UnityEngine;

public class ColTransform : CollisionEventSc
{
    Sequence seq;
    #region Á¶½É
    [SerializeField] Rigidbody rigid;

    [SerializeField] bool isParentValue;
    [SerializeField] Vector3[] aVec;
    [SerializeField] float[] aPosTime;
    [SerializeField] GameObject[] objs;
    private Vector3[] firVecs;
    #endregion

    private void Start()
    {
        if (!isParentValue)
        {
            firPos = o.transform.position;
            firScl = o.transform.localScale;
        }
        else
        {
            firVecs = new Vector3[aVec.Length];

            for(int i=0; i<firVecs.Length; i++)
            {
                firVecs[i] = objs[i].transform.position;
            }
        }
    }

    public override void CollisionFunc()
    {
        seq = DOTween.Sequence().SetId(DOTIdStr);
        
        if(id==10)
        {
            seq.Append(o.transform.DOMove(new Vector3(pos.x,pos.y,pos.z),posT));
            seq.Append(o.transform.DOScale(new Vector3(scl.x, scl.y, scl.z), sclT));
            seq.Play();
        }
        else if(id==30)
        {
            seq.Append(o.transform.DOScale(new Vector3(scl.x, scl.y, scl.z), sclT));
            seq.Append(o.transform.DOMove(new Vector3(pos.x, pos.y, pos.z), posT));
            
            seq.Play();
        }
        else if(id==50)
        {
            for(int i=0; i<aVec.Length; i++)
            {
                seq.AppendInterval(aPosTime[i]);
                //seq.AppendCallback(() => { objs[i].SetActive(true); });
                objs[i].SetActive(true);
                seq.Append(objs[i].transform.DOMove(new Vector3(aVec[i].x, aVec[i].y, aVec[i].z), posT));
            }

            seq.Play();
        }
        else if(id==100)
        {
            for(int i=0; i<aVec.Length; i++)
            {
                objs[i].transform.DOMove(new Vector3(aVec[i].x, aVec[i].y, aVec[i].z), aPosTime[i]).SetId("NoSeqDOT");
            }
        }    
    }

    public override void ObsReset()
    {
        //seq.Kill();
        //DOTween.KillAll();
        DOTween.Kill(DOTIdStr);
        
        if(isParentValue)
        {
            for(int i=0; i<objs.Length; i++)
            {
                objs[i].transform.position = firVecs[i];
                objs[i].SetActive(id!=50);
            }

            if(id==100)
            {
                DOTween.Kill("NoSeqDOT");
            }

            return;
        }

        o.SetActive(true);
        
        if (id <=30)
        {
            o.transform.position = firPos;
            o.transform.localScale = firScl;
        }

        gameObject.SetActive(firstActive);

        /*if (rigid != null)
        {
            rigid.drag = 100;
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject==GameManager.Instance.player.gameObject)
        {
            CollisionFunc();

            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameManager.Instance.player.gameObject)
        {
            CollisionFunc();
        }
    }
}
