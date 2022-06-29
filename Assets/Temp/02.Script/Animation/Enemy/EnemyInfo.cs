using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SerializeField]
public class EnemyInfo : MonoBehaviour
{
    [Header("[정보]")]
    public E_Enemy_Type type;       //적 타입
    //[HideInInspector]
    public E_Enemy_State state;  //몬스터 상태
    public float attackPower;
    public float maxHp;
    [HideInInspector]
    public float hp;
    public float moveSpeed;
    public float attackDelay;
    public float woundPercent;  //공격 시 플레이어에게 외상을 걸 확률

    [HideInInspector]
    public bool isHitAnimation;     //피격 애니메이션 출력 중 인지 체크
    [HideInInspector]
    public bool isAttackAnimation;  //공격 애니메이션 출력 중 인지 체크
    [HideInInspector]
    public bool isFollow;       //추격 중인지 체크
    [HideInInspector]
    public bool isDontChange;   //애니메이션이 바꾸면 안되는 상황(공격, 피격)
    [HideInInspector]
    public bool isAttackDelay;  //공격 후 다음 공격까지의 쿨타임

    [HideInInspector]
    public AudioSource audioSource; //오디오
    [HideInInspector]
    public int soundTime;       //울음 소리를 내는 시간
    [Header("[사운드]")]
    public int soundInterval;   //울음 소리 간격

    [Header("[범위]")] 
    public float searchDis;
    public float attackDis;
    [HideInInspector]
    public Vector3 centerPos;     //추격 시작 위치
    public float centerDis;     //추격 가능 범위 
    [HideInInspector]
    public Transform target;    //타겟

    [HideInInspector]
    public GatheringObj gatheringObj;
    [HideInInspector]
    public NavMeshAgent agent;  //네비메쉬
    [HideInInspector]
    public bool isIdleCoroutine;    //대기 애니메이션 코루틴이 돌고있는지 체크
    [HideInInspector]
    public Animator ani;

    public GameObject weapon;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        gatheringObj = GetComponent<GatheringObj>();

