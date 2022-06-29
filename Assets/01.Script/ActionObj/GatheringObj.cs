using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//채집 오브젝트
public class GatheringObj : MonoBehaviour, ActionObj
{
    [Header("채집 허용")]
    public bool isAllowGet;         //채집 허용, true: 채집가능 false: 채집불가능

    public int IncID;

    public GatheringInfo gatheringInfo;
    ObjectPrefab m_ObjectPrefab;

    protected bool isgathering;       //채집중   확인
    protected bool isComplete;       //채집 결과물 얻기 확인

    bool isData;

    IEnumerator enumerator;

    private void Awake()
    {
        m_ObjectPrefab = GetComponent<ObjectPrefab>();
    }

    private void Start()
    {
        SetData();
    }

    //******
    protected virtual void Update()
    {
        //채집중인지 확인
        if(isgathering)
        {
            //플레이어, 카메라가 움직이면 채집완료 불가능
            if (Player.Instance.Control.isMove
                || Player.Instance._Animation.hiting)
            {
                StopCoroutine(enumerator);
                Player.Instance._Animation.hiting = false;
                isComplete = false;
                isgathering = false;
                Player.Instance.Control.isAllowInteraction = true;  //상호작용 가능하게 바꿈
            }
        }
    }

    public bool IsAction()
    {
        //채집가능, 플레이어 도구가 채집할 수 있는 도구와 같은지
        ObjectData.Info info = CSVManager.Instance.GetObjectInfo(m_ObjectPrefab.ID);
        if (isAllowGet && UIManager.Instance.equipPanel.PlayerTool == info.Use_Tool
            && !isgathering)
            return true;

        return false;
    }

    //채집실행
    public void Action()
    {
        StartPickUp();
    }

    public E_InteractionType GetInteractionType()
    {
        return E_InteractionType.Gathering;
    }

    //데이터 세팅
    public void SetData(List<GatheringInfo> _list)
    {
        if (isData)
            return;
        isData = true;
        if(this.tag == "ActionObj")
        {
            GatheringInfo temp = _list.Find(x => x.IncID == IncID);     //아이디 찾기
                                                                        //없으면 생성
            if (temp == null)
            {
                temp = new GatheringInfo();
                _list.Add(temp);
                temp.IncID = IncID;     //아이디 세팅
            }

            gatheringInfo = temp;
            //채집 완료된 아이템 일 경우
            if (gatheringInfo.isCom)
                this.gameObject.SetActive(false);
        }
        else
        {
            GatheringInfo temp = new GatheringInfo();
            gatheringInfo = temp;
        }
    }

    public void SetData()
    {
        if (isData)
            return;
        isData = true;
        if(this.tag == "ActionObj")
        {
            GatheringInfo temp = new GatheringInfo();
            temp.IncID = IncID;     //아이디 세팅

            gatheringInfo = temp;
            //채집 완료된 아이템 일 경우
            if (gatheringInfo.isCom)
                this.gameObject.SetActive(false);
        }
        else
        {
            GatheringInfo temp = new GatheringInfo();
            gatheringInfo = temp;
        }
    }

    //획득  아이템 아이디 얻기
    void GetItems()
    {
        int count = 0;      //획득한 아이템 수

        bool[] getitem = new bool[4];

        //확률에 따른 추가
        ObjectData.Info info = CSVManager.Instance.GetObjectInfo(m_ObjectPrefab.ID);

        for (int i = 0; i < info.DropArr.Length; i++)
        {
            if (info.DropArr[i].itemID != -1)
            {
                if (Random.Range(0, info.DropArr[i].Percent) <= info.DropArr[i].Percent)
                {
                    ++count;
                    getitem[i] = true;
                }
            }
        }

        //인벤토리에 아이템 추가
        for (int i = 0; i < getitem.Length; i++)
        {
            if (getitem[i])
            {
                ItemData.Info iteminfo = CSVManager.Instance.GetItemInfo(info.DropArr[i].itemID);
                UIManager.Instance.invenPanel.AddItem(
                    new ItemInfo(info.DropArr[i].itemID, info.DropArr[i].count, iteminfo.Dur));   //아이템 추가
            }

        }
    }

    //채집 코루틴 시작
    void StartPickUp()
    {
        enumerator = PickUp();
        StartCoroutine(enumerator);
    }

