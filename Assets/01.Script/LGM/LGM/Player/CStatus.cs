/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CStatus : MonoBehaviour
{
    [Header("[MAX �������ͽ�]")]
    public float maxHydration;
    public float maxHunger;
    public float maxFatigue;
    public float maxHp;

    [Header("[�������ͽ�]")]
    public float Hydration;     //����
    public float Hunger;        //����
    public float Fatigue;       //�Ƿ�
    public float Hp;            //ü��
    public float beforeHp;      //�������� ü��


    [Header("[�������ͽ� ���� �ֱ�]")]
    public int HydrationTime;    //���� ���� �ֱ�
    public int HungerTime;       //���� ���� �ֱ�
    public int FatigueTime;      //�Ƿ� ���� �ֱ�
    public int HpTime;           //ü�� ���� �ֱ�

    [Header("[�������ͽ� ���� ��]")]
    public float mHydration;    //���� ���� ġ
    public float mHunger;       //���� ���� ġ
    public float mFatigue;      //�Ƿ� ���� ġ
    public float mHp;           //ü�� ���� ġ

    private bool hiting;             //�ǰݻ���



    //�÷��̾� �ǰ�
    public void CalculationHp()
    {
        Status s = Player.Instance._Status;
        PlayerAnimationController _ani = Player.Instance._animation;

        //�ǰ�/ü�°���   
        if (s.beforeHp > s.Hp)
        {
            hiting = true;
            SoundManager.Instance.PlaySound(E_SoundType.Effect, E_Effect_Player.Effect_Hit);
            UIManager.Instance.equipPanel.ReductionArmor(); //�� ������ ����
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
            Debug.Log("ȸ��");
            s.beforeHp = s.Hp;
        }
    }
}
*/