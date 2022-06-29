using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_RandemPosMove_Behaviour : StateMachineBehaviour
{
    private float loadingTime;

    private int randomArithmeticx;
    private int randomArithmeticz;

    private float randomPosx;
    private float randomPosY;
    private float randomPosZ;

    [HideInInspector]
    public Vector3 randomPos;
    [HideInInspector]
    public EnemyInfo info;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //x, z�� ������ �������� ����
        randomArithmeticx = Random.Range(0, 2) == 1 ? 1 : -1;
        randomArithmeticz = Random.Range(0, 2) == 1 ? 1 : -1;

        //x, z�� 1~5f������ ������ ��ǥ ����
        randomPosx = info.transform.position.x + (Random.Range(1, 6f) * randomArithmeticx);
        randomPosY = info.transform.position.y;
        randomPosZ = info.transform.position.z + (Random.Range(1, 6f) * randomArithmeticx);
        randomPos = new Vector3(randomPosx, randomPosY, randomPosZ);
        info.agent.SetDestination(randomPos);   //��ǥ�� �̵�

        animator.SetBool("IdleIng", true);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //��ǥ�� �̵��Ѱ� ���� ���� ���ؼ� �ִϸ��̼��� ������ �ʴ� ��츦 ���
        //4�ʵ��� ������ �Ǹ� �ִϸ��̼� ����
        loadingTime += Time.deltaTime;

        //�ִϸ��̼� �����ϰ� 4�� ���� �ִϸ��̼� ����
        if (loadingTime >= 4) 
        {
            animator.SetBool("IdleIng", false);
        }

        //���� ��ġ���� �Ÿ��� 1�����϶� ����
        if (info.GetDistance(info.transform.position, randomPos) <= 1f) 
        {
            animator.SetBool("IdleIng", false);
        }
        info.MonsterMoveSet();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        loadingTime = 0;
        animator.SetBool("IdleIng", false);
    }

}
