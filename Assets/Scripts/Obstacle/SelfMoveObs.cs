using DG.Tweening;
using UnityEngine;


public class SelfMoveObs : MonoBehaviour
{
    public string deathCause="Ä®»§";

    [SerializeField] int id;
    [SerializeField] float rotSpeed, t;
    [SerializeField] Vector3 vec;
    [SerializeField] Ease[] _ease;
    private Sequence seq;

    private void Start()
    {
        seq = DOTween.Sequence();
        if (id == 20)
        {
            transform.DOMove(vec, t).SetEase(_ease[0]).SetLoops(-1, LoopType.Yoyo);
        }
        //seq.SetLoops(-1, LoopType.Yoyo);
        //seq.AppendInterval
    }

    private void Update()
    {
        if(id==10)
        {
            transform.Rotate(vec * rotSpeed * Time.deltaTime);
        }
    }
}
