using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_RandomIdle_Behaviour : StateMachineBehaviour
{
    [HideInInspector]
    public EnemyInfo info;
    public int randomA;
    public int randomB;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        info.state = E_Enemy_State.Idle;
        animator.SetInteger("IdleNumber", Random.Range(randomA, randomB + 1));
        animator.SetBool("IdleIng", true);
    }
    
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //반복하지 않는 애니(대기 모션)의 시간이 다되면 대기 애니메이션 종료
        if(!stateInfo.loop)
        {
            if (stateInfo.normalizedTime >= 1f) 
            {
                animator.SetBool("IdleIng", false);
            }
        }

        //육식동물일때만
        if (info.type == E_Enemy_Type.Carnivore)
        {
            //플레이어가 몬스터의 탐색 범위 안에 있을 경우
            if (info.Distance_A_B_Dis(Player.Instance.transform.position, animator.transform.position, info.searchDis))
            {
                animator.SetBool("IdleIng", false);
            }
        }
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IdleIng", false);
    }
}
