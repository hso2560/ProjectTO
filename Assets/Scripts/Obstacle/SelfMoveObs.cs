using DG.Tweening;
using UnityEngine;


public class SelfMoveObs : MonoBehaviour //CollisionEventSc 상속받아서 해도 되겠지만 이미 해버렸으니 귀찮으니 걍 이대로
{
    public bool isObstacle=true;
    public string deathCause="칼빵";

    #region 조심히 건들
    [SerializeField] int id;
    [SerializeField] float rotSpeed, t;
    [SerializeField] Vector3 vec;
    [SerializeField] Ease[] _ease;
    [SerializeField] int index = 0;
    #endregion 
    private Sequence seq;

    private void Start()
    {
        seq = DOTween.Sequence();
        if (id == 20)
        {
            transform.DOMove(vec, t).SetEase(_ease[index]).SetLoops(-1, LoopType.Yoyo);
        }
        else if(id==25)
        {
            transform.DOMove(vec, t).SetLoops(-1, LoopType.Yoyo);
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
