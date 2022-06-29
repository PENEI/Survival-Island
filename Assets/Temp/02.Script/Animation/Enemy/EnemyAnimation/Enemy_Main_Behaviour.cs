using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Main_Behaviour : StateMachineBehaviour
{
    [HideInInspector]
    public EnemyInfo info;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        info.state = E_Enemy_State.Move;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
