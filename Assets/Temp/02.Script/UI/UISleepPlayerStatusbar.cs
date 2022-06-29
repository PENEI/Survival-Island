using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UISleepPlayerStatusbar : MonoBehaviour
{
    [HideInInspector]
    public PlayerStatus playerStatus;
    [Header("[월드 시간]")]
    public WorldTime timer;
    [Header("[UI Status 정보]")]
    public UIPlayerStatusBar statusbarImage;//현재 스테이터스
    [Header("[기본 스테이터스 이미지]")]
    public Image[] statusImage;             //현재 스테이테스        체력, 공복, 수분, 피로
    [Header("[회복/소모할 스테이터스 이미지]")]
    public Image[] changeStatusImage;     //회복하는 스테이터스    체력, 공복, 수분, 피로 || 체력,피로 = 회복 / 수분, 공복 = 소모

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

    //player의 스테이터스에 따라 스테이터스UI 이미지의 변화
    public float PlayerStatusBar(SingleStatus status)
    {
        return status.statusValue / status.maxStatus;
    }
}
