using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LB3DCreatureUI : MonoBehaviour
{

    public LB3DCreature creature;

    public Slider sliderIdle;
    public Slider sliderWalkBackForward;
    public Slider sliderWalkLeftRight;

    public Button attackMelee1;
    public Button attackMelee2;
    public Button attackMelee3;
    public Button attackMelee4;
    public Button attackRanged1;
    public Button takeDamage1;
    public Button takeDamage2;
    public Button takeDamage3;
    public Button die1;
    public Button die2;
    public Button die3;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    // Start is called before ... (why doesn't anyone ever delete these default comments?)
    void Start()
    {
        SetupResetPosition();
        SetupIdle1();
        SetupWalk1();
        SetupAttacks();
        SetupTakeDamage();
    }

    //Reset
    public void SetupResetPosition() {
        originalPosition = creature.transform.position;
        originalRotation = creature.transform.rotation;
    }

    public void ResetPosition() {
        creature.transform.position = originalPosition;
        creature.transform.rotation = originalRotation;
    }

    //IDLE
    public void SetupIdle1() {
        if (!sliderIdle) return;
        sliderIdle.onValueChanged.AddListener(delegate { UpdateIdle1(); });
    }
    public void UpdateIdle1() {
        if (!sliderIdle) return;
        creature.SetIdleMode1(sliderIdle.value);
    }

    //WALK
    public void SetupWalk1() {
        if (sliderWalkBackForward)
        {
            sliderWalkBackForward.onValueChanged.AddListener(delegate { UpdateWalkForwardBack(); });
        }
        if (sliderWalkLeftRight)
        {
            sliderWalkLeftRight.onValueChanged.AddListener(delegate { UpdateWalkLeftRight(); });
        }
    }

    public void UpdateWalkForwardBack() {
        if (!sliderWalkBackForward) return;
        creature.SetWalkForwardBack(sliderWalkBackForward.value); 
        
    }
    public void UpdateWalkLeftRight()
    {
        if (!sliderWalkLeftRight) return;    
            creature.SetWalkLeftRight(sliderWalkLeftRight.value);         
    }

    //ATTACK
    public void SetupAttacks() {
        if (attackMelee1) { 
            attackMelee1.onClick.AddListener(delegate { DoAttackMelee1(); });
        }
        if (attackMelee2)
        {
            attackMelee2.onClick.AddListener(delegate { DoAttackMelee2(); });
        }
        if (attackMelee3)
        {
            attackMelee3.onClick.AddListener(delegate { DoAttackMelee3(); });
        }
        if (attackMelee4)
        {
            attackMelee4.onClick.AddListener(delegate { DoAttackMelee4(); });
        }
        if (attackRanged1)
        {
            attackRanged1.onClick.AddListener(delegate { DoAttackRanged1(); });
        }
    }
       
    public void DoAttackMelee1() {        
        if (!attackMelee1) return; //exit out if button not assigned    
        creature.DoAttackMelee1();
    }

    public void DoAttackMelee2()
    {
        if (!attackMelee2) return; //exit out if button not assigned
        creature.DoAttackMelee2();
    }

    public void DoAttackMelee3()
    {
        if (!attackMelee3) return; //exit out if button not assigned
        creature.DoAttackMelee3();
    }

    public void DoAttackMelee4()
    {
        if (!attackMelee3) return; //exit out if button not assigned
        creature.DoAttackMelee4();
    }

    public void DoAttackRanged1()
    {
        if (!attackRanged1) return; //exit out if button not assigned
        creature.DoAttackRanged1();
    }

    // TAKE DAMAGE
    public void SetupTakeDamage() {
        if (takeDamage1) {
            takeDamage1.onClick.AddListener(delegate { DoTakeDamage1(); });
        }

        if (takeDamage2) {
            takeDamage2.onClick.AddListener(delegate { DoTakeDamage2(); });
        }

        if (takeDamage3)
        {
            takeDamage3.onClick.AddListener(delegate { DoTakeDamage3(); });
        }

        if (die1) {
            die1.onClick.AddListener(delegate { DoDie1(); });
        }

        if (die2)
        {
            die2.onClick.AddListener(delegate { DoDie2(); });
        }
        if (die3)
        {
            die3.onClick.AddListener(delegate { DoDie3(); });
        }

    }

    public void DoTakeDamage1() {
        if (!takeDamage1) return;
        creature.DoTakeDamage1();
    }

    public void DoTakeDamage2() {
        if (!takeDamage2) return;
        creature.DoTakeDamage2();
    }

    public void DoTakeDamage3()
    {
        if (!takeDamage3) return;
        creature.DoTakeDamage3();
    }

    public void DoDie1() {
        if (!die1) return;
        creature.DoDeath();
        creature.DoDeath1();
    }
    public void DoDie2()
    {
        if (!die2) return;
        creature.DoDeath();
        creature.DoDeath2();
    }
    public void DoDie3()
    {
        if (!die3) return;
        creature.DoDeath();
        creature.DoDeath3();
    }

}
