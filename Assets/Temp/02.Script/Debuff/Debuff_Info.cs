using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Debuff")]
public class Debuff_Info : ScriptableObject
{
    [Header("[디버프 타입]")]
    public E_Debuff_Type type;
    [Header("[이름]")]
    public string _name;
    [Header("[아이콘]")]
    public Sprite icon;
    [Header("[스테이터스 감소]")]
    public float[] minusValue;     //스테이터스 감소
    [Header("[디버프 치유 조건]")]
    public float[] careValue;
    [Header("[디버프 발생 조건]")]
    public float[] conditionValue;
    [Header("[이동속도 감소]")]
    public float downMoveSpeed;     //이동속도 감소
}
