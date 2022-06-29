using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebuff : MonoBehaviour
{
    
    [Header("�����")]
    public Debuff bodyache;
    public Debuff cold;
    public Debuff dehydration;
    public Debuff foodpoison;
    public Debuff stun;
    public Debuff swim;
    public Debuff wound;

    //����� Ȱ��ȭ ����
    [Header("[����� Ȱ��ȭ ����]")]
    public CISDebuff isDebuff;

    [Header("����� ����")]
    public Debuff_Info bodyacheInfo;    //����
    public Debuff_Info coldInfo;        //����
    public Debuff_Info dehydrationInfo; //Ż��
    public Debuff_Info foodpoisonInfo;  //���ߵ�
    public Debuff_Info stunInfo;        //����
    public Debuff_Info swimInfo;        //����
    public Debuff_Info woundInfo;       //�ܻ�(��ó)

    private WorldTime timer;
    private PlayerStatus player;

    private PlayerDebuff()
    {
        //Debug.Log("��!?");
    }

    private void Awake()
    {
        timer = World.Instance.worldTime;
        player = Player.Instance._Status;
    }

    private void Create_Condition(ref Debuff debuff, Debuff_Info info, bool isDebuff)
    {
        //����� Ȱ��ȭ ����
        if (debuff == null) 
        {
            //����� �߻� �ÿ��� ����
            if (isDebuff)
            {
                //������� �̸��� ���� ����� ����
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
        //���� ���� �ο�
        if (!isDebuff.isSwim && Player.Instance.transform.position.y <= swimInfo.conditionValue[0]) { isDebuff.isSwim = true; }

        // Ż�� ���� �߻�
        StatusDebuffActivation();
        // ���� ���� �߻�
        StatusDebuffActivation();
        // ���� ���� �߻�
        StatusDebuffActivation();

        Create_Condition(ref bodyache, bodyacheInfo, isDebuff.isBodyache);
        Create_Condition(ref cold, coldInfo, isDebuff.isCold);
        Create_Condition(ref dehydration, dehydrationInfo, isDebuff.isDehydration);
        Create_Condition(ref foodpoison, foodpoisonInfo, isDebuff.isFoodpoison);
        Create_Condition(ref stun, stunInfo, isDebuff.isStun);
        Create_Condition(ref swim, swimInfo, isDebuff.isSwim);
        Create_Condition(ref wound, woundInfo, isDebuff.isWound);
    }

    // ����� �߻�
    public void StatusDebuffActivation()
    {
        Status status = player.status;

        // ���� 
        if (!isDebuff.isDehydration)
        {
            // conditionValue[0]���Ϸ� �������� Ż�� ����� Ȱ��ȭ
            if (status.hydration.statusValue <= dehydrationInfo.conditionValue[0])
            {
                isDebuff.isDehydration = true;
            }
        }

        // ����
        if (!isDebuff.isCold)
        {
            // ���� ����� Ȱ��ȭ
            if (status.hp.statusValue <= coldInfo.conditionValue[0])
            {
                isDebuff.isCold = true;
            }
        }

        // ����
        if (!isDebuff.isBodyache)
        {
            // ���� ����� Ȱ��ȭ
            if (status.hp.statusValue <= bodyacheInfo.conditionValue[0] && 
                status.fatigue.statusValue <= bodyacheInfo.conditionValue[1]) 
            {
                isDebuff.isBodyache = true;
            }
        }
    }
}
