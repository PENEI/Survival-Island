using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����г�
public class SlotPanel : MonoBehaviour
{
    public List<Slot> SlotList;     //���� ����Ʈ

    void Awake()
    {
        //�ڽ� ���� ������
        SlotList.AddRange(this.gameObject.GetComponentsInChildren<Slot>());
    }

}
