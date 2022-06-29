using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingObj : MonoBehaviour, ActionObj
{
    [Header("��ȣ�ۿ� ���")]
    public bool isAllowAction;

    void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }

    //��������
    public void Action()
    {
        //�ƽ�����
        Player.Instance.Control.SetAllowAll(false);
        SceneLoader.Instance.LoadGameScene("Credit", false);
    }

    public E_InteractionType GetInteractionType()
    {
        return E_InteractionType.Ending;
    }

    public bool IsAction()
    {
        //��ȣ�ۿ� �������� Ȯ��
        if (isAllowAction)
            return true;

        return false;
    }

}
