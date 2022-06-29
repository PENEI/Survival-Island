using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Debuff")]
public class Debuff_Info : ScriptableObject
{
    [Header("[����� Ÿ��]")]
    public E_Debuff_Type type;
    [Header("[�̸�]")]
    public string _name;
    [Header("[������]")]
    public Sprite icon;
    [Header("[�������ͽ� ����]")]
    public float[] minusValue;     //�������ͽ� ����
    [Header("[����� ġ�� ����]")]
    public float[] careValue;
    [Header("[����� �߻� ����]")]
    public float[] conditionValue;
    [Header("[�̵��ӵ� ����]")]
    public float downMoveSpeed;     //�̵��ӵ� ����
}
