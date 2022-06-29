using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackBehaviour : StateMachineBehaviour
{
    public MonsterAnimation monAni;
    public float beforeSpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //공격시 플레이어를 처다보게하기
        monAni.info.isAttack = true;
        //공격 시에는 이동을 멈춤 
        beforeSpeed = monAni.info.agent.speed;
        monAni.info.agent.speed = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //플레이어에게 대미지
        //*****
        /*monAni.AttackDamage();*/        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monAni.info.isAttack = false;
        monAni.info.agent.speed = beforeSpeed;
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
