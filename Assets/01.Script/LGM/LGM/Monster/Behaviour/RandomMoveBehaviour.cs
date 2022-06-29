using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoveBehaviour : StateMachineBehaviour
{
    public MonsterAnimation monAni;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            //monAni.info.agent.SetDestination(monAni.RandomPos());
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (monAni.info.agent.velocity.sqrMagnitude >= Mathf.Pow(0.4f, 2) && monAni.info.agent.remainingDistance <= 1.5f)
        {
            monAni.info.randAni.isState = true;
            monAni.ani.SetBool("IsIdle", monAni.info.randAni.isState);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            monAni.info.randAni.isState = false;
            monAni.ani.SetBool("IsIdle", monAni.info.randAni.isState);
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
