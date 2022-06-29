using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitBehaviour : StateMachineBehaviour
{
    public MonsterAnimation monAni;
    public float beforeSpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //�ǰݽÿ��� �̵��� ���� 
        beforeSpeed = monAni.info.agent.speed;
        monAni.info.agent.speed = 0;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monAni.info.agent.speed = beforeSpeed;
    }
}
