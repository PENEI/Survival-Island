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
        //�ݺ����� �ʴ� �ִ�(��� ���)�� �ð��� �ٵǸ� ��� �ִϸ��̼� ����
        if(!stateInfo.loop)
        {
            if (stateInfo.normalizedTime >= 1f) 
            {
                animator.SetBool("IdleIng", false);
            }
        }

        //���ĵ����϶���
        if (info.type == E_Enemy_Type.Carnivore)
        {
            //�÷��̾ ������ Ž�� ���� �ȿ� ���� ���
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
