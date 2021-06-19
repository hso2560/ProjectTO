using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1 : MonoBehaviour, IDamageable
{
    public bool isActive;

    public float patrolDist;
    public Animator ani;

    [SerializeField] private Rigidbody rigid;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private int maxHp;
    [SerializeField] private float dForce;

    private int hp;
    private bool isInvinci = false;
    private bool bSetTarget=false;
    private bool isMoveStart = false;
    
    private Transform playerTr;
    private Vector3 target;

    [SerializeField] float speed, traceSpeed, _angle, dist;
    [SerializeField] GameObject bloodPrefab;
    [SerializeField] LayerMask pLayer;
    [SerializeField] float atkRange;

    private float angle;
    private Vector3 dir;
    private Transform tr;
    private float lastPatrolTime;
    private float nextPatrolTime;

    private void Start()
    {
        hp = maxHp;
    }

    public void InitData(Vector3 spawnPos, bool isActive)
    {
        agent.enabled = false;

        transform.position = spawnPos;
        target = transform.position;

        isInvinci = false;
        isMoveStart = true;
       
        this.isActive = isActive;

        agent.enabled = true;
    }

    private void OnEnable()
    {
        if (playerTr == null)
        {
            try
            {
                playerTr = GameManager.Instance.player.transform;
            }
            catch { }
        }

        nextPatrolTime = 1;
        lastPatrolTime = Time.time;
    }

    private void Update()
    {
        if (isMoveStart)
        {
            Patrol();
            Sight();
            agent.destination = bSetTarget ? playerTr.position : target;
            ani.SetBool("walk", target != transform.position);
        }
        
    }

    private void Patrol()
    {
        if(!bSetTarget)
        {
            agent.speed = speed;

            if(lastPatrolTime+nextPatrolTime<Time.time)
            {
                NextPatrolAction();
            }
            /*else
            {
                agent.SetDestination(target);
            }*/
        }
    }
    private void NextPatrolAction()
    {
        int i = Random.Range(0, 10);

        if (i <= 2)
        {
            target = transform.position;
        }
        else
        {
            float x = transform.position.x;
            float z = transform.position.z;
            target = new Vector3(Random.Range(x-patrolDist, x+patrolDist), transform.position.y, Random.Range(z-patrolDist, z+patrolDist));
        }

        nextPatrolTime = Random.Range(2f, 4.5f);
        lastPatrolTime = Time.time;
    }
    private void Sight()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, dist, pLayer);

        for(int i=0; i<cols.Length; i++)
        {
            if(cols[i].gameObject==playerTr.gameObject)
            {
                tr = cols[i].transform;
                dir = (tr.position - transform.position).normalized;
                angle = Vector3.Angle(dir, transform.forward);

                if (angle <= _angle * 0.5f)
                {
                    bSetTarget = true;
                    agent.speed = traceSpeed;
                    //agent.SetDestination(playerTr.position);

                    if (Vector3.Distance(transform.position, playerTr.position) <= atkRange)
                    {
                        Attack();
                    }
                    #region ÁÖ¼®
                    /* if (Physics.Raycast(transform.position, dir, out RaycastHit hit, dist))
                     {
                         if (hit.transform.gameObject == player.gameObject)
                         {


                         }
                     }*/
                    #endregion
                }
            }
        }

        if(bSetTarget)
        {
            dir = playerTr.position - transform.position;
            bSetTarget = !(dir.sqrMagnitude > dist * dist);
        }
    }

    private void FixedUpdate()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    private void Attack()
    {
        ani.SetTrigger("atk");
    }

    public void Damaged(int damage, Vector3 hitPos, Vector3 hitNormal)
    {
        if (isInvinci) return;

        isInvinci = true;
        Invoke("IvkInvc", 0.5f);
        hp -= damage;
        GameObject blood = Instantiate(bloodPrefab, hitPos, Quaternion.LookRotation(hitNormal));
        rigid.AddForce(hitNormal.normalized * dForce);
        Destroy(blood, 0.6f);
        if(hp<=0)
        {
            Die();
        }
    }

    public void Die()
    {
        CancelInvoke("IvkInvc");
        isInvinci = true;
        ani.SetTrigger("die");
        hp = maxHp;
        bSetTarget = false;
        target = transform.position;
        isMoveStart = false;
        Invoke("IvkActive",3);
    }

    public void ResetData()
    {
        CancelInvoke("IvkInvc");
        CancelInvoke("IvkActive");
        ani.SetTrigger("atk");
        hp = maxHp;
        isMoveStart = false;
        target = transform.position;
        bSetTarget = false;

        gameObject.SetActive(isActive);
    }

    private void IvkInvc() => isInvinci = false;
    private void IvkActive() => gameObject.SetActive(false);
}
