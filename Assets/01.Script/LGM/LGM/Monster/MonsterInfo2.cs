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
    [Header("[���� Ÿ��]")]
    public bool type;           //���� Ÿ��
    public bool isSerching2;

    [Space]
    public GatheringObj gatheringObj;
    public Collider collider;


    [Header("[���� ����]")]
    public float maxHp;         //�ִ� ü��
    public float hp;            //ü��
    public float beforeHp;      //�ǰ� ������ ü��
    public float attackPower;   //�� 
    public float speed;         //���� �ӵ�
    public float maxSpeed;      //�߰ݽ� �ִ� �ӵ�
    public NavMeshAgent agent;  //�׺�޽�
    public AudioSource audioSource; //�����
    

    [Header("[����]")]
    public bool isAttack;       //���� �ִϸ��̼� ���� ����
    public bool damageDelay;    //����� ����
    public float attackDis;     //���� ����    
    
    [Header("[Ž��]")]
    public float searchDis;     //Ž�� ����
    public bool searching;      //Ž�� ������
    public Vector3 centerPos;   //Ž�� ���� ������ �߽� ��ġ
    public float centerDis;     //Ž�� ���� ���� ������
    
    [Header("[üũ]")]
    public bool isComback;      //���� �� ���� üũ
    public bool isDeath;        //�״��� üũ
    public bool isMotion;       //���� ����� ������ üũ

    public RandAni randAni;

    [Header("[���� �����Ҹ�]")]
    public int cryTime;         //�����Ҹ� ���� �ð�

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

    //���� �Ҹ�
    public void MonsterCrying()
    {
        //���� �ð����� ���� �Ҹ� ���
        if (World.Instance.worldTime.GetTime("��") == cryTime)
        {
            //������� ��� ������ ������ ��� ��
            //������ �Ҹ��� �ð��� �������� ����
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            cryTime = Random.Range(0, 60);
        }
    }
}
