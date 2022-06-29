using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationControll : MonoBehaviour
{
    public MonsterInfo2 info;
    public Animator ani;
    void Start()
    {
        
    }

    
    void Update()
    {
        //몬스터 이동에 따른 애니메이션 움직임 설정
        MonsterMoveSet();
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

    //랜덤한 대기 모션
    public void RandomMotion()
    {
        info.randAni.motion = Random.Range(1, 5);
        ani.SetInteger("State", info.randAni.motion);
    }

    //랜덤 이동 좌표
    public Vector3 RandomPos()
    {
        float randomX;
        float randomZ;

        Vector3 targetPos = transform.position;

        randomX = Random.Range(3, 6) * Mathf.Sign(Random.Range(-1, 1));
        randomZ = Random.Range(3, 6) * Mathf.Sign(Random.Range(-1, 1));

        targetPos.x += randomX;
        targetPos.z += randomZ;

        Debug.Log(Mathf.Sign(Random.Range(-1, 1)));
        return targetPos;
    }


    //오브젝트 간의 거리 계산
    public float GetDistance(Vector3 a, Vector3 b)
    {
        //플레이어와의 거리를 제곱 값 return
        return (a - b).sqrMagnitude;
    }
}
