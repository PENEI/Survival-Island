using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Hit_Behaviour : StateMachineBehaviour
{
    [HideInInspector]
    public EnemyInfo info;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        info.state = E_Enemy_State.Hit;
        info.isHitAnimation = true;
        animator.SetBool("DontChange", (info.isDontChange = true));
        info.isFollow = true;
        info.centerPos = info.transform.position;    //추격 시작 위치 저장 
        info.target = Player.Instance.transform;     //타겟의 transform 저장
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        info.isHitAnimation = false;
        animator.SetBool("DontChange", (info.isDontChange = false));
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
