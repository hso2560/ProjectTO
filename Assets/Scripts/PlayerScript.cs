using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Animator ani;
    [SerializeField] private Transform centerTr;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private float runSpeed;
    [SerializeField] private float speed;
    [SerializeField] private int maxHp;
    [SerializeField] private float jumpPower;
    [SerializeField] private float groundRayDist;
    public int id;
    public bool isDie;

    private GameObject playerModel;
    private int hp;
    private bool isJumping;

    private void Start()
    {
        gameObject.AddComponent<AudioListener>();
        playerModel = ani.gameObject;
        hp = maxHp;
    }

    private void Update()
    {
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
        GroundCheck();
    }

    private void Move()
    {
        Vector3 moveVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 worldDir = transform.TransformDirection(moveVec);
        Vector3 veloc = worldDir * (Input.GetKey(KeyCode.LeftShift) ? runSpeed : speed);
        Vector3 force = new Vector3(veloc.x - rigid.velocity.x, 0, veloc.z - rigid.velocity.z);
        rigid.AddForce(force, ForceMode.VelocityChange);

        playerModel.transform.localRotation = Quaternion.Euler(0, transform.rotation.y, 0);
    }

    private void Jump()
    {
        if (!isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            rigid.velocity = Vector3.up * jumpPower;
        }
    }

    private void GroundCheck()
    {
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
