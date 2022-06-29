using UnityEngine;
using System.Collections;
//날씨
public class Weader : MonoBehaviour
{
    protected WorldTime timer;
    protected PlayerStatus status;     //플레이어 스테이터스
    public WeaderInfo weaderInfo;   //날씨가 적용하는 값들

    public int starth;
    public int startm;
    public int endh;
    public int endm;

    public bool distroyOn;          //재해 시작 여부

    [HideInInspector]
    public AudioSource audio;
    

    //Weader클래스 초기화
    public void Weader_Awake()
    {
        timer = GetComponent<WorldTime>();
        status = Player.Instance._Status;
        audio = GameObject.Find("Canvas").GetComponent<AudioSource>();
        
    }
    
    //Weader 클래스 삭제 시 실행
    public void Weader_Deastroy()
    {
        audio.clip = null;
    }

    //스테이터스 정기 감소 값 감소
    public void VariationValueStatus(SingleStatus status, float value)
    {
        status.minusValue[1] += value;
    }

    //디버프 시작 및 끝낼건지의 여부
    public bool StartDebuff()
    {
        //현재 시간이 재해 시작 시간보다 크거나 재해
        //끝나는 시간보다 작을 시 true
        //현재 시간이 재해 시작 시간(h,m)보다 크거나,
        //현재 시간이 재해 끝나는 시간(h,m) 보다 작을 시 true 
        if ((starth < (int)timer.GetTime("시") && 
            endh > (int)timer.GetTime("시")) ||
            ((starth == (int)timer.GetTime("시") && 
            startm <= (int)timer.GetTime("분")))||
            (endh == (int)timer.GetTime("시") && 
            endm >= (int)timer.GetTime("분")))
        {
            return true;
        }
        return false;
    }

    //날씨 효과 적용
    public void WeaderApplication(bool tf, SingleStatus status = null)
    {
        //섬 침몰
        if (weaderInfo.closeCost != 0)
        {
            World.Instance.islandManager.cost += weaderInfo.closeCost;
        }

        //스테이터스 감소 값
        if (weaderInfo.value != 0)
        {
            //활성화 시 value만큼 스테이터스 감소 값 증가 아닐 시 감소
            VariationValueStatus(status, (tf ? weaderInfo.value : (-weaderInfo.value)));
        }

        //카메라 필터
        if (weaderInfo.fxFilter != null)
        {
            Camera.main.GetComponent<AmplifyColorBase>().LutTexture = tf ? weaderInfo.fxFilter : null;
        }

        // 날씨 소리 적용
        if (tf && weaderInfo.audio != null)
        {
            audio.clip = weaderInfo.audio;
            
        }
        else if (!tf || weaderInfo.audio == null)
        {
            audio.clip = null;
        }

        //디버프
        if (tf && weaderInfo.debuff != null) 
        {
            //모든 디버프 적용
            for (int i = 0; i < weaderInfo.debuff.Length; i++)
            {
                switch(weaderInfo.debuff[i].type)
                {
                    case E_Debuff_Type.Bodyache:
                        Player.Instance._Debuff.isDebuff.isBodyache = true;
                        break;
                    case E_Debuff_Type.Cold:
                        Player.Instance._Debuff.isDebuff.isCold = true;
                        break;
                    case E_Debuff_Type.Dehydration:
                        Player.Instance._Debuff.isDebuff.isDehydration = true;
                        break;
                    case E_Debuff_Type.Foodpoison:
                        Player.Instance._Debuff.isDebuff.isFoodpoison = true;
                        break;
                    case E_Debuff_Type.Swim:
                        Player.Instance._Debuff.isDebuff.isSwim = true;
                        break;
                    case E_Debuff_Type.Wound:
                        Player.Instance._Debuff.isDebuff.isWound = true;
                        break;
                }
            }
        }
    }
}
