using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTimeBehaviour : StateMachineBehaviour
{
    [HideInInspector]
    public MonsterAnimation monAni;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!monAni.info.searching && !monAni.info.isComback)
        {
            if (monAni.ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                monAni.info.randAni.isState = true;
                monAni.ani.SetBool("IsIdle", monAni.info.randAni.isState);
            }
            
        }
        if ((monAni.GetDistance(Player.Instance.transform.position, monAni.transform.position) <= Mathf.Pow(monAni.info.searchDis, 2)))
        {
            if (!monAni.info.type)
            {
                monAni.info.randAni.isState = true;
                monAni.ani.SetBool("IsIdle", monAni.info.randAni.isState);
                monAni.SearchTarget();
            }
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
