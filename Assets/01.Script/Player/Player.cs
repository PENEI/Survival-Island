using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public PlayerStatus _Status;        // 스테이터스
    public PlayerControl Control;       // 컨트롤러
    public PlayerDebuff _Debuff;        // 디버프
    public Player_Animation_Conroller _Animation;   // 애니메이션 컨트롤러

    protected override void SingletonInit()
    {
        base.SingletonInit();

        _Status = GetComponent<PlayerStatus>();
        Control = GetComponent<PlayerControl>();
        _Debuff = GetComponent<PlayerDebuff>();
        _Animation = GetComponentInChildren<Player_Animation_Conroller>();
    }
}
