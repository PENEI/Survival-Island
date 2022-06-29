using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public struct RandAni
{
    public int motion;
    public bool isState;
}
public class MonsterInfo2 : MonoBehaviour
{
    [Header("[몬스터 타입]")]
    public bool type;           //몬스터 타입
    public bool isSerching2;

    [Space]
    public GatheringObj gatheringObj;
    public Collider collider;


    [Header("[몬스터 정보]")]
    public float maxHp;         //최대 체력
    public float hp;            //체력
    public float beforeHp;      //피격 이전의 체력
    public float attackPower;   //힘 
    public float speed;         //기존 속도
    public float maxSpeed;      //추격시 최대 속도
    public NavMeshAgent agent;  //네비메쉬
    public AudioSource audioSource; //오디오
    

    [Header("[공격]")]
    public bool isAttack;       //공격 애니메이션 실행 여부
    public bool damageDelay;    //대미지 간격
    public float attackDis;     //공격 범위    
    
    [Header("[탐색]")]
    public float searchDis;     //탐색 범위
    public bool searching;      //탐색 중인지
    public Vector3 centerPos;   //탐색 가능 범위의 중심 위치
    public float centerDis;     //탐색 가능 범위 반지름
    
    [Header("[체크]")]
    public bool isComback;      //복귀 중 인지 체크
    public bool isDeath;        //죽는지 체크
    public bool isMotion;       //랜덤 대기모션 중인지 체크

    public RandAni randAni;

    [Header("[몬스터 울음소리]")]
    public int cryTime;         //울음소리 시작 시간

    private void Awake()
    {
        collider = GetComponent<Collider>();
        gatheringObj = GetComponent<GatheringObj>();
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        randAni = new RandAni();
        hp = maxHp;
        beforeHp = hp;
    }
    private void Start()
    {
        cryTime = Random.Range(0, 60);
    }
    private void OnDrawGizmos()
    {
        CGizmo.DrawGizmosCircleXZ(transform.position, searchDis, Color.white);
        CGizmo.DrawGizmosCircleXZ(transform.position, attackDis, Color.red);
        if (searching) 
        {
            CGizmo.DrawGizmosCircleXZ(centerPos, centerDis, Color.black);
        }
    }

    //동물 소리
    public void MonsterCrying()
    {
        //일정 시간마다 울음 소리 출력
        if (World.Instance.worldTime.GetTime("분") == cryTime)
        {
            //오디오가 출력 중이지 않으면 출력 후
            //다음에 소리낼 시간을 랜덤으로 지정
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            cryTime = Random.Range(0, 60);
        }
    }
}
