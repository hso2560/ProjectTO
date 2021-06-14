using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LB3DCreature : MonoBehaviour
{
    public Animator creatureAnimator;
   
    public virtual void Start()
    {
        SetupAnimator();
        
    }

    public void ToggleLocomotionState(float val) {
        if (val > 0.1f || val < -0.1f)
        {
            creatureAnimator.SetBool("is_idle", false);
            creatureAnimator.SetBool("is_walking", true);
        }
        else {
            creatureAnimator.SetBool("is_idle", true);
            creatureAnimator.SetBool("is_walking", false);
        }
    }

    public void SetIdleMode1(float amount) {
        amount = Mathf.Clamp(amount, 0, 1);
        creatureAnimator.SetFloat("Blend_Idle", amount);        
    }

    public void SetWalkForwardBack(float amount) {
        amount = Mathf.Clamp(amount, -1, 1);
     
        creatureAnimator.SetFloat("Blend_Walk_Backward_Forward", amount);
        ToggleLocomotionState(amount);
    }

    public void SetWalkLeftRight(float amount)
    {
        amount = Mathf.Clamp(amount, -1, 1);
        creatureAnimator.SetFloat("Blend_Walk_Left_Right", amount);
        ToggleLocomotionState(amount);
    }

    public void DoAttack() {
        creatureAnimator.SetTrigger("do_attack");
    }
    public void DoAttackMelee1() {        
        creatureAnimator.SetTrigger("do_attack_melee_1");
        DoAttack();
    }
    public void DoAttackMelee2()
    {
        creatureAnimator.SetTrigger("do_attack_melee_2");
        DoAttack();
    }
    public void DoAttackMelee3()
    {
        creatureAnimator.SetTrigger("do_attack_melee_3");
        DoAttack();
    }
    public void DoAttackMelee4()
    {
        creatureAnimator.SetTrigger("do_attack_melee_4");
        DoAttack();
    }
    public void DoAttackRanged1()
    {
        creatureAnimator.SetTrigger("do_attack_ranged_1");
        DoAttack();
    }

    public void DoTakeDamage() {
        creatureAnimator.SetTrigger("do_take_damage");
    }
    public void DoTakeDamage1()
    {
        creatureAnimator.SetTrigger("do_take_damage_1");
        DoTakeDamage();
    }
    public void DoTakeDamage2()
    {
        creatureAnimator.SetTrigger("do_take_damage_2");
        DoTakeDamage();
    }
    public void DoTakeDamage3()
    {
        creatureAnimator.SetTrigger("do_take_damage_3");
        DoTakeDamage();
    }
    public void DoDeath()
    {
        creatureAnimator.SetTrigger("do_death");
    }
    public void DoDeath1()
    {
        creatureAnimator.SetTrigger("do_death_1");
    }
    public void DoDeath2()
    {
        creatureAnimator.SetTrigger("do_death_2");
    }
    public void DoDeath3()
    {
        creatureAnimator.SetTrigger("do_death_3");
    }



    public void SetupAnimator() {
        creatureAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }
}
