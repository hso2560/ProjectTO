using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LB3DRandomFrameStart : MonoBehaviour
{
    void Start()
    {       
        Animator anim = GetComponent<Animator>();        
        anim.Play(anim.GetCurrentAnimatorStateInfo(0).fullPathHash, -1, Random.Range(0f, 1f));        
    }
}
