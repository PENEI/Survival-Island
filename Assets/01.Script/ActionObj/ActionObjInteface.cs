using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ȣ�ۿ�
public interface ActionObj
{
    //��ȣ�ۿ� �������� Ȯ��
    public bool IsAction();

    //��ȣ�ۿ�
    void Action();

    //��ȣ�ۿ� Ÿ�� ���
    E_InteractionType GetInteractionType();

}
