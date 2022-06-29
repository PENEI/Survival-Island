using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakingUIObj : MonoBehaviour, ActionObj
{
    public E_MakingType MakingType;

    [Header("상호작용 허용")]
    public bool isAllowAction;      //제작 유아이 허용
    
    [SerializeField]
    MakingPanel m_MakingUI;     //제작 유아이

    ObjectPrefab m_ObjectPrefab;
    AudioSource m_Audio;

    bool isInteraction;         //상호작용중
    bool isComplete;            //상호작용 끝확인
    bool isMaking;                  //제작중

    IEnumerator enumerator;
    void Awake()
    {
        //제작 유아이 설정한 타입에 맞춰서 가져옴
        m_MakingUI = UIManager.Instance.makingPanelList.PanelList.Find(x => x.MakingType == MakingType);
        m_ObjectPrefab = GetComponent<ObjectPrefab>();
        m_Audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        //오브젝트 사운드 재생
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
        //상호작용중인지 확인
        if (isInteraction)
        {
            //플레이어 움직이면 상호작용완료 불가능
            if (Player.Instance.Control.isMove
                || Player.Instance._Animation.hiting)
            {
                StopCoroutine(enumerator);
                Player.Instance._Animation.hiting = false;
                isInteraction = false;
                isComplete = false;
                Player.Instance.Control.isAllowInteraction = true;  //상호작용 가능하게 바꿈
            }
        }

        //제작 중
        if (isMaking)
        {
            //플레이어 움직이면 유아이 비활성화
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
        //상호작용 가능, 플레이어 제작 가능
        if (isAllowAction && Player.Instance.Control.isAllowMaking
            && !isInteraction)
            return true;

        return false;
    }

    //유아이 활성/비활성화
    IEnumerator UIActive()
    {
        isInteraction = true;
        isComplete = true;
        Player.Instance.Control.isAllowInteraction = false; //다른 상호작용 불가능
        Player.Instance.Control.isAllowCharMove = false;
        Debug.Log("제작 상호작용 중");

        ObjectData.Info info = CSVManager.Instance.GetObjectInfo(m_ObjectPrefab.ID);
        Player.Instance._Animation.ani.SetTrigger("IsWork");
        //**************************************************************
        //제작 오브젝트 타입
        switch (MakingType)
        {
            case E_MakingType.None:
                break;
            case E_MakingType.Cook:
                Player.Instance._Animation.ani.SetInteger("IsFire", 1);
                Debug.Log("Fire");
                break;
            case E_MakingType.Workbench:
                //제작대 사운드
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

        yield return new WaitForSeconds(info.Gathering_Time);       //대기
        Debug.Log("제작 상호작용 끝");

        //제작 오브젝트 타입
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
        Player.Instance.Control.isAllowInteraction = true;  //다른 상호작용 가능

        if(isComplete)
        {
            UIManager.Instance.ActiveMakingNInven(m_MakingUI.gameObject);
            isMaking = m_MakingUI.gameObject.activeSelf;
        }
    }
}
