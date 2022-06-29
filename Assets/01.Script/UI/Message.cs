using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [Header("�������� �ð�")]
    public float ShowDelay = 0.5f;      //�������� �ð�
    [Header("������� �ð�")]         
    public float HideDelay = 0.5f;       //������� �ð�
    
    Text MSText;     //�ؽ�Ʈ
    Image m_img;

    void Awake()
    {
        MSText = GetComponentInChildren<Text>();
        m_img = GetComponent<Image>();
        SetAlpha(0);
    }

    //�޼��� Ȱ��ȭ
    public void SetShow(string text)
    {
        MSText.text = text;
        
        //��Ÿ��
        m_img.DOFade(1, ShowDelay);
        MSText.DOFade(1, ShowDelay);
        //�����
        m_img.DOFade(0, ShowDelay).SetDelay(HideDelay + ShowDelay);
        MSText.DOFade(0, ShowDelay).SetDelay(HideDelay + ShowDelay);

    }

    //�̹���,�ؽ�Ʈ ���İ� ����
    void SetAlpha(float a)
    {
        Color tempColor = m_img.color;
        tempColor.a = a;
        m_img.color = tempColor;

        tempColor = MSText.color;
        tempColor.a = a;
        MSText.color = tempColor;

    }
}
