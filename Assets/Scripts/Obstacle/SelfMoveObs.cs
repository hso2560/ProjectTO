using DG.Tweening;
using UnityEngine;
using System.Collections;


public class SelfMoveObs : MonoBehaviour 
{
    public bool isObstacle=true;
    //배열로 만들면 편하게 언어에 따라서 글자 나오게 할 수 있겠지만 이미 전에 이걸로 해버려서 이렇게 함..
    public string deathCause="칼빵";
    public string deathCause_en= "Knife";

    #region 조심히 건들
    [SerializeField] int id;
    [SerializeField] float rotSpeed, t;
    [SerializeField] Vector3 vec;
    [SerializeField] Ease[] _ease;
    [SerializeField] int index = 0;
    [SerializeField] Collider col;

    [Header("ID: 300")]
    [SerializeField] ParticleSystem fire;
    [Header("ID: 400")]
    [SerializeField] LineRenderer laser;
    [Header("Fire:300 Laser:400")]
    [SerializeField] bool isFire;

    [SerializeField] Color startColor, endColor;
    #endregion 
    
    
    private Material mat;
    private WaitForSeconds ws1;
    private WaitForSeconds ws2;

    private void Awake()
    {
        if (id == 300 || id == 400)
        {
            ws1 = new WaitForSeconds(t);  // 파이어(레이저) 대기 시간
            ws2 = new WaitForSeconds(rotSpeed); //파이어(레이저) 발사 시간

            if (id == 400)
            {
                laser.startColor = startColor;
                laser.endColor = endColor;
            }
        }
    }

    private void Start()
    {
        
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

        else if(id==200)
        {
            transform.DOScale(vec, t).SetLoops(-1, LoopType.Yoyo);
        }
        else if(id==500)
        {
            transform.DORotateQuaternion(Quaternion.Euler(vec), t).SetLoops(-1, LoopType.Yoyo).SetEase(_ease[index]);
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
        else if(id==70)
        {
            transform.RotateAround(vec, Vector3.up, rotSpeed);
        }

        //Sin, Cos 이용해서 움직이니까 에디터에서 실행했을 때와 빌드 후 실행했을 때의 움직이는 게 다름.
        /*else if(id==100)
        {
            transform.Translate(vec * Mathf.Sin(Time.time) * t * Time.deltaTime);
        }
        else if(id==120)
        {
            transform.Translate(vec * Mathf.Cos(Time.time) * t * Time.deltaTime);
        }*/
        else if(id==150)
        {
            float x = Mathf.Cos(Time.time) * t;
            float z = Mathf.Sin(Time.time) * t;
            transform.localPosition = (new Vector3(x, 0, z)+vec)*rotSpeed*Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        if (id == 300 || id==400)
        {
            StartCoroutine(Fire(isFire));
        }
    }

    private IEnumerator Fire(bool bFire)
    {
        if (bFire)
        {
            while (true)
            {
                yield return ws1;
                fire.gameObject.SetActive(true);
                yield return ws2;
                fire.gameObject.SetActive(false);
                
            }
        }
        else
        {
            while (true)
            {
                yield return ws1;
                laser.gameObject.SetActive(true);
                yield return ws2;
                laser.gameObject.SetActive(false);
            }
        }
    }
}
