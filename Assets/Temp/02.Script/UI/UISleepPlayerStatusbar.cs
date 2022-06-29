using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UISleepPlayerStatusbar : MonoBehaviour
{
    [HideInInspector]
    public PlayerStatus playerStatus;
    [Header("[���� �ð�]")]
    public WorldTime timer;
    [Header("[UI Status ����]")]
    public UIPlayerStatusBar statusbarImage;//���� �������ͽ�
    [Header("[�⺻ �������ͽ� �̹���]")]
    public Image[] statusImage;             //���� �������׽�        ü��, ����, ����, �Ƿ�
    [Header("[ȸ��/�Ҹ��� �������ͽ� �̹���]")]
    public Image[] changeStatusImage;     //ȸ���ϴ� �������ͽ�    ü��, ����, ����, �Ƿ� || ü��,�Ƿ� = ȸ�� / ����, ���� = �Ҹ�

    public void Awake()
    {
        playerStatus = Player.Instance._Status;
    }
    private void OnEnable()
    {
        Player.Instance.Control.SetAllowAll(false);
        for (int i = 0; i < statusImage.Length; i++)
        {
            changeStatusImage[i].fillAmount = 0f;
        }
        statusImage[0].fillAmount= PlayerStatusBar(playerStatus.status.hp);
        statusImage[1].fillAmount = PlayerStatusBar(playerStatus.status.hunger);
        statusImage[2].fillAmount = PlayerStatusBar(playerStatus.status.hydration);
        statusImage[3].fillAmount = PlayerStatusBar(playerStatus.status.fatigue);
    }
    private void Update()
    {
        
    }
    private void OnDisable()
    {
        Player.Instance.Control.SetAllowAll(true);
    }

    //player�� �������ͽ��� ���� �������ͽ�UI �̹����� ��ȭ
    public float PlayerStatusBar(SingleStatus status)
    {
        return status.statusValue / status.maxStatus;
    }
}
