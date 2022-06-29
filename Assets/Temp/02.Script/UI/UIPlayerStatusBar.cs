using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIPlayerStatusBar : MonoBehaviour
{
    private PlayerStatus status;     //player�������ͽ�
    public Image[] statusBarImage;  //�������ͽ� ���� �̹��� ����


    void Awake()
    {
        status = Player.Instance._Status;
        ReSizeStatusBarImage();
    } 
    
    void Update()
    {
        //player�� �������ͽ��� �׸��� 
        PlayerAllStatusBar(statusBarImage);
    }
    
    //�迭 �缳�� �� �������ͽ��� �̹��� ���� �������
    public void ReSizeStatusBarImage()
    {

        statusBarImage = transform.GetComponentsInChildren<Image>();
        Array.Resize<Image>(ref statusBarImage, transform.childCount);
        //�������ͽ��� �� ���� �������ͽ��� ǥ���ϴ� ���� �̹��� �������� ������
        for (int i = 0; i < transform.childCount; i++)
        {
            statusBarImage[i] = transform.GetChild(i).transform.GetChild(1).GetComponent<Image>();
        }
    }

  

    //player�� ��� �������ͽ��� �׸���
    public void PlayerAllStatusBar(Image[] image)
    {
        //�������ͽ��� ����
        image[0].fillAmount = Status.PlayerStatusPercentage(status.status.hp);
        image[1].fillAmount = Status.PlayerStatusPercentage(status.status.hunger);
        image[2].fillAmount = Status.PlayerStatusPercentage(status.status.hydration);
        image[3].fillAmount = Status.PlayerStatusPercentage(status.status.fatigue);
    }
}
