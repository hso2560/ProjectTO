using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Rendering;
using DG.Tweening;

public class PlayerScript : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private Animator ani;
    [SerializeField] private Transform centerTr;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private float runSpeed;
    [SerializeField] private float speed;
    [SerializeField] private int maxHp;
    [SerializeField] private float jumpPower;
    [SerializeField] private float groundRayDist, rayDist;  //�������� ���� ���� ����, ���� ���� ���� ����
    [SerializeField] private float gravity;
    [SerializeField] private GameObject[] bodys;
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private float atkCool=1f, atkColTime=0.5f;  //���� ��, ���� �ݶ��̴� �ڽ��� Ȱ��ȭ�Ǿ��ִ� �ð�
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
    private bool isInvinci=false; //��������?
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

    private CamMove cam;
    private int langInt;
    private Vector3 playerScale;

    #region ������ �ƴ�
    private bool isBenefit = false;
    private bool isRewind = false;
    public bool IsRewind { get { return isRewind; } }
    private List<TrmInfo> trInfos = new List<TrmInfo>();
    private TrmInfo formerInfo;
    [SerializeField] private int saveTrmMaxCnt = 600;

    private readonly int devID = 25504;
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
        messageClass = new Message();
        playerScale = transform.localScale;
    }

    void Init()
    {
        gameObject.AddComponent<AudioListener>();
        mainManager = GameManager.Instance.mainManager;
        cam = mainManager.cam;
        cam.target = centerTr;
        cam.rotTarget = transform;
        cam.player = this;
        chatInput = NetManager.instance.chatInput;
        langInt = (int)GameManager.Instance.savedData.option.language;
        isBenefit = (GameManager.Instance.savedData.userInfo.isClear && GameManager.Instance.savedData.userInfo.devId == devID);
    }

    private void Update()
    {
        if (playerId == PhotonNetwork.LocalPlayer.ActorNumber && !isDie && !bCompulsoryIdle)
        {
            if (!mainManager.IsGoal)
            {
                if (!isRewind)
                {
                    Jump();
                    Attack();
                }
                _Input();  //�׽�Ʈ��� ����
                Benefit();
            }
        }
    }

    private void FixedUpdate()
    {
        if (playerId == PhotonNetwork.LocalPlayer.ActorNumber && !isDie)
        {
            if (!mainManager.IsGoal)
            {
                if (!isRewind)
                {
                    Move();
                }
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
        
        if(Input.GetKeyDown(KeyCode.X) && isBenefit)
        {
            isRewind = true;

            mainManager.rewindPanel.DOKill();
            mainManager.rewindPanel.DOFade(1, 0.4f);
            if (!mainManager.isLast) GameManager.Instance.bgmAudio.pitch = -1;
            else GameManager.Instance.bgmAudio.pitch = 1.3f;
        }
        if(Input.GetKeyUp(KeyCode.X) && isBenefit)
        {
            isRewind = false;

            mainManager.rewindPanel.DOKill();
            mainManager.rewindPanel.DOFade(0, 0.3f);
            if (!mainManager.isLast) GameManager.Instance.bgmAudio.pitch = 1;
            else GameManager.Instance.bgmAudio.pitch = -1.3f;
        }
    }

    void Benefit()
    {
        if (!isBenefit) return;

        if (!isRewind) Record();
        else Rewind();
    }
    void Record()
    {
        if(trInfos.Count>saveTrmMaxCnt)
        {
            trInfos.RemoveAt(0);
        }

        trInfos.Add(new TrmInfo(transform.position, cam.transform.rotation));
    }
    void Rewind()
    {
        if(trInfos.Count==0)
        {
            trInfos.Add(new TrmInfo(transform.position, cam.transform.rotation));
            return;
        }

        formerInfo = trInfos[trInfos.Count - 1];

        transform.position = formerInfo.position;
        cam.transform.rotation = formerInfo.rotation;
        transform.rotation = Quaternion.Euler(0, formerInfo.rotation.eulerAngles.y, 0);

        trInfos.RemoveAt(trInfos.Count - 1);
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
        
        //playerModel.transform.localRotation = Quaternion.Euler(0, transform.rotation.y, 0);
        ani.SetBool("walk", moveVec != Vector3.zero);
        ani.SetBool("run", moveVec != Vector3.zero && Input.GetKey(KeyCode.LeftShift));  // Animator.StringToHash�ϸ� int������ ������ �� ����. 
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
    private void AtkSoundDelay() => SoundManager.Instance.PlaySoundEffect(1); //�������ѵ� �ϴ� �̷���

    private void GroundCheck()
    {
        //Debug.DrawRay(centerTr.position, Vector3.down * groundRayDist);
        if(Physics.Raycast(centerTr.position,Vector3.down, out RaycastHit hit, groundRayDist,LayerMask.GetMask("Ground","Wall","Player")))  //���������� LayerMask�Ἥ �װɷ� �ִ°� ������. �ϴ��� �� �̷���
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
                Die(langInt==0? "Murdered" : "���� ����");

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
            stream.SendNext(playerId);              //��.....
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
                //SelfMoveObj��ũ��Ʈ�� �޷��־ Obstacle�±װ� ������ ������.
                //string cause = other.transform.parent.GetComponent<SelfMoveObs>().deathCause;

                SelfMoveObs smo = other.transform.parent.GetComponent<SelfMoveObs>();  //�θ� SelfMoveObj����
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
                Die(langInt==0? "Pressure" : "�л�");
            }
            else if (collision.transform.CompareTag("Obstacle"))
            {
                //SelfMoveObj��ũ��Ʈ�� �޷��־ Obstacle�±װ� ������ ������.
                SelfMoveObs smo = collision.transform.parent.GetComponent<SelfMoveObs>();  //�θ� SelfMoveObj����
                string cause = langInt == 0 ? smo.deathCause_en : smo.deathCause;
                Die(cause);
            }
        }
    }

    public void Die(string cause)
    {
        if (isDie || mainManager.IsGoal) return;

        trInfos.Clear();
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
        transform.localScale = playerScale;
    }
}
