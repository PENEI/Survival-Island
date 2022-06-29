/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator ani;            //애니메이션 정보
    public PlayerControl control;   //플레이어 컨트롤 정보
    public GameObject weaponHand;

    public bool hiting;             //피격상태


    public bool b_attack;           //<-- 공격 가능여부

    public bool aniPlaying;
    void Awake()
    {      
        ani = GetComponent<Animator>();
        //무기든 손 저장

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
        //플레이어가 들고있는 도구에 따른 공격 모션
        if (E_UseTool.Knife == UIManager.Instance.equipPanel.PlayerTool) 
        {
            ani.SetBool("AttackType", true);
        }
        else
        {
            ani.SetBool("AttackType", false);
        }
        //피격 판정/체력 변화 설정
        *//*Player.Instance._CStatus.*//*CalculationHp();
        //이동/달리기
        PlayerMove();
        if (Input.GetMouseButtonDown(0))
            PlayerAttack();
        //대기 애니 실행
        RandomIdle();
    }
    //플레이어 이동 (걷기, 뛰기)
    public void PlayerMove()
    {
        //플레이어가 이동중일때
        if (control.isMove)
        {
            //걷기 true
            ani.SetBool("IsWalk", true);
            //뛰기를 하지않을때는 false 뛸때는 true
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
    //플레이어 공격
    public void PlayerAttack()
    {
        //공격 불가 예외 처리
        if (b_attack)
        {
            //플레이어가 들고 있는 도구에 따른 사운드 출력
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
                //애니메이션 출력
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

    //플레이어 피격
    public void CalculationHp()
    {
        Status s = Player.Instance._Status;
        //피격/체력감소   
        if (s.beforeHp > s.Hp)
        {
            hiting = true;
            //피격 사운드 출력
            SoundManager.Instance.PlaySound(E_SoundType.Effect, E_Effect_Player.Effect_Hit);
            UIManager.Instance.equipPanel.ReductionArmor(); //방어구 내구도 감소
            //남은 체력이 0일 시 사망 애니메이션 출력
            if (s.Hp <= 0)
            {
                ani.SetTrigger("DeathTrigger");
                Player.Instance.Control.SetAllowAll(false);
            }
            else
            {
                //받은 데미지 량에 따른 다른 피격 모션 출력
                if (s.overHit <= s.beforeHp - s.Hp)
                {
                    ani.SetTrigger("BigDamageTrigger");
                }
                else if (s.beforeHp - s.Hp > 1)
                {
                    ani.SetTrigger("DamageTrigger");
                }
            }
            //피격 후 체력 저장
            s.beforeHp = s.Hp;
        }
        else if (s.beforeHp < s.Hp)
        {
            Debug.Log("회복");
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