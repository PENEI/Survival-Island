using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuff : MonoBehaviour
{
    public Debuff_Info info;            //����� ����
    protected float maintenanceTime;    //��� �ð� 
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

    //�ڽ��� ���� ���ǵ��� ��Ƴ��� �Լ�
    public abstract void Debuff_Destroy();

    //����� �߻� �� �Ͼ�� ����
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

    //����� ��� �ð� ��ȯ
    public float MaintenanceTime()
    {
        return maintenanceTime += World.Instance.worldTime.cTime.multiply * Time.deltaTime;
    }


    //������� ��Ȱ��ȭ �� ����� ��Ȱ��ȭ üũ
    private void Common_Debuff(ref bool isDebuff)
    {
        isDebuff = false;
    }

    #region �÷��̾� �̵��ӵ� ����
    private void Player_DownMoveSpeed(bool tf)
    {
        Player.Instance.Control.SetMoveReduction(tf, tf ? -info.downMoveSpeed : info.downMoveSpeed);
    }
    #endregion

    #region �����

    #region ��������
    /* ���� �Ұ� */
    private void Debuff_Bodyache(bool tf)
    {
        Player.Instance.Control.isAllowMaking = tf ? false : true;
        
        if(!tf) Common_Debuff(ref Player.Instance._Debuff.isDebuff.isBodyache);
    }
    #endregion

    #region ���� �����
    /* �Ƿ� �Ҹ� �� ����
    * ���� �Ҹ� �� ����
    * ���� �Ҹ� �� ���� */
    private void Debuff_Cold(bool tf)
    {
        status.Fatigue_Status_InDecrease(info.minusValue[0], tf);
        status.Hydration_Status_InDecrease(info.minusValue[1], tf);
        status.Hunger_Status_InDecrease(info.minusValue[2], tf);
        if (!tf) Common_Debuff(ref Player.Instance._Debuff.isDebuff.isCold);
    }
    #endregion

    #region Ż�� �����
    /* �Ƿ� �Ҹ� �� ����
    * �̵� �ӵ� ���� */
    private void Debuff_Dehydration(bool tf)
    {
        status.Hydration_Status_InDecrease(info.minusValue[0], tf);
        Player_DownMoveSpeed(tf);
        if (!tf) Common_Debuff(ref Player.Instance._Debuff.isDebuff.isDehydration);
    }
    #endregion

    #region ���ߵ� �����
    /* ������ ��� �Ұ� */
    private void Debuff_Foodpoison(bool tf)
    {
        //������ ��� �Ұ�
        Player.Instance.Control.isAllowItem = tf ? false : true;
        if (!tf) Common_Debuff(ref Player.Instance._Debuff.isDebuff.isFoodpoison);
    }
    #endregion

    #region ���� �����
    /* �̵� �Ұ� */
    private void Debuff_Stun(bool tf)
    {
        //�̵� �Ұ� (������ �ʹ� �������� ���Ͽ��� �̵� �Ұ��� ��� ����) 
        //Player.Instance.Control.isAllowCharMove = tf ? false : true;
        if (!tf) Common_Debuff(ref Player.Instance._Debuff.isDebuff.isStun);
    }
    #endregion

    #region �ܻ� �����
    /* �̵��ӵ� ���� */
    private void Debuff_Wound(bool tf)
    {
        //������� Ȱ��ȭ �� �̼� ����
        //��Ȱ��ȭ �� �̼� ����
        Player_DownMoveSpeed(tf);
        if (!tf) Common_Debuff(ref Player.Instance._Debuff.isDebuff.isWound);
    }
    #endregion

    #region ���� �����
     /* ������ ��� �Ұ�
     * �̵��ӵ� ���� */
    private void Debuff_Swim(bool tf)
    {
        //������ ��� �Ұ�
        Player.Instance.Control.isAllowItem = tf ? false : true;
        //�̵��ӵ� ����
        Player.Instance.Control.SetMoveReduction(true, tf ? -info.downMoveSpeed : info.downMoveSpeed);
        if (!tf) Common_Debuff(ref Player.Instance._Debuff.isDebuff.isSwim);
    }
    #endregion

    #endregion
}
