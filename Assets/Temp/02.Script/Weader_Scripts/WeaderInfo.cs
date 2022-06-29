using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Weader")]

public class WeaderInfo : ScriptableObject
{
    public string _name;                //재해 이름
    public Sprite icon;                 //재해 아이콘
    public float value;                    //스테이터스, 이동속도 등 변화 값
    public int closeCost;                   //폐쇠 카운트
    public Debuff_Info[] debuff;         //재해 발생 시 생기는 디버프
    public Texture2D fxFilter;          //카메라 필터

    public int startTimeMin = 8;        //재해 유지 시간 최소 값
    public int startTimeMax = 16;       //재해 유지 시간 최대 값

    public AudioClip audio;     //재해 소리

    [HideInInspector]
    public int maintainWeader;          //재해 유지 시간
}
