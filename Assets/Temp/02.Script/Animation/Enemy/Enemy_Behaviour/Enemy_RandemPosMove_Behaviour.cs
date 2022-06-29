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
        //x, z의 방향을 랜덤으로 설정
        randomArithmeticx = Random.Range(0, 2) == 1 ? 1 : -1;
        randomArithmeticz = Random.Range(0, 2) == 1 ? 1 : -1;

        //x, z축 1~5f까지의 랜덤한 좌표 설정
        randomPosx = info.transform.position.x + (Random.Range(1, 6f) * randomArithmeticx);
        randomPosY = info.transform.position.y;
        randomPosZ = info.transform.position.z + (Random.Range(1, 6f) * randomArithmeticx);
        randomPos = new Vector3(randomPosx, randomPosY, randomPosZ);
        info.agent.SetDestination(randomPos);   //좌표로 이동

        animator.SetBool("IdleIng", true);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //좌표가 이동한계 범위 밖을 향해서 애니메이션이 끝나지 않는 경우를 대비
        //4초동안 실행이 되면 애니메이션 종류
        loadingTime += Time.deltaTime;

        //애니메이션 실행하고 4초 이후 애니메이션 종료
        if (loadingTime >= 4) 
        {
            animator.SetBool("IdleIng", false);
        }

        //랜덤 위치와의 거리가 1이하일때 종료
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
