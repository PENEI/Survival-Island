using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Comeback_Behaviour : StateMachineBehaviour
{
    [HideInInspector]
    public EnemyInfo info;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        info.target = null;
        info.agent.ResetPath();     //저장되 있는 경로 지우기

        info.state = E_Enemy_State.Comeback;
        animator.SetBool("DontChange", (info.isDontChange = true));
        info.agent.SetDestination(info.centerPos);
        info.isFollow = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(info.GetDistance(info.transform.position, info.centerPos) <= 4f)
        {
            animator.SetBool("Comeback", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        info.hp = info.maxHp;
        info.agent.ResetPath();
        animator.SetBool("DontChange", (info.isDontChange = false));
    }
}
