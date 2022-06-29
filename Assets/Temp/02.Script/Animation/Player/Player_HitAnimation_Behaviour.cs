using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HitAnimation_Behaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("HitDelay", true);     //�ǰ� �ִϸ��̼� ������ üũ
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance._Animation.hiting = false;
    }
}
