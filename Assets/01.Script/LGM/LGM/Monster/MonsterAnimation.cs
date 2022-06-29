using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    //플레이어 추격을 그만두면 이동을 하면서 애니멘이션 대기모션 출력
    //랜덤이동이 연속으로 2번 출력시 애니메이션 굳음 현상
    public MonsterInfo2 info;
    public Animator ani;

    public float attackDelay;   //공격 딜레이
    private bool attacking;      //공격 딜레이 체크

    private void Awake()
    {
        // 유저가 들고있는 장비
        //UIManager.Instance.equipPanel.PlayerTool;
        ani = transform.GetChild(0).GetComponent<Animator>();
        ani.GetBehaviour<MonsterAttackBehaviour>().monAni = this;
        ani.GetBehaviour<RandomMoveBehaviour>().monAni = this;
        ani.GetBehaviour<MainBehaviour>().monAni = this;
        ani.GetBehaviour<MonsterHitBehaviour>().monAni = this;
        ExitTimeBehaviour[] arr = ani.GetBehaviours<ExitTimeBehaviour>();
        foreach(var a in arr)
        {
            a.monAni = this;
        }
        info = GetComponent<MonsterInfo2>();
    }
    private void Update()
    {
        if(info.isDeath)
        {
            info.collider.isTrigger = true;
            info.gatheringObj.isAllowGet = true;
            transform.tag = "ActionObj";
        }
        //이동에 따른 애니메이션 변화
        MonsterMoveSet();
        if (!info.isDeath)
        {
            //데미지 판정
            HitDamage();
        }

        //랜덤한 시간에 몬스터 울음소리 내기
        info.MonsterCrying();

        if (!info.type)
        {
            //type이 true면 선공형
            Type1();
        }
        else
        {
            if (info.isSerching2)
            {
                Type1();
            }
        }
        
    }

    
    //피격/체력감소   
    public void HitDamage()
    {
        if (info.beforeHp != info.hp)
        {
            //플레이어 장착도구에 따른 피격음
            switch (UIManager.Instance.equipPanel.PlayerTool)
            {
                case E_UseTool.None:

                    break;
                case E_UseTool.Default:
                    SoundManager.Instance.PlaySound(E_SoundType.Effect, E_Effect_Monster.Effect_Attack_Hit);
                    break;
                case E_UseTool.Bottle:
                    break;
                case E_UseTool.Axe:
                    break;
                case E_UseTool.Shovel:
                    break;
                case E_UseTool.Knife:
                    SoundManager.Instance.PlaySound(E_SoundType.Effect, E_Effect_Monster.Effect_Knife_Hit);
                    break;
                case E_UseTool.Pickaxe:
                    break;
                case E_UseTool.Hammer:
                    break;
                case E_UseTool.Max:
                    break;
            }
            //체력이 0보다 작을 시 사망 애니메이션 출력
            if (info.hp <= 0)
            {
                info.isDeath = true;
                ani.SetTrigger("DeathTrigger");
            }
            //체력이 남아있을시 피격 애니메이션 출력
            else
            {
                ani.SetTrigger("HitTrigger");
                if (info.type)
                {
                    info.isSerching2 = true;
                }
            }
            //감소된 체력 저장
            info.beforeHp = info.hp;
        }
    }

    public void Type1()
    {
        if (!info.isDeath)
        {
            //플레이어가 탐색 범위 안에 잇을때
            if (!(!info.searching && !info.isComback &&
                GetDistance(Player.Instance.transform.position, transform.position) > Mathf.Pow(info.searchDis, 2)))
            {
                /*
                 * 공격 범위내에 적이 있으면 이동을 멈추고 공격
                 * 공격 범위내에 적이 없으면 추격
                 */
                SearchTarget();
                Comeback();
                if (!info.isComback&& !attacking)
                {
                    StartCoroutine(AttackDelay());
                }
            }
        }
    }
    
    //복귀
    public void Comeback()
    {
        //플레이어가 몬스터의 탐색범위에 들어갔었다가 벗어나면 몬스터 후퇴
        if (info.searching && 
            GetDistance(transform.position, info.centerPos) > Mathf.Pow(info.centerDis, 2)) 
        {
            ani.SetInteger("State", 4);                 //추격 애니메이션 실행
            info.agent.SetDestination(info.centerPos);  //추격 시작 위치로 돌아감
            info.isComback = true;
        }

        //목적지 도착 시 실행
        if (info.isComback)
        {
            if (info.agent.velocity.sqrMagnitude >=
                Mathf.Pow(0.4f, 2) &&
                info.agent.remainingDistance <= 1.5f)
            {
                info.hp = info.maxHp;
                info.beforeHp = info.hp;
                ani.SetInteger("State", 0);
                info.searching = false;
                info.isComback = false;
                if (info.type)
                {
                    info.isSerching2 = false;
                }
            }
        }
    }

    //추격
    public void SearchTarget()
    {
        //[탐색범위내에 플레이어가 있을 시 추격]
        //플레이어와 몬스터 사이의 거리가 탐색 범위보다 작으면 실행
        if (GetDistance(Player.Instance.transform.position, transform.position) 
            <= Mathf.Pow(info.searchDis, 2))
        {
            if (!info.searching)
            {
                info.searching = true;                  //추격 중 체크
                ani.SetInteger("State", 4);             //추격 애니메이션 실행
                info.centerPos = transform.position;    //추격 시작 위치 저장 
            }
            if(!info. isComback)
                info.agent.SetDestination(Player.Instance.transform.position);
        }
    }


    //공격
    public void AttackTarget()
    {
        //플레이어와 몬스터의 거리가 공격범위보다 작으면 공격 애니메이션 출력
        if (GetDistance(Player.Instance.transform.position, transform.position) <= Mathf.Pow(info.attackDis, 2))
        {
            //공격 애니메이션이 반복 호출되는걸 막기 위한 체크
            //isAttack는 애니메이션이 시작할때 true가 되며 애니메이션이 끝날때 false
            if (!info.isAttack)
            {
                ani.SetTrigger("AttackTrigger");
                
            }
            //공격 애니메이션 실행 시 플래이어를 자연스럽게 바라보게 만듬    
            SmoothLookAt();
        }
    }
    public void SmoothLookAt()
    {
        Vector3 dir = Player.Instance.transform.position - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * info.speed);
    }

    //-----

    IEnumerator AttackDelay()
    {
        AttackTarget();
        attacking = true;
        yield return new WaitForSeconds(attackDelay);
        attacking = false;
    }
    //-----

    //몬스터 공격
    //*****
    /*public void AttackDamage()
    {
        //애니메이션이 시작될때 공격 할수 있도록 false로 초기화
        if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.2f)
        {
            info.damageDelay = false;
        }
        //애니메이션의 중간 부분
        if (!info.damageDelay && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
        {
            if (info.attackPower >= Player.Instance._Status.overHit)
            {
                Debug.Log((Player.Instance.transform.position - transform.position).normalized);
                Player.Instance.GetComponent<Rigidbody>().
                    AddForce((Player.Instance.transform.position - transform.position).normalized
                    * Player.Instance._Status.force, ForceMode.Impulse);
            }
            //플레이어 체력 감소
            Player.Instance._Status.Hp -= info.attackPower;
            //공격 후에 애니메이션 반복 하지 않도록 true체크
            info.damageDelay = true;
        }
    }*/

    public void RandomMotion()
    {
        info.randAni.motion = Random.Range(1, 5);
        ani.SetInteger("State", info.randAni.motion);
    }

    //랜덤 이동 좌표
    public Vector3 RandomPos()
    {
        int x;          //랜덤 이동 좌표 X
        int z;          //랜덤 이동 좌표 Y
        int reverseX;   //랜덤 이동 좌표 반전 여부 X축
        int reverseZ;   //랜덤 이동 좌표 반전 여부 Y축
        Vector3 targetPos;

        x = Random.Range(3, 6);
        z = Random.Range(3, 6);
        reverseX = Random.Range(0, 2);
        reverseZ = Random.Range(0, 2);
        if (reverseX == 1)
            x *= -1;
        if (reverseZ == 1)
            z *= -1;

        targetPos = transform.position;
        targetPos.x += x;
        targetPos.z += z;

        return targetPos;
    }

    //몬스터 이동에 따른 애니메이션 움직임 설정
    public void MonsterMoveSet()
    {
        Vector3 velocityX;
        float velocityZ;

        velocityX = info.agent.velocity;
        velocityZ = info.agent.velocity.x;

        ani.SetFloat("Z", velocityX.magnitude);   //몬스터가 nav가 가야할 방향으로 몬스터 회전.
        ani.SetFloat("X", velocityZ);            //몬스터 이동속도 적용.
    }
    public float GetDistance(Vector3 a, Vector3 b)
    {
        //플레이어와의 거리를 제곱 값 return
        return (a - b).sqrMagnitude;
    }
}
