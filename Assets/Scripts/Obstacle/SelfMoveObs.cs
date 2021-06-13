using DG.Tweening;
using UnityEngine;


public class SelfMoveObs : MonoBehaviour //CollisionEventSc ��ӹ޾Ƽ� �ص� �ǰ����� �̹� �ع������� �������� �� �̴��
{
    public bool isObstacle=true;
    public string deathCause="Į��";

    #region ������ �ǵ�
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