    /// <summary>
    /// 아이템 줍기
    /// </summary>
    /// <param name="playerVec">아이템 픽업 시 플레이어 위치</param>
    /// <returns></returns>
    IEnumerator PickUp()
    {
        
        isgathering = true;     //채집중
        isComplete = true;      //채집결과물 얻기 
        Player.Instance.Control.isAllowInteraction = false; //다른 상호작용 불가능
        Debug.Log("수집중");

        Player.Instance._Animation.state = E_Player_State.Work;
        if (!Player.Instance._Animation.ani.GetBool("IdleExit"))
        {
            Player.Instance._Animation.ani.SetBool("IdleExit", true);
        }
        CancelInvoke("IdleAnimation");
        Player.Instance._Animation.ani.SetTrigger("IsWork");
        //**************************************************************
        //플레이어 도구
        switch (UIManager.Instance.equipPanel.PlayerTool)
        {
            case E_UseTool.None:
                break;
            case E_UseTool.Default:
                Player.Instance._Animation.ani.SetInteger("IsSearch",1);
                break;
            case E_UseTool.Bottle:
                Player.Instance._Animation.ani.SetInteger("IsWater", 1);
                break;
            case E_UseTool.Axe:
                Player.Instance._Animation.ani.SetInteger("IsAxe", 1);
                break;
            case E_UseTool.Shovel:
                Player.Instance._Animation.ani.SetInteger("IsShovel", 1);
                break;
            case E_UseTool.Knife:
                Player.Instance._Animation.ani.SetInteger("IsGalmuri", 1);
                break;
            case E_UseTool.Pickaxe:
                Player.Instance._Animation.ani.SetInteger("IsPickax", 1);
                break;

            case E_UseTool.Max:
                break;
            default:
                break;
        }

        Player.Instance.Control.isAllowCharMove = false;

        //채집 사운드 재생(도구)
        ItemData.Info iteminfo = CSVManager.Instance.GetItemInfo(
            UIManager.Instance.equipPanel.m_HandSlot.ItemInfo.itemID);
        if (iteminfo != null)
            SoundManager.Instance.PlaySound(E_SoundType.Effect, iteminfo.Sound);

        //채집 시간에 따른 채집
        ObjectData.Info info = CSVManager.Instance.GetObjectInfo(m_ObjectPrefab.ID);
        yield return new WaitForSeconds(info.Gathering_Time);
        Debug.Log($"수집끝,{info.Name_Eng},{isComplete}");

        //플레이어 도구
        switch (UIManager.Instance.equipPanel.PlayerTool)
        {
            case E_UseTool.None:
                break;
            case E_UseTool.Default:
                Player.Instance._Animation.ani.SetInteger("IsSearch", 2);
                break;
            case E_UseTool.Bottle:
                Player.Instance._Animation.ani.SetInteger("IsWater", 2);
                break;
            case E_UseTool.Axe:
                Player.Instance._Animation.ani.SetInteger("IsAxe", 2);
                break;
            case E_UseTool.Shovel:
                Player.Instance._Animation.ani.SetInteger("IsShovel", 2);
                break;
            case E_UseTool.Knife:
                Player.Instance._Animation.ani.SetInteger("IsGalmuri", 2);
                break;
            case E_UseTool.Pickaxe:
                Player.Instance._Animation.ani.SetInteger("IsPickax", 2);
                break;
            case E_UseTool.Max:
                break;
            default:
                break;
        }

        //**************************************************************
        Player.Instance.Control.isAllowCharMove = true;
        Player.Instance.Control.isAllowInteraction = true;  //다른 상호작용 가능
        //--------------------------------------------------------

        //아이템 얻는지 확인
        if (isComplete)
        {
            //채집 피로도 소모
            if (info.Fatigue_Reduction > 0)
                Player.Instance._Status.status.fatigue.statusValue -= info.Fatigue_Reduction;
            UIManager.Instance.equipPanel.ReductionHand();  //내구도감소
            GetItems();     //아이템얻기
            CompleteProcess();

            //채집 완료 시 
            SoundManager.Instance.PlaySound(E_SoundType.Effect, info.Sound);
        }

        Player.Instance._Animation.hiting = false;
    }

    //채집완료
    protected virtual void CompleteProcess()
    {
        this.gameObject.SetActive(false);       //채집된 오브젝트 비활성화
        gatheringInfo.isCom = true;
    }

}
