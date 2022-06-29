/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator ani;            //�ִϸ��̼� ����
    public PlayerControl control;   //�÷��̾� ��Ʈ�� ����
    public GameObject weaponHand;

    public bool hiting;             //�ǰݻ���


    public bool b_attack;           //<-- ���� ���ɿ���

    public bool aniPlaying;
    void Awake()
    {      
        ani = GetComponent<Animator>();
        //����� �� ����

        foreach (var i in ani.GetBehaviours<Player_AttackBehaviour>())
        {
            i.weapon = weaponHand;
        }
    }
    private void Start()
    {
        control = Player.Instance.Control;
        aniPlaying = false;
        hiting = false;
    }
    private void Update()
    {
        //�÷��̾ ����ִ� ������ ���� ���� ���
        if (E_UseTool.Knife == UIManager.Instance.equipPanel.PlayerTool) 
        {
            ani.SetBool("AttackType", true);
        }
        else
        {
            ani.SetBool("AttackType", false);
        }
        //�ǰ� ����/ü�� ��ȭ ����
        *//*Player.Instance._CStatus.*//*CalculationHp();
        //�̵�/�޸���
        PlayerMove();
        if (Input.GetMouseButtonDown(0))
            PlayerAttack();
        //��� �ִ� ����
        RandomIdle();
    }
    //�÷��̾� �̵� (�ȱ�, �ٱ�)
    public void PlayerMove()
    {
        //�÷��̾ �̵����϶�
        if (control.isMove)
        {
            //�ȱ� true
            ani.SetBool("IsWalk", true);
            //�ٱ⸦ ������������ false �۶��� true
            if (control.isAccel)
            {
                ani.SetBool("IsRun", true);
            }
            else if (!control.isAccel)
            {
                ani.SetBool("IsRun", false);
            }
        }
        else if (!control.isMove)
        {
            ani.SetBool("IsWalk", false);
        }
    }
    //�÷��̾� ����
    public void PlayerAttack()
    {
        //���� �Ұ� ���� ó��
        if (b_attack)
        {
            //�÷��̾ ��� �ִ� ������ ���� ���� ���
            if (Player.Instance._Status.state != E_Player_State.Attack)
            {
                Player.Instance._Status.state = E_Player_State.Attack;
                switch (UIManager.Instance.equipPanel.PlayerTool)
                {
                    case E_UseTool.None:

                        break;
                    case E_UseTool.Default:
                        SoundManager.Instance.PlaySound(E_SoundType.Effect, E_Effect_Player.Effect_Attack);
                        break;
                    case E_UseTool.Bottle:
                        break;
                    case E_UseTool.Axe:
                        break;
                    case E_UseTool.Shovel:
                        break;
                    case E_UseTool.Knife:
                        SoundManager.Instance.PlaySound(E_SoundType.Effect, E_Effect_Player.Effect_Knife_Attack);
                        break;
                    case E_UseTool.Pickaxe:
                        break;
                    case E_UseTool.Hammer:
                        break;
                    case E_UseTool.Max:
                        break;
                }
                //�ִϸ��̼� ���
                ani.SetTrigger("AttackTrigger");
            }
        }
        
    }
    public void WaterTrigger()
    {
        if (Player.Instance._Status.state == E_Player_State.Water)
        {
            ani.SetTrigger("WaterTrigger");
        }
    }
    
    public void SleepTrigger()
    {
        if (Player.Instance._Status.state == E_Player_State.Sleep)
        {
            ani.SetTrigger("SleepTrigger");
        }
    }

    //�÷��̾� �ǰ�
    public void CalculationHp()
    {
        Status s = Player.Instance._Status;
        //�ǰ�/ü�°���   
        if (s.beforeHp > s.Hp)
        {
            hiting = true;
            //�ǰ� ���� ���
            SoundManager.Instance.PlaySound(E_SoundType.Effect, E_Effect_Player.Effect_Hit);
            UIManager.Instance.equipPanel.ReductionArmor(); //�� ������ ����
            //���� ü���� 0�� �� ��� �ִϸ��̼� ���
            if (s.Hp <= 0)
            {
                ani.SetTrigger("DeathTrigger");
                Player.Instance.Control.SetAllowAll(false);
            }
            else
            {
                //���� ������ ���� ���� �ٸ� �ǰ� ��� ���
                if (s.overHit <= s.beforeHp - s.Hp)
                {
                    ani.SetTrigger("BigDamageTrigger");
                }
                else if (s.beforeHp - s.Hp > 1)
                {
                    ani.SetTrigger("DamageTrigger");
                }
            }
            //�ǰ� �� ü�� ����
            s.beforeHp = s.Hp;
        }
        else if (s.beforeHp < s.Hp)
        {
            Debug.Log("ȸ��");
            s.beforeHp = s.Hp;
        }
    }
    public void RandomIdle()
    {
        int idle = Random.Range(1, 4);
        ani.SetInteger("IdleState", idle);
    }
}
*/