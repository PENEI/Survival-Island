/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class PlayerStatusBar : MonoBehaviour
{
    //statusBar������Ʈ�� Image����
    public Image[] statusBar;

    private void Awake()
    {
        //statusBar������Ʈ ������ŭ statusBar�� ũ�⸦ �����ϰ� �ڽİ�ü���� ����.
        Array.Resize(ref statusBar, transform.childCount);
        for (int i = 0; i < transform.childCount; i++) 
        {
            statusBar[i] = transform.GetChild(i).transform.GetChild(1).GetComponent<Image>();
        }
    }
    private void Update()
    {
        statusBar[0].fillAmount = Player.Instance._Status.Hp / Player.Instance._Status.maxHp;
        statusBar[1].fillAmount = Player.Instance._Status.Hunger / Player.Instance._Status.maxHunger;
        statusBar[2].fillAmount = Player.Instance._Status.Hydration / Player.Instance._Status.maxHydration;
        statusBar[3].fillAmount = Player.Instance._Status.Fatigue / Player.Instance._Status.maxFatigue;
    }
}
*/