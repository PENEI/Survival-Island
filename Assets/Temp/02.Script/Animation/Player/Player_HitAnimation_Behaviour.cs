using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HitAnimation_Behaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("HitDelay", true);     //피격 애니메이션 딜레이 체크
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance._Animation.hiting = false;
    }
}
