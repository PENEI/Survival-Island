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
        //���� �̵��� ���� �ִϸ��̼� ������ ����
        MonsterMoveSet();
    }

    //���� �̵��� ���� �ִϸ��̼� ������ ����
    public void MonsterMoveSet()
    {
        Vector3 velocityX;
        float velocityZ;

        velocityX = info.agent.velocity;
        velocityZ = info.agent.velocity.x;

        ani.SetFloat("Z", velocityX.magnitude);   //���Ͱ� nav�� ������ �������� ���� ȸ��.
        ani.SetFloat("X", velocityZ);            //���� �̵��ӵ� ����.
    }

    //������ ��� ���
    public void RandomMotion()
    {
        info.randAni.motion = Random.Range(1, 5);
        ani.SetInteger("State", info.randAni.motion);
    }

    //���� �̵� ��ǥ
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


    //������Ʈ ���� �Ÿ� ���
    public float GetDistance(Vector3 a, Vector3 b)
    {
        //�÷��̾���� �Ÿ��� ���� �� return
        return (a - b).sqrMagnitude;
    }
}
