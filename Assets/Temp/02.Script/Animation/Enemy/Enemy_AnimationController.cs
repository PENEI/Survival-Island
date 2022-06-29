using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AnimationController : MonoBehaviour
{
    [HideInInspector]
    public EnemyInfo info;
    [HideInInspector]
    public Animator ani;

    public Collider[] enemyCollider;
    private void Awake()
    {
        info = GetComponentInParent<EnemyInfo>();
        ani = GetComponent<Animator>();

        ani.GetBehaviour<Enemy_RandomIdle_Behaviour>().info = info;
        ani.GetBehaviour<Enemy_Main_Behaviour>().info = info;
        ani.GetBehaviour<Enemy_RandemPosMove_Behaviour>().info = info;
        ani.GetBehaviour<Enemy_Attack_Behaviour>().info = info;
        ani.GetBehaviour<Enemy_Hit_Behaviour>().info = info;
        ani.GetBehaviour<Enemy_Comeback_Behaviour>().info = info;
        ani.GetBehaviour<Enemy_Death_Behaviour>().info = info;
        ani.GetBehaviour<Enemy_Death_Behaviour>().collider = enemyCollider;
    }
    private void Update()
    {
        if(info.DeathState()) { return; }

        //Ÿ�Ժ� ����
        switch(info.type)
        {
            case E_Enemy_Type.Carnivore:
                info.SearchTarget_Carnivore();     // ����
                break;
            case E_Enemy_Type.Herbivores:
                info.SearchTarget_Herbivores();     // �İ�
                break;
        }      

        info.MonsterCrying();//���� �Ҹ�

        if (info.target != null)
        {
            info.agent.SetDestination(info.target.position);
        }
    }

}
