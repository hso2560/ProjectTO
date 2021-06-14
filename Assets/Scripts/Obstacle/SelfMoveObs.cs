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
    [SerializeField] Collider col;
    #endregion 
    
    private Color noColor;
    private Material mat;

    private void Start()
    {
        noColor = new Color(0, 0, 0, 0);
        if (id == 20)
        {
            transform.DOMove(vec, t).SetEase(_ease[index]).SetLoops(-1, LoopType.Yoyo);
        }
        else if(id==25)
        {
            transform.DOMove(vec, t).SetLoops(-1, LoopType.Yoyo);
        }

        else if (id == 50)
        {
            Sequence seq = DOTween.Sequence();
            mat = GetComponent<MeshRenderer>().material;

            seq.Append(mat.DOColor(new Color(0, 0, 0, 0), t));
            seq.AppendInterval(2.5f);
        
            seq.Append(mat.DOColor(new Color(1, 1, 1, 1), t));
            seq.AppendInterval(2.5f);
            
            seq.Play().SetLoops(-1, LoopType.Yoyo);
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
        else if (id == 50)
        {
            col.enabled = mat.color.a > 0.1f;
        }
    }

    
}
