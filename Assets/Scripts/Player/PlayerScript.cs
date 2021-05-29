using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Rendering;

public class PlayerScript : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private Animator ani;
    [SerializeField] private Transform centerTr;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private float runSpeed;
    [SerializeField] private float speed;
    [SerializeField] private int maxHp;
    [SerializeField] private float jumpPower;
    [SerializeField] private float groundRayDist, rayDist;  //땅으로의 방향 레이 길이, 정면 방향 레이 길이
    [SerializeField] private float gravity;
    [SerializeField] private GameObject[] bodys;
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private float atkCool=1f, atkColTime=0.5f;  //공격 쿨, 공격 콜라이더 박스가 활성화되어있는 시간
    public int damage=50;
    public int playerId;
    public bool isDie;

    private GameObject playerModel, attackCol;
    [SerializeField] private int hp;
    private bool isJumping, isAtk;
    private bool isInvinci=false; //무적인지?
    private MainManager mainManager;
    private Vector3 moveVec;

    #region UI
    private Text rayText;  //레이에 맞아서 그 정보를 가져올 때의 텍스트
    #endregion

    private void Start()
    {
        if(playerId==PhotonNetwork.LocalPlayer.ActorNumber)
        {
            Init();
            for(int i=0; i<bodys.Length; i++)
            {
                bodys[i].SetActive(false);
            }
            for(int i=0; i<weapons.Length; i++)
            {
                weapons[i].GetComponent<SkinnedMeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
            }
        }

        playerModel = ani.gameObject;
        hp = maxHp;
        attackCol = transform.GetChild(2).gameObject;
    }

    void Init()
    {
        gameObject.AddComponent<AudioListener>();
        mainManager = GameManager.Instance.mainManager;
        mainManager.cam.target = centerTr;
        mainManager.cam.rotTarget = transform;
        rayText = UIManager.Instance.rayText;
    }

    private void Update()
    {
        if (playerId == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            Jump();
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (playerId == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            Move();
            GroundCheck();
            RayHit();
        }
    }

    private void Move()
    {
        if(!isJumping)
           moveVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        Vector3 worldDir = transform.TransformDirection(moveVec);
        Vector3 veloc = worldDir * (Input.GetKey(KeyCode.LeftShift) ? runSpeed : speed);
        Vector3 force = new Vector3(veloc.x - rigid.velocity.x, -gravity, veloc.z - rigid.velocity.z);
        rigid.AddForce(force, ForceMode.VelocityChange);

        playerModel.transform.localRotation = Quaternion.Euler(0, transform.rotation.y, 0);

        ani.SetBool("walk", moveVec != Vector3.zero);
        ani.SetBool("run", moveVec != Vector3.zero && Input.GetKey(KeyCode.LeftShift));
    }

    private void Jump()
    {
        if (!isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            rigid.velocity = Vector3.up * jumpPower;
            ani.SetTrigger("jump");
        }
    }

    private void Attack()
    {
        if(Input.GetMouseButtonDown(0) && !isAtk)
        {
            ani.SetTrigger("attack");
            isAtk = true;
            attackCol.SetActive(true);
            Invoke("ResetAtkCol", atkColTime);
            Invoke("ResetAtkCool", atkCool);
        }
    }

    private void ResetAtkCol() => attackCol.SetActive(false);
    private void ResetAtkCool() => isAtk = false;

    private void GroundCheck()
    {
        //Debug.DrawRay(centerTr.position, Vector3.down * groundRayDist);
        if(Physics.Raycast(centerTr.position,Vector3.down,groundRayDist,LayerMask.GetMask("Ground","Wall","Object","Player")))
        {
            isJumping = false;
        }
        else
        {
            isJumping = true;
        }
    }

    private void RayHit()
    {
        //Debug.DrawRay(mainManager.cam.transform.position, mainManager.cam.transform.forward * rayDist, Color.red);
        if(Physics.Raycast(mainManager.cam.transform.position,mainManager.cam.transform.forward, out RaycastHit hit, rayDist, LayerMask.GetMask("Player","Item","Object")))
        {
            rayText.gameObject.SetActive(true);
            if (hit.transform.CompareTag("Player"))
            {
                rayText.text = hit.transform.GetComponent<PlayerScript>().playerId.ToString();
            }
            else if(hit.transform.CompareTag("Item"))
            {

            }
            else if(hit.transform.CompareTag("Object"))
            {

            }
        }
        else
        {
            rayText.gameObject.SetActive(false);
        }
    }

    public void HitPlayer(int myAct,int otherAct, int damage)
    {
        photonView.RPC("Damaged", NetManager.instance.idToPlayer[otherAct], myAct, otherAct, damage);
    }

    [PunRPC]
    private void Damaged(int otherAct, int myAct,int damage)
    {
        if(!isInvinci)
        {
            hp -= damage;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(playerId);
            stream.SendNext(hp);
            stream.SendNext(isDie);
        }
        else
        {
            playerId = (int)stream.ReceiveNext();
            hp = (int)stream.ReceiveNext();
            isDie = (bool)stream.ReceiveNext();
        }
    }
}
