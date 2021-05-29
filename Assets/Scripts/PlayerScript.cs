using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Rendering;

public class PlayerScript : MonoBehaviourPun
{
    [SerializeField] private Animator ani;
    [SerializeField] private Transform centerTr;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private float runSpeed;
    [SerializeField] private float speed;
    [SerializeField] private int maxHp;
    [SerializeField] private float jumpPower;
    [SerializeField] private float groundRayDist;
    [SerializeField] private float gravity;
    [SerializeField] private GameObject[] bodys;
    [SerializeField] private GameObject[] weapons;
    public int playerId;
    public bool isDie;

    private GameObject playerModel;
    private int hp;
    private bool isJumping;
    private int act;
    private MainManager mainManager;
    private Vector3 moveVec;

    private void Start()
    {
        if(playerId==PhotonNetwork.LocalPlayer.ActorNumber)
        {
            act = PhotonNetwork.LocalPlayer.ActorNumber;
            gameObject.AddComponent<AudioListener>();
            mainManager = GameManager.Instance.mainManager;
            mainManager.cam.target = centerTr;
            mainManager.cam.rotTarget = transform;
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

    }

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
}
