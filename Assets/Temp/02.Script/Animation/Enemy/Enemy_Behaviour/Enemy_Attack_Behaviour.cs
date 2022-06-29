using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack_Behaviour : StateMachineBehaviour
{
    [HideInInspector]
    public EnemyInfo info;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        info.state = E_Enemy_State.Attack;
        info.isAttackAnimation = true;
        animator.SetBool("DontChange", (info.isDontChange = true));
        info.weapon.SetActive(true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
    
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        info.isAttackAnimation = false;
        animator.SetBool("DontChange", (info.isDontChange = false));
        info.weapon.SetActive(false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
