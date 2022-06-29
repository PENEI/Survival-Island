using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIPlayerStatusBar : MonoBehaviour
{
    private PlayerStatus status;     //player스테이터스
    public Image[] statusBarImage;  //스테이터스 바의 이미지 정보


    void Awake()
    {
        status = Player.Instance._Status;
        ReSizeStatusBarImage();
    } 
    
    void Update()
    {
        //player의 스테이터스바 그리기 
        PlayerAllStatusBar(statusBarImage);
    }
    
    //배열 재설정 및 스테이터스바 이미지 정보 갖고오기
    public void ReSizeStatusBarImage()
    {

        statusBarImage = transform.GetComponentsInChildren<Image>();
        Array.Resize<Image>(ref statusBarImage, transform.childCount);
        //스테이터스바 중 현재 스테이터스를 표시하는 바의 이미지 정보만을 가져옴
        for (int i = 0; i < transform.childCount; i++)
        {
            statusBarImage[i] = transform.GetChild(i).transform.GetChild(1).GetComponent<Image>();
        }
    }

  

    //player의 모든 스테이터스바 그리기
    public void PlayerAllStatusBar(Image[] image)
    {
        //스테이터스바 갱신
        image[0].fillAmount = Status.PlayerStatusPercentage(status.status.hp);
        image[1].fillAmount = Status.PlayerStatusPercentage(status.status.hunger);
        image[2].fillAmount = Status.PlayerStatusPercentage(status.status.hydration);
        image[3].fillAmount = Status.PlayerStatusPercentage(status.status.fatigue);
    }
}
