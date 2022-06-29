using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakingUIObj : MonoBehaviour, ActionObj
{
    public E_MakingType MakingType;

    [Header("��ȣ�ۿ� ���")]
    public bool isAllowAction;      //���� ������ ���
    
    [SerializeField]
    MakingPanel m_MakingUI;     //���� ������

    ObjectPrefab m_ObjectPrefab;
    AudioSource m_Audio;

    bool isInteraction;         //��ȣ�ۿ���
    bool isComplete;            //��ȣ�ۿ� ��Ȯ��
    bool isMaking;                  //������

    IEnumerator enumerator;
    void Awake()
    {
        //���� ������ ������ Ÿ�Կ� ���缭 ������
        m_MakingUI = UIManager.Instance.makingPanelList.PanelList.Find(x => x.MakingType == MakingType);
        m_ObjectPrefab = GetComponent<ObjectPrefab>();
        m_Audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        //������Ʈ ���� ���
        if (m_Audio != null)
        {
            m_Audio.loop = true;
            ObjectData.Info info = CSVManager.Instance.GetObjectInfo(m_ObjectPrefab.ID);
            SoundManager.Instance.PlaySound(m_Audio, info.Sound);
        }
    }

    //************
    private void Update()
    {
        //��ȣ�ۿ������� Ȯ��
        if (isInteraction)
        {
            //�÷��̾� �����̸� ��ȣ�ۿ�Ϸ� �Ұ���
            if (Player.Instance.Control.isMove
                || Player.Instance._Animation.hiting)
            {
                StopCoroutine(enumerator);
                Player.Instance._Animation.hiting = false;
                isInteraction = false;
                isComplete = false;
                Player.Instance.Control.isAllowInteraction = true;  //��ȣ�ۿ� �����ϰ� �ٲ�
            }
        }

        //���� ��
        if (isMaking)
        {
            //�÷��̾� �����̸� ������ ��Ȱ��ȭ
            if (Player.Instance.Control.isMove
                || Player.Instance._Animation.hiting)
            {
                Player.Instance._Animation.hiting = false;
                UIManager.Instance.ActiveMakingNInven(m_MakingUI.gameObject);
                isMaking = m_MakingUI.gameObject.activeSelf;
            }
        }
    }


    public void Action()
    {
        StartAction();

    }

    void StartAction()
    {
        enumerator = UIActive();
        StartCoroutine(enumerator);
    }

    public E_InteractionType GetInteractionType()
    {
        return E_InteractionType.Making;
    }

    public bool IsAction()
    {
        //��ȣ�ۿ� ����, �÷��̾� ���� ����
        if (isAllowAction && Player.Instance.Control.isAllowMaking
            && !isInteraction)
            return true;

        return false;
    }

    //������ Ȱ��/��Ȱ��ȭ
    IEnumerator UIActive()
    {
        isInteraction = true;
        isComplete = true;
        Player.Instance.Control.isAllowInteraction = false; //�ٸ� ��ȣ�ۿ� �Ұ���
        Player.Instance.Control.isAllowCharMove = false;
        Debug.Log("���� ��ȣ�ۿ� ��");

        ObjectData.Info info = CSVManager.Instance.GetObjectInfo(m_ObjectPrefab.ID);
        Player.Instance._Animation.ani.SetTrigger("IsWork");
        //**************************************************************
        //���� ������Ʈ Ÿ��
        switch (MakingType)
        {
            case E_MakingType.None:
                break;
            case E_MakingType.Cook:
                Player.Instance._Animation.ani.SetInteger("IsFire", 1);
                Debug.Log("Fire");
                break;
            case E_MakingType.Workbench:
                //���۴� ����
                SoundManager.Instance.PlaySound(E_SoundType.Effect, info.Sound);
                Debug.Log("Ham");
                Player.Instance._Animation.ani.SetInteger("IsHam", 1);
                break;
            case E_MakingType.Inven:

                break;
            case E_MakingType.Brazier:
                Player.Instance._Animation.ani.SetInteger("IsFire", 1);
                Debug.Log("Fire");
                break;
            case E_MakingType.Max:
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(info.Gathering_Time);       //���
        Debug.Log("���� ��ȣ�ۿ� ��");

        //���� ������Ʈ Ÿ��
        switch (MakingType)
        {
            case E_MakingType.None:
                break;
            case E_MakingType.Cook:
                Player.Instance._Animation.ani.SetInteger("IsFire", 2);
                break;
            case E_MakingType.Workbench:
                Player.Instance._Animation.ani.SetInteger("IsHam", 2);
                break;
            case E_MakingType.Inven:

                break;
            case E_MakingType.Brazier:
                Player.Instance._Animation.ani.SetInteger("IsFire", 2);
                break;
            case E_MakingType.Max:
                break;
            default:
                break;
        }

        //**************************************************************

        Player.Instance.Control.isAllowCharMove = true;
        Player.Instance.Control.isAllowInteraction = true;  //�ٸ� ��ȣ�ۿ� ����

        if(isComplete)
        {
            UIManager.Instance.ActiveMakingNInven(m_MakingUI.gameObject);
            isMaking = m_MakingUI.gameObject.activeSelf;
        }
    }
}
