/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CStatus : MonoBehaviour
{
    [Header("[MAX 스테이터스]")]
    public float maxHydration;
    public float maxHunger;
    public float maxFatigue;
    public float maxHp;

    [Header("[스테이터스]")]
    public float Hydration;     //수분
    public float Hunger;        //공복
    public float Fatigue;       //피로
    public float Hp;            //체력
    public float beforeHp;      //감소전의 체력


    [Header("[스테이터스 감소 주기]")]
    public int HydrationTime;    //수분 감소 주기
    public int HungerTime;       //공복 감소 주기
    public int FatigueTime;      //피로 감소 주기
    public int HpTime;           //체력 감소 주기

    [Header("[스테이터스 감소 값]")]
    public float mHydration;    //수분 감소 치
    public float mHunger;       //공복 감소 치
    public float mFatigue;      //피로 감소 치
    public float mHp;           //체력 감소 치

    private bool hiting;             //피격상태



    //플레이어 피격
    public void CalculationHp()
    {
        Status s = Player.Instance._Status;
        PlayerAnimationController _ani = Player.Instance._animation;

        //피격/체력감소   
        if (s.beforeHp > s.Hp)
        {
            hiting = true;
            SoundManager.Instance.PlaySound(E_SoundType.Effect, E_Effect_Player.Effect_Hit);
            UIManager.Instance.equipPanel.ReductionArmor(); //방어구 내구도 감소
            if (s.Hp <= 0)
            {

                _ani.ani.SetTrigger("DeathTrigger");
                Player.Instance.Control.SetAllowAll(false);
            }
            else
            {
                if (s.overHit <= s.beforeHp - s.Hp)
                {
                    _ani.ani.SetTrigger("BigDamageTrigger");
                }
                else if (s.beforeHp - s.Hp > 1)
                {
                    _ani.ani.SetTrigger("DamageTrigger");
                }
            }
            s.beforeHp = s.Hp;
        }
        else if (s.beforeHp < s.Hp)
        {
            Debug.Log("회복");
            s.beforeHp = s.Hp;
        }
    }
}
*/