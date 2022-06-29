using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dead_Behaviour : StateMachineBehaviour
{
    public AnimationClip clip;
    public float _time;
    bool isdead;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.Control.SetAllowAll(false);
        animator.SetBool("isDead", true);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _time += Time.deltaTime;
        if (clip.length <= _time)
        {
            //사후 처리
            if(!isdead)
            {
                isdead = true;
                UIManager.Instance.GameOver.SetGameOver();
            }
            
        }
    }
}
