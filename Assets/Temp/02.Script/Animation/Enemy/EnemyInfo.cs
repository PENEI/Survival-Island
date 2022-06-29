using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SerializeField]
public class EnemyInfo : MonoBehaviour
{
    [Header("[����]")]
    public E_Enemy_Type type;       //�� Ÿ��
    //[HideInInspector]
    public E_Enemy_State state;  //���� ����
    public float attackPower;
    public float maxHp;
    [HideInInspector]
    public float hp;
    public float moveSpeed;
    public float attackDelay;
    public float woundPercent;  //���� �� �÷��̾�� �ܻ��� �� Ȯ��

    [HideInInspector]
    public bool isHitAnimation;     //�ǰ� �ִϸ��̼� ��� �� ���� üũ
    [HideInInspector]
    public bool isAttackAnimation;  //���� �ִϸ��̼� ��� �� ���� üũ
    [HideInInspector]
    public bool isFollow;       //�߰� ������ üũ
    [HideInInspector]
    public bool isDontChange;   //�ִϸ��̼��� �ٲٸ� �ȵǴ� ��Ȳ(����, �ǰ�)
    [HideInInspector]
    public bool isAttackDelay;  //���� �� ���� ���ݱ����� ��Ÿ��

    [HideInInspector]
    public AudioSource audioSource; //�����
    [HideInInspector]
    public int soundTime;       //���� �Ҹ��� ���� �ð�
    [Header("[����]")]
    public int soundInterval;   //���� �Ҹ� ����

    [Header("[����]")] 
    public float searchDis;
    public float attackDis;
    [HideInInspector]
    public Vector3 centerPos;     //�߰� ���� ��ġ
    public float centerDis;     //�߰� ���� ���� 
    [HideInInspector]
    public Transform target;    //Ÿ��

    [HideInInspector]
    public GatheringObj gatheringObj;
    [HideInInspector]
    public NavMeshAgent agent;  //�׺�޽�
    [HideInInspector]
    public bool isIdleCoroutine;    //��� �ִϸ��̼� �ڷ�ƾ�� �����ִ��� üũ
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
        //Ž�� ����
        CGizmo.DrawGizmosCircleXZ(transform.position, searchDis, Color.white);
        //���� ����
        CGizmo.DrawGizmosCircleXZ(transform.position, attackDis, Color.red);