        ani = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        agent.speed = moveSpeed;
        hp = maxHp;
    }

    private void OnDrawGizmos()
    {
        //탐색 범위
        CGizmo.DrawGizmosCircleXZ(transform.position, searchDis, Color.white);
        //공격 범위
        CGizmo.DrawGizmosCircleXZ(transform.position, attackDis, Color.red);

        //추격 시 생성되는 기즈모
        if (isFollow)
        {
            CGizmo.DrawGizmosCircleXZ(centerPos, centerDis, Color.black);
        }
    }

    //a와 b의 거리 구하기
    public float GetDistance(Vector3 a, Vector3 b)
    {
        //플레이어와의 거리를 제곱 값 return
        return (a - b).sqrMagnitude;
    }

    //오브젝트가 플레이어를 향해 회전
    public void SmoothLookAt()
    {
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 dir = new Vector3(playerPos.x - transform.position.x, transform.position.y, playerPos.z - transform.position.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * moveSpeed);
    }

    //동물 소리
    public void MonsterCrying()
    {
        //일정 시간마다 울음 소리 출력
        if ((int)(World.Instance.worldTime.GetTime("분")) == soundTime)
        {
            //오디오가 출력 중이지 않으면 출력 후
            //다음에 소리낼 시간을 랜덤으로 지정
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            soundTime = Random.Range(0, soundInterval);
        }
    }

    #region 이동
    //몬스터 이동에 따른 애니메이션 움직임 설정
    public void MonsterMoveSet()
    {
        float velocityX = agent.velocity.magnitude * moveSpeed;
        float velocityZ = agent.velocity.x;

        ani.SetFloat("Vertical", velocityX);            //몬스터 이동속도 적용.

        //몬스터가 nav가 가야할 방향으로 몬스터 회전.
        if (90 < transform.eulerAngles.y && 270 > transform.eulerAngles.y)
        {
            ani.SetFloat("Horizontal", -velocityZ);
        }
        else
        {
            ani.SetFloat("Horizontal", velocityZ);
        }
    }
    #endregion

    #region 대기/추격
    public void SearchTarget_Carnivore()    //선공형
    {
        //복귀
        if (Comeback()) { return; }

        //복귀가 아닐 때
        if (!ani.GetBool("Comeback"))
        {
            //[탐색범위내에 플레이어가 있을 시 추격]
            //플레이어와 몬스터 사이의 거리가 탐색 범위보다 작으면 실행
            if (Distance_A_B_Dis(Player.Instance.transform.position, transform.position, searchDis))
            {

                //공격범위 안에있으면 공격
                if (AttackTarget()) { return; }

                //플레이어가 범위 안에 있으나 추격 중이지 않을 때 플레이어 추격
                if (!isFollow)
                {
                    centerPos = transform.position;    //추격 시작 위치 저장 
                    target = Player.Instance.transform;     //타겟의 transform 저장
                    isFollow = true;                   //추격 중 체크
                }

                MonsterMoveSet();   //몬스터 이동
            }

            //플레이어가 범위 밖에 있을때
            else if (!Distance_A_B_Dis(Player.Instance.transform.position, transform.position, searchDis))
            {
                if (state != E_Enemy_State.Idle && !isFollow)
                {
                    ani.SetTrigger("IdleTrigger");
                }
            }
        }
    }

    public void SearchTarget_Herbivores()
    {
        //복귀
        if (Comeback()) { return; }

        //복귀가 아닐 때
        if (!ani.GetBool("Comeback"))
        {
            //추격 상태일 때  //피격에서 실행
            if (isFollow)
            {
                //공격범위 안에있으면 공격
                if (AttackTarget()) { return; }

                //플레이어가 범위 안에 있으나 추격 중이지 않을 때 플레이어 추격
                if (!isFollow)
                {
                    centerPos = transform.position;    //추격 시작 위치 저장 
                    target = Player.Instance.transform;     //타겟의 transform 저장
                    isFollow = true;                   //추격 중 체크
                }

                MonsterMoveSet();   //몬스터 이동
            }

            //대기 상태가 아니면서 전투 중이 아닐때 대기 모션 출력
            else if (state != E_Enemy_State.Idle && !isFollow)
            {
                ani.SetTrigger("IdleTrigger");
            }
        
        }
    }
    #endregion

    #region 공격
    public bool AttackTarget()
    {
        //플레이어와 몬스터의 거리가 공격범위보다 작으면 공격 애니메이션 출력
        //공격 딜레이 대기
        if (!isAttackDelay && Distance_A_B_Dis(Player.Instance.transform.position, transform.position, attackDis))
        {
            //공격 애니메이션이 반복 호출되는걸 막기 위한 체크
            //isAttack는 애니메이션이 시작할때 true가 되며 애니메이션이 끝날때 false
            if (state != E_Enemy_State.Attack)
            {
                if (!isHitAnimation) { ani.SetTrigger("AttackTrigger"); }              
                StartCoroutine(AttackDelay());
            }
            //공격 애니메이션 실행 시 플래이어를 자연스럽게 바라보게 만듬    
            SmoothLookAt();
            return true;
        }
        return false;
    }
    
    IEnumerator AttackDelay()
    {
        isAttackDelay = true;
        yield return new WaitForSeconds(attackDelay);
        isAttackDelay = false;
    }
    #endregion

    #region 피격 / 죽음
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Weapon"))
        {
            //플레이어 공격력 만큼 데미지 감소
            hp -= Player.Instance._Status.status.attackPower;

            //플레이어 장착도구에 따른 사운드 출력
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
            if (hp <= 0 )
            {
                if (!ani.GetBool("Death"))
                {
                    ani.SetBool("Death", true);
                    ani.SetTrigger("DeathTrigger");
                }
            }
            //체력이 남아있을시 피격 애니메이션 출력
            else
            {
                Debug.Log("Hit2");
                if (!isDontChange && !isAttackAnimation) { ani.SetTrigger("HitTrigger"); }
            }
        }
    }
    //죽은 상태(행동 불가)
    public bool DeathState()
    {
        if (ani.GetBool("Death"))
        {
            agent.isStopped = true;
            audioSource.Stop();
            target = null;
            return true;
        }
        return false;
    }

    #endregion

    #region 복귀
    public bool Comeback()
    {
        if (!Distance_A_B_Dis(centerPos, transform.position, centerDis) && isFollow) 
        {
            if (state != E_Enemy_State.Comeback)
            {
                ani.SetBool("Comeback", true);
            }
            return true;
        }
        return false;
    }
    #endregion    
    
    //a의 전방(dis) 범위 안에 b가 있으면 true, 아니면 false
    public bool Distance_A_B_Dis(Vector3 a, Vector3 b,float dis)
    {
        if (GetDistance(a, b) <= Mathf.Pow(dis, 2))
            return true;

        return false;
    }

}
