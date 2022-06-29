using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public PlayerStatus _Status;        // �������ͽ�
    public PlayerControl Control;       // ��Ʈ�ѷ�
    public PlayerDebuff _Debuff;        // �����
    public Player_Animation_Conroller _Animation;   // �ִϸ��̼� ��Ʈ�ѷ�

    protected override void SingletonInit()
    {
        base.SingletonInit();

        _Status = GetComponent<PlayerStatus>();
        Control = GetComponent<PlayerControl>();
        _Debuff = GetComponent<PlayerDebuff>();
        _Animation = GetComponentInChildren<Player_Animation_Conroller>();
    }
}