        //�߰� �� �����Ǵ� �����
        if (isFollow)
        {
            CGizmo.DrawGizmosCircleXZ(centerPos, centerDis, Color.black);
        }
    }

    //a�� b�� �Ÿ� ���ϱ�
    public float GetDistance(Vector3 a, Vector3 b)
    {
        //�÷��̾���� �Ÿ��� ���� �� return
        return (a - b).sqrMagnitude;
    }

    //������Ʈ�� �÷��̾ ���� ȸ��
    public void SmoothLookAt()
    {
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 dir = new Vector3(playerPos.x - transform.position.x, transform.position.y, playerPos.z - transform.position.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * moveSpeed);
    }

    //���� �Ҹ�
    public void MonsterCrying()
    {
        //���� �ð����� ���� �Ҹ� ���
        if ((int)(World.Instance.worldTime.GetTime("��")) == soundTime)
        {
            //������� ��� ������ ������ ��� ��
            //������ �Ҹ��� �ð��� �������� ����
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            soundTime = Random.Range(0, soundInterval);
        }
    }

    #region �̵�
    //���� �̵��� ���� �ִϸ��̼� ������ ����
    public void MonsterMoveSet()
    {
        float velocityX = agent.velocity.magnitude * moveSpeed;
        float velocityZ = agent.velocity.x;

        ani.SetFloat("Vertical", velocityX);            //���� �̵��ӵ� ����.

        //���Ͱ� nav�� ������ �������� ���� ȸ��.
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

    #region ���/�߰�
    public void SearchTarget_Carnivore()    //������
    {
        //����
        if (Comeback()) { return; }

        //���Ͱ� �ƴ� ��
        if (!ani.GetBool("Comeback"))
        {
            //[Ž���������� �÷��̾ ���� �� �߰�]
            //�÷��̾�� ���� ������ �Ÿ��� Ž�� �������� ������ ����
            if (Distance_A_B_Dis(Player.Instance.transform.position, transform.position, searchDis))
            {

                //���ݹ��� �ȿ������� ����
                if (AttackTarget()) { return; }

                //�÷��̾ ���� �ȿ� ������ �߰� ������ ���� �� �÷��̾� �߰�
                if (!isFollow)
                {
                    centerPos = transform.position;    //�߰� ���� ��ġ ���� 
                    target = Player.Instance.transform;     //Ÿ���� transform ����
                    isFollow = true;                   //�߰� �� üũ
                }

                MonsterMoveSet();   //���� �̵�
            }

            //�÷��̾ ���� �ۿ� ������
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
        //����
        if (Comeback()) { return; }

        //���Ͱ� �ƴ� ��
        if (!ani.GetBool("Comeback"))
        {
            //�߰� ������ ��  //�ǰݿ��� ����
            if (isFollow)
            {
                //���ݹ��� �ȿ������� ����
                if (AttackTarget()) { return; }

                //�÷��̾ ���� �ȿ� ������ �߰� ������ ���� �� �÷��̾� �߰�
                if (!isFollow)
                {
                    centerPos = transform.position;    //�߰� ���� ��ġ ���� 
                    target = Player.Instance.transform;     //Ÿ���� transform ����
                    isFollow = true;                   //�߰� �� üũ
                }

                MonsterMoveSet();   //���� �̵�
            }

            //��� ���°� �ƴϸ鼭 ���� ���� �ƴҶ� ��� ��� ���
            else if (state != E_Enemy_State.Idle && !isFollow)
            {
                ani.SetTrigger("IdleTrigger");
            }
        
        }
    }
    #endregion

    #region ����
    public bool AttackTarget()
    {
        //�÷��̾�� ������ �Ÿ��� ���ݹ������� ������ ���� �ִϸ��̼� ���
        //���� ������ ���
        if (!isAttackDelay && Distance_A_B_Dis(Player.Instance.transform.position, transform.position, attackDis))
        {
            //���� �ִϸ��̼��� �ݺ� ȣ��Ǵ°� ���� ���� üũ
            //isAttack�� �ִϸ��̼��� �����Ҷ� true�� �Ǹ� �ִϸ��̼��� ������ false
            if (state != E_Enemy_State.Attack)
            {
                if (!isHitAnimation) { ani.SetTrigger("AttackTrigger"); }              
                StartCoroutine(AttackDelay());
            }
            //���� �ִϸ��̼� ���� �� �÷��̾ �ڿ������� �ٶ󺸰� ����    
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

    #region �ǰ� / ����
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Weapon"))
        {
            //�÷��̾� ���ݷ� ��ŭ ������ ����
            hp -= Player.Instance._Status.status.attackPower;

            //�÷��̾� ���������� ���� ���� ���
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

            //ü���� 0���� ���� �� ��� �ִϸ��̼� ���
            if (hp <= 0 )
            {
                if (!ani.GetBool("Death"))
                {
                    ani.SetBool("Death", true);
                    ani.SetTrigger("DeathTrigger");
                }
            }
            //ü���� ���������� �ǰ� �ִϸ��̼� ���
            else
            {
                Debug.Log("Hit2");
                if (!isDontChange && !isAttackAnimation) { ani.SetTrigger("HitTrigger"); }
            }
        }
    }
    //���� ����(�ൿ �Ұ�)
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

    #region ����
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
    
    //a�� ����(dis) ���� �ȿ� b�� ������ true, �ƴϸ� false
    public bool Distance_A_B_Dis(Vector3 a, Vector3 b,float dis)
    {
        if (GetDistance(a, b) <= Mathf.Pow(dis, 2))
            return true;

        return false;
    }

}
