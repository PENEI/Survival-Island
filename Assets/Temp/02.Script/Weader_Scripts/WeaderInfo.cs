using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Weader")]

public class WeaderInfo : ScriptableObject
{
    public string _name;                //���� �̸�
    public Sprite icon;                 //���� ������
    public float value;                    //�������ͽ�, �̵��ӵ� �� ��ȭ ��
    public int closeCost;                   //��� ī��Ʈ
    public Debuff_Info[] debuff;         //���� �߻� �� ����� �����
    public Texture2D fxFilter;          //ī�޶� ����

    public int startTimeMin = 8;        //���� ���� �ð� �ּ� ��
    public int startTimeMax = 16;       //���� ���� �ð� �ִ� ��

    public AudioClip audio;     //���� �Ҹ�

    [HideInInspector]
    public int maintainWeader;          //���� ���� �ð�
}
