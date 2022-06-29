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
        //�ִϸ��̼��� ������ ��µǸ� ��� ��� ����
        //�÷��̾ ��� ���°� �ƴ� �� ��� ��� ����
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