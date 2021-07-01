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
    //[SerializeField] private LayerMask WhatIsGrounds;             //Ground, Wall, Player
    public int damage=50;
    public int playerId;
    public bool isDie;
    public ParticleSystem bloodParticle;
    public GameObject scanObj;
    public bool bCompulsoryIdle = false;

    //private int walkToHash, runToHash, atkToHash, deadToHash;

    private GameObject playerModel, attackCol;
    [SerializeField] private int hp;
    private bool isJumping, isAtk;
    private bool isInvinci=false; //무적인지?
    private MainManager mainManager;
    private Vector3 moveVec;
    public Vector3 MoveVec { get { return moveVec; } }
    public bool IsJumping { get { return isJumping; } }
    private Message messageClass;
    private Transform JoinedTr;
    //private FixedJoint FixedObj;

    #region UI
    private InputField chatInput;
    #endregion

    private int langInt;

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
        messageClass = new Message();
    }

    void Init()
    {
        gameObject.AddComponent<AudioListener>();
        mainManager = GameManager.Instance.mainManager;
        mainManager.cam.target = centerTr;
        mainManager.cam.rotTarget = transform;
        mainManager.cam.player = this;
        chatInput = NetManager.instance.chatInput;
        langInt = (int)GameManager.Instance.savedData.option.language;
    }

    private void Update()
    {
        if (playerId == PhotonNetwork.LocalPlayer.ActorNumber && !isDie && !bCompulsoryIdle)
        {
            if (!mainManager.IsGoal)
            {
                Jump();
                Attack();
                _Input();  //테스트모드 전용
            }
        }
    }

    private void FixedUpdate()
    {
        if (playerId == PhotonNetwork.LocalPlayer.ActorNumber && !isDie)
        {
            if (!mainManager.IsGoal)
            {
                Move();
                GroundCheck();
                RayHit();
            }
        }
        rigid.angularVelocity = Vector3.zero;
    }
    void _Input()
    {
        if(Input.GetKeyDown(KeyCode.F)&&NetManager.instance.IsDev)
        {
            rigid.velocity = Vector3.up * jumpPower;
        }
        else if(Input.GetKeyDown(KeyCode.V)&&NetManager.instance.IsDev)
        {
            transform.position = mainManager.devVec;
        }
        
    }

    private void Move()
    { 
        if(!isJumping)
           moveVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        Vector3 worldDir = transform.TransformDirection(moveVec);
        Vector3 veloc = worldDir * (Input.GetKey(KeyCode.LeftShift) ? runSpeed : speed);
        Vector3 force = new Vector3(veloc.x - rigid.velocity.x, -gravity, veloc.z - rigid.velocity.z);
        if (!bCompulsoryIdle && !chatInput.isFocused)
        {
            rigid.AddForce(force, ForceMode.VelocityChange);
        }
        
        playerModel.transform.localRotation = Quaternion.Euler(0, transform.rotation.y, 0);
        ani.SetBool("walk", moveVec != Vector3.zero);
        ani.SetBool("run", moveVec != Vector3.zero && Input.GetKey(KeyCode.LeftShift));  // Animator.StringToHash하면 int형으로 가져올 수 있음. 
    }

    private void Jump()
    {
        if (!isJumping && Input.GetKeyDown(KeyCode.Space) && !chatInput.isFocused) 
        {
            rigid.velocity = Vector3.up * jumpPower;
            ani.SetTrigger("jump");
        }
    }

    private void Attack()
    {
        if(Input.GetMouseButtonDown(0) && !isAtk && !chatInput.isFocused)
        {
            ani.SetTrigger("attack");
            isAtk = true;
            attackCol.SetActive(true);
            Invoke("ResetAtkCol", atkColTime);
            Invoke("ResetAtkCool", atkCool);
            Invoke("AtkSoundDelay", 0.3f);
        }
    }

    private void ResetAtkCol() => attackCol.SetActive(false);
    private void ResetAtkCool() => isAtk = false;
    private void AtkSoundDelay() => SoundManager.Instance.PlaySoundEffect(1); //안좋긴한데 일단 이렇게

    private void GroundCheck()
    {
        //Debug.DrawRay(centerTr.position, Vector3.down * groundRayDist);
        if(Physics.Raycast(centerTr.position,Vector3.down, out RaycastHit hit, groundRayDist,LayerMask.GetMask("Ground","Wall","Player")))  //전역변수로 LayerMask써서 그걸로 넣는게 나을듯. 일단은 걍 이렇게
        {
            isJumping = false;
            if (hit.transform.CompareTag("JoinObj"))
            {
                JoinedTr = hit.transform;
                transform.parent = JoinedTr;
            }
            else
            {
                if (JoinedTr != null) transform.parent = null;
            }
            /*if (hit.transform.GetComponent<FixedJoint>() != null)
            {
                hit.transform.GetComponent<FixedJoint>().connectedBody = rigid;
                FixedObj = hit.transform.GetComponent<FixedJoint>();
            }*/
        }
        else
        {
            isJumping = true;
            if (JoinedTr != null) transform.parent = null;
        }
    }

    private void RayHit()
    {
         
        //Debug.DrawRay(mainManager.cam.transform.position, mainManager.cam.transform.forward * rayDist, Color.red);
        if(Physics.Raycast(mainManager.cam.transform.position,mainManager.cam.transform.forward, out RaycastHit hit, rayDist, LayerMask.GetMask("Player")))
        {
            if (scanObj == hit.transform.gameObject) return;
            scanObj = hit.transform.gameObject;

            if (hit.transform.CompareTag("Player") && hit.transform.gameObject!=gameObject)
            {
                mainManager.TxtDOTw(NetManager.instance.idToPlayer[hit.transform.GetComponent<PlayerScript>().playerId].NickName);
            }
        }
        else
        {
            mainManager.TxtOff();
            scanObj = null;
        }
    }

    public void Damaged(string msg)
    {
        if(!isInvinci)
        {
            messageClass = JsonUtility.FromJson<Message>(msg);
            hp -= messageClass.iValue;
            photonView.RPC("BloodEffect", RpcTarget.AllViaServer, messageClass.otherAct);

            if(hp<=0&&playerId==PhotonNetwork.LocalPlayer.ActorNumber)
            {
                Die(langInt==0? "Murdered" : "살해 당함");

                messageClass.otherAct = messageClass.myAct;
                messageClass.myAct = playerId;
                NetManager.instance.DiedPlayer(JsonUtility.ToJson(messageClass));
            }
        }
    }

     [PunRPC]
     private void BloodEffect(int act)
     {
        if(act==playerId)
        {
            bloodParticle.Play();
        }
     }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(playerId);              //음.....
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

    private void OnTriggerEnter(Collider other)
    {
        if (playerId == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            if(other.tag=="Goal")
            {
                mainManager.objManager.EnemysOff();
                rigid.velocity = Vector3.zero;
                rigid.angularVelocity = Vector3.zero;
                ani.SetBool("walk", false);

                isInvinci = true;

                mainManager.Goal();
            }
            else if(other.tag=="Save" && !isDie)
            {
                NetManager.instance.firstPos = transform.position;
                mainManager.PlayerTfSave(other.gameObject);
            }
            else if (other.tag == "Glich")
            {
                mainManager.cam.camGlich.enabled = true;
                SoundManager.Instance.PlaySoundEffect(0);
            }
            else if (other.CompareTag("Obstacle"))
            {
                //SelfMoveObj스크립트가 달려있어도 Obstacle태그가 없으면 안죽음.
                //string cause = other.transform.parent.GetComponent<SelfMoveObs>().deathCause;

                SelfMoveObs smo = other.transform.parent.GetComponent<SelfMoveObs>();  //부모에 SelfMoveObj달자
                string cause = langInt == 0 ? smo.deathCause_en : smo.deathCause;
                
                Die(cause);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerId == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            if (other.tag == "Glich")
            {
                mainManager.cam.camGlich.enabled = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (playerId == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            if (collision.gameObject.tag == "Rock")
            {
                Die(langInt==0? "Pressure" : "압사");
            }
            else if (collision.transform.CompareTag("Obstacle"))
            {
                //SelfMoveObj스크립트가 달려있어도 Obstacle태그가 없으면 안죽음.
                SelfMoveObs smo = collision.transform.parent.GetComponent<SelfMoveObs>();  //부모에 SelfMoveObj달자
                string cause = langInt == 0 ? smo.deathCause_en : smo.deathCause;
                Die(cause);
            }
        }
    }

    public void Die(string cause)
    {
        if (isDie || mainManager.IsGoal) return;

        hp = 0;
        isDie = true;
        ani.SetTrigger("death");
        isInvinci = true;
        mainManager.Die(cause);

        if(JoinedTr!=null) transform.parent = null;
        //if(FixedObj!=null) FixedObj.connectedBody = null;
    }

    public void Respawn()
    {
        ani.SetTrigger("jump");  
        hp = maxHp;
        isDie = false;
        isInvinci = false;
        transform.position = NetManager.instance.firstPos;
    }
}
