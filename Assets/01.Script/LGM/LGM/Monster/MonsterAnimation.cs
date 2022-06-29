using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    //�÷��̾� �߰��� �׸��θ� �̵��� �ϸ鼭 �ִϸ��̼� ����� ���
    //�����̵��� �������� 2�� ��½� �ִϸ��̼� ���� ����
    public MonsterInfo2 info;
    public Animator ani;

    public float attackDelay;   //���� ������
    private bool attacking;      //���� ������ üũ

    private void Awake()
    {
        // ������ ����ִ� ���
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
        //�̵��� ���� �ִϸ��̼� ��ȭ
        MonsterMoveSet();
        if (!info.isDeath)
        {
            //������ ����
            HitDamage();
        }

        //������ �ð��� ���� �����Ҹ� ����
        info.MonsterCrying();

        if (!info.type)
        {
            //type�� true�� ������
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

    
    //�ǰ�/ü�°���   
    public void HitDamage()
    {
        if (info.beforeHp != info.hp)
        {
            //�÷��̾� ���������� ���� �ǰ���
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
            if (info.hp <= 0)
            {
                info.isDeath = true;
                ani.SetTrigger("DeathTrigger");
            }
            //ü���� ���������� �ǰ� �ִϸ��̼� ���
            else
            {
                ani.SetTrigger("HitTrigger");
                if (info.type)
                {
                    info.isSerching2 = true;
                }
            }
            //���ҵ� ü�� ����
            info.beforeHp = info.hp;
        }
    }

    public void Type1()
    {
        if (!info.isDeath)
        {
            //�÷��̾ Ž�� ���� �ȿ� ������
            if (!(!info.searching && !info.isComback &&
                GetDistance(Player.Instance.transform.position, transform.position) > Mathf.Pow(info.searchDis, 2)))
            {
                /*
                 * ���� �������� ���� ������ �̵��� ���߰� ����
                 * ���� �������� ���� ������ �߰�
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
    
    //����
    public void Comeback()
    {
        //�÷��̾ ������ Ž�������� �����ٰ� ����� ���� ����
        if (info.searching && 
            GetDistance(transform.position, info.centerPos) > Mathf.Pow(info.centerDis, 2)) 
        {
            ani.SetInteger("State", 4);                 //�߰� �ִϸ��̼� ����
            info.agent.SetDestination(info.centerPos);  //�߰� ���� ��ġ�� ���ư�
            info.isComback = true;
        }

        //������ ���� �� ����
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

    //�߰�
    public void SearchTarget()
    {
        //[Ž���������� �÷��̾ ���� �� �߰�]
        //�÷��̾�� ���� ������ �Ÿ��� Ž�� �������� ������ ����
        if (GetDistance(Player.Instance.transform.position, transform.position) 
            <= Mathf.Pow(info.searchDis, 2))
        {
            if (!info.searching)
            {
                info.searching = true;                  //�߰� �� üũ
                ani.SetInteger("State", 4);             //�߰� �ִϸ��̼� ����
                info.centerPos = transform.position;    //�߰� ���� ��ġ ���� 
            }
            if(!info. isComback)
                info.agent.SetDestination(Player.Instance.transform.position);
        }
    }


    //����
    public void AttackTarget()
    {
        //�÷��̾�� ������ �Ÿ��� ���ݹ������� ������ ���� �ִϸ��̼� ���
        if (GetDistance(Player.Instance.transform.position, transform.position) <= Mathf.Pow(info.attackDis, 2))
        {
            //���� �ִϸ��̼��� �ݺ� ȣ��Ǵ°� ���� ���� üũ
            //isAttack�� �ִϸ��̼��� �����Ҷ� true�� �Ǹ� �ִϸ��̼��� ������ false
            if (!info.isAttack)
            {
                ani.SetTrigger("AttackTrigger");
                
            }
            //���� �ִϸ��̼� ���� �� �÷��̾ �ڿ������� �ٶ󺸰� ����    
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

    //���� ����
    //*****
    /*public void AttackDamage()
    {
        //�ִϸ��̼��� ���۵ɶ� ���� �Ҽ� �ֵ��� false�� �ʱ�ȭ
        if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.2f)
        {
            info.damageDelay = false;
        }
        //�ִϸ��̼��� �߰� �κ�
        if (!info.damageDelay && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
        {
            if (info.attackPower >= Player.Instance._Status.overHit)
            {
                Debug.Log((Player.Instance.transform.position - transform.position).normalized);
                Player.Instance.GetComponent<Rigidbody>().
                    AddForce((Player.Instance.transform.position - transform.position).normalized
                    * Player.Instance._Status.force, ForceMode.Impulse);
            }
            //�÷��̾� ü�� ����
            Player.Instance._Status.Hp -= info.attackPower;
            //���� �Ŀ� �ִϸ��̼� �ݺ� ���� �ʵ��� trueüũ
            info.damageDelay = true;
        }
    }*/

    public void RandomMotion()
    {
        info.randAni.motion = Random.Range(1, 5);
        ani.SetInteger("State", info.randAni.motion);
    }

    //���� �̵� ��ǥ
    public Vector3 RandomPos()
    {
        int x;          //���� �̵� ��ǥ X
        int z;          //���� �̵� ��ǥ Y
        int reverseX;   //���� �̵� ��ǥ ���� ���� X��
        int reverseZ;   //���� �̵� ��ǥ ���� ���� Y��
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
    public float GetDistance(Vector3 a, Vector3 b)
    {
        //�÷��̾���� �Ÿ��� ���� �� return
        return (a - b).sqrMagnitude;
    }
}
