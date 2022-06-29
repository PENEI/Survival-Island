using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebuff : MonoBehaviour
{
    
    [Header("디버프")]
    public Debuff bodyache;
    public Debuff cold;
    public Debuff dehydration;
    public Debuff foodpoison;
    public Debuff stun;
    public Debuff swim;
    public Debuff wound;

    //디버프 활성화 여부
    [Header("[디버프 활성화 상태]")]
    public CISDebuff isDebuff;

    [Header("디버프 정보")]
    public Debuff_Info bodyacheInfo;    //몸살
    public Debuff_Info coldInfo;        //감기
    public Debuff_Info dehydrationInfo; //탈수
    public Debuff_Info foodpoisonInfo;  //식중독
    public Debuff_Info stunInfo;        //기절
    public Debuff_Info swimInfo;        //수영
    public Debuff_Info woundInfo;       //외상(상처)

    private WorldTime timer;
    private PlayerStatus player;

    private PlayerDebuff()
    {
        //Debug.Log("어!?");
    }

    private void Awake()
    {
        timer = World.Instance.worldTime;
        player = Player.Instance._Status;
    }

    private void Create_Condition(ref Debuff debuff, Debuff_Info info, bool isDebuff)
    {
        //디버프 활성화 여부
        if (debuff == null) 
        {
            //디버프 발생 시에만 실행
            if (isDebuff)
            {
                //디버프의 이름에 따라 디버프 생성
                switch(info.type)
                {
                    case E_Debuff_Type.Bodyache:
                        debuff = gameObject.AddComponent<Bodyache>();
                        debuff.info = bodyacheInfo;
                        break;
                    case E_Debuff_Type.Cold:
                        debuff = gameObject.AddComponent<Cold>();
                        debuff.info = coldInfo;
                        break;
                    case E_Debuff_Type.Dehydration:
                        debuff = gameObject.AddComponent<Dehydration>();
                        debuff.info = dehydrationInfo;
                        break;
                    case E_Debuff_Type.Foodpoison:
                        debuff = gameObject.AddComponent<Foodpoison>();
                        debuff.info = foodpoisonInfo;
                        break;
                    case E_Debuff_Type.Stun:
                        debuff = gameObject.AddComponent<Stun>();
                        debuff.info = stunInfo;
                        break;
                    case E_Debuff_Type.Swim:
                        debuff = gameObject.AddComponent<Swim>();
                        debuff.info = swimInfo;
                        break;
                    case E_Debuff_Type.Wound:
                        debuff = gameObject.AddComponent<Wound>();
                        debuff.info = woundInfo;
                        break;
                }
            }
        }
    }
    private void Update()
    {
        //수영 상태 부여
        if (!isDebuff.isSwim && Player.Instance.transform.position.y <= swimInfo.conditionValue[0]) { isDebuff.isSwim = true; }

        // 탈수 증상 발생
        StatusDebuffActivation();
        // 감기 증상 발생
        StatusDebuffActivation();
        // 몸살 증상 발생
        StatusDebuffActivation();

        Create_Condition(ref bodyache, bodyacheInfo, isDebuff.isBodyache);
        Create_Condition(ref cold, coldInfo, isDebuff.isCold);
        Create_Condition(ref dehydration, dehydrationInfo, isDebuff.isDehydration);
        Create_Condition(ref foodpoison, foodpoisonInfo, isDebuff.isFoodpoison);
        Create_Condition(ref stun, stunInfo, isDebuff.isStun);
        Create_Condition(ref swim, swimInfo, isDebuff.isSwim);
        Create_Condition(ref wound, woundInfo, isDebuff.isWound);
    }

    // 디버프 발생
    public void StatusDebuffActivation()
    {
        Status status = player.status;

        // 수분 
        if (!isDebuff.isDehydration)
        {
            // conditionValue[0]이하로 떨어지면 탈수 디버프 활성화
            if (status.hydration.statusValue <= dehydrationInfo.conditionValue[0])
            {
                isDebuff.isDehydration = true;
            }
        }

        // 감기
        if (!isDebuff.isCold)
        {
            // 감기 디버프 활성화
            if (status.hp.statusValue <= coldInfo.conditionValue[0])
            {
                isDebuff.isCold = true;
            }
        }

        // 몸살
        if (!isDebuff.isBodyache)
        {
            // 몸살 디버프 활성화
            if (status.hp.statusValue <= bodyacheInfo.conditionValue[0] && 
                status.fatigue.statusValue <= bodyacheInfo.conditionValue[1]) 
            {
                isDebuff.isBodyache = true;
            }
        }
    }
}
