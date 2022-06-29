using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_IdleAnimation_Behaviour : StateMachineBehaviour
{
    public AnimationClip ani;
    private float _time;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance._Animation.state = E_Player_State.Idle;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _time += Time.deltaTime;
        //애니메이션이 끝까지 출력되면 대기 모션 종료
        //플레이어가 대기 상태가 아닐 시 대기 모션 종료
        if (AnimationManager.Instance.animationTime[ani.name] <= _time)
        {
            animator.SetBool("IdleExit", true);
        }
        if (Player.Instance._Animation.state != E_Player_State.Idle)
        {
            animator.SetBool("IdleExit", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _time = 0;
        animator.SetBool("IdleExit", false);
    }
}