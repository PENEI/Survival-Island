using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuff : MonoBehaviour
{
    public Debuff_Info info;            //디버프 정보
    protected float maintenanceTime;    //경과 시간 
    protected PlayerStatus status;

    //private Status status;
    private PlayerDebuff debuff;

    void Start()
    {
        status = Player.Instance._Status;
        Debuff_Occurrence(true);
    }

    private void OnDestroy()
    {
        Debuff_Occurrence(false);
    }

    //자신을 삭제 조건들을 모아놓을 함수
    public abstract void Debuff_Destroy();

    //디버프 발생 시 일어나는 현상
    public void Debuff_Occurrence(bool tf)
    {
        switch(info.type)
        {
            case E_Debuff_Type.Bodyache:
                Debuff_Bodyache(tf);
                break;
            case E_Debuff_Type.Cold:
                Debuff_Cold(tf);
                break;
            case E_Debuff_Type.Dehydration:
                Debuff_Dehydration(tf);
                break;
            case E_Debuff_Type.Foodpoison:
                Debuff_Foodpoison(tf);
                break;
            case E_Debuff_Type.Stun:
                Debuff_Stun(tf);
                break;
            case E_Debuff_Type.Swim:
                Debuff_Swim(tf);
                break;
            case E_Debuff_Type.Wound:
                Debuff_Wound(tf);
                break;
        }
    }

    //디버프 경과 시간 반환
    public float MaintenanceTime()
    {
        return maintenanceTime += World.Instance.worldTime.cTime.multiply * Time.deltaTime;
    }


    //디버프가 비활성화 시 디버프 비활성화 체크
    private void Common_Debuff(ref bool isDebuff)
    {
        isDebuff = false;
    }

    #region 플레이어 이동속도 증감
    private void Player_DownMoveSpeed(bool tf)
    {
        Player.Instance.Control.SetMoveReduction(tf, tf ? -info.downMoveSpeed : info.downMoveSpeed);
    }
    #endregion

    #region 디버프

    #region 몸살디버프
    /* 제작 불가 */
    private void Debuff_Bodyache(bool tf)
    {
        Player.Instance.Control.isAllowMaking = tf ? false : true;
        
        if(!tf) Common_Debuff(ref Player.Instance._Debuff.isDebuff.isBodyache);
    }
    #endregion

    #region 감기 디버프
    /* 피로 소모 값 증가
    * 수분 소모 값 증가
    * 공복 소모 값 증가 */
    private void Debuff_Cold(bool tf)
    {
        status.Fatigue_Status_InDecrease(info.minusValue[0], tf);
        status.Hydration_Status_InDecrease(info.minusValue[1], tf);
        status.Hunger_Status_InDecrease(info.minusValue[2], tf);
        if (!tf) Common_Debuff(ref Player.Instance._Debuff.isDebuff.isCold);
    }
    #endregion

    #region 탈수 디버프
    /* 피로 소모 값 증가
    * 이동 속도 감소 */
    private void Debuff_Dehydration(bool tf)
    {
        status.Hydration_Status_InDecrease(info.minusValue[0], tf);
        Player_DownMoveSpeed(tf);
        if (!tf) Common_Debuff(ref Player.Instance._Debuff.isDebuff.isDehydration);
    }
    #endregion

    #region 식중독 디버프
    /* 아이템 사용 불가 */
    private void Debuff_Foodpoison(bool tf)
    {
        //아이템 사용 불가
        Player.Instance.Control.isAllowItem = tf ? false : true;
        if (!tf) Common_Debuff(ref Player.Instance._Debuff.isDebuff.isFoodpoison);
    }
    #endregion

    #region 기절 디버프
    /* 이동 불가 */
    private void Debuff_Stun(bool tf)
    {
        //이동 불가 (스턴이 너무 깜빡여서 스턴에서 이동 불가를 사용 못함) 
        //Player.Instance.Control.isAllowCharMove = tf ? false : true;
        if (!tf) Common_Debuff(ref Player.Instance._Debuff.isDebuff.isStun);
    }
    #endregion

    #region 외상 디버프
    /* 이동속도 감소 */
    private void Debuff_Wound(bool tf)
    {
        //디버프가 활성화 시 이속 감소
        //비활성화 시 이속 증가
        Player_DownMoveSpeed(tf);
        if (!tf) Common_Debuff(ref Player.Instance._Debuff.isDebuff.isWound);
    }
    #endregion

    #region 수영 디버프
     /* 아이템 사용 불가
     * 이동속도 감소 */
    private void Debuff_Swim(bool tf)
    {
        //아이템 사용 불가
        Player.Instance.Control.isAllowItem = tf ? false : true;
        //이동속도 감소
        Player.Instance.Control.SetMoveReduction(true, tf ? -info.downMoveSpeed : info.downMoveSpeed);
        if (!tf) Common_Debuff(ref Player.Instance._Debuff.isDebuff.isSwim);
    }
    #endregion

    #endregion
}
