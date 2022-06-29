using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//ä�� ������Ʈ
public class GatheringObj : MonoBehaviour, ActionObj
{
    [Header("ä�� ���")]
    public bool isAllowGet;         //ä�� ���, true: ä������ false: ä���Ұ���

    public int IncID;

    public GatheringInfo gatheringInfo;
    ObjectPrefab m_ObjectPrefab;

    protected bool isgathering;       //ä����   Ȯ��
    protected bool isComplete;       //ä�� ����� ��� Ȯ��

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
        //ä�������� Ȯ��
        if(isgathering)
        {
            //�÷��̾�, ī�޶� �����̸� ä���Ϸ� �Ұ���
            if (Player.Instance.Control.isMove
                || Player.Instance._Animation.hiting)
            {
                StopCoroutine(enumerator);
                Player.Instance._Animation.hiting = false;
                isComplete = false;
                isgathering = false;
                Player.Instance.Control.isAllowInteraction = true;  //��ȣ�ۿ� �����ϰ� �ٲ�
            }
        }
    }

    public bool IsAction()
    {
        //ä������, �÷��̾� ������ ä���� �� �ִ� ������ ������
        ObjectData.Info info = CSVManager.Instance.GetObjectInfo(m_ObjectPrefab.ID);
        if (isAllowGet && UIManager.Instance.equipPanel.PlayerTool == info.Use_Tool
            && !isgathering)
            return true;

        return false;
    }

    //ä������
    public void Action()
    {
        StartPickUp();
    }

    public E_InteractionType GetInteractionType()
    {
        return E_InteractionType.Gathering;
    }

    //������ ����
    public void SetData(List<GatheringInfo> _list)
    {
        if (isData)
            return;
        isData = true;
        if(this.tag == "ActionObj")
        {
            GatheringInfo temp = _list.Find(x => x.IncID == IncID);     //���̵� ã��
                                                                        //������ ����
            if (temp == null)
            {
                temp = new GatheringInfo();
                _list.Add(temp);
                temp.IncID = IncID;     //���̵� ����
            }

            gatheringInfo = temp;
            //ä�� �Ϸ�� ������ �� ���
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
            temp.IncID = IncID;     //���̵� ����

            gatheringInfo = temp;
            //ä�� �Ϸ�� ������ �� ���
            if (gatheringInfo.isCom)
                this.gameObject.SetActive(false);
        }
        else
        {
            GatheringInfo temp = new GatheringInfo();
            gatheringInfo = temp;
        }
    }

    //ȹ��  ������ ���̵� ���
    void GetItems()
    {
        int count = 0;      //ȹ���� ������ ��

        bool[] getitem = new bool[4];

        //Ȯ���� ���� �߰�
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

        //�κ��丮�� ������ �߰�
        for (int i = 0; i < getitem.Length; i++)
        {
            if (getitem[i])
            {
                ItemData.Info iteminfo = CSVManager.Instance.GetItemInfo(info.DropArr[i].itemID);
                UIManager.Instance.invenPanel.AddItem(
                    new ItemInfo(info.DropArr[i].itemID, info.DropArr[i].count, iteminfo.Dur));   //������ �߰�
            }

        }
    }

    //ä�� �ڷ�ƾ ����
    void StartPickUp()
    {
        enumerator = PickUp();
        StartCoroutine(enumerator);
    }

    /// <summary>
    /// ������ �ݱ�
    /// </summary>
    /// <param name="playerVec">������ �Ⱦ� �� �÷��̾� ��ġ</param>
    /// <returns></returns>
    IEnumerator PickUp()
    {
        
        isgathering = true;     //ä����
        isComplete = true;      //ä������� ��� 
        Player.Instance.Control.isAllowInteraction = false; //�ٸ� ��ȣ�ۿ� �Ұ���
        Debug.Log("������");

        Player.Instance._Animation.state = E_Player_State.Work;
        if (!Player.Instance._Animation.ani.GetBool("IdleExit"))
        {
            Player.Instance._Animation.ani.SetBool("IdleExit", true);
        }
        CancelInvoke("IdleAnimation");
        Player.Instance._Animation.ani.SetTrigger("IsWork");
        //**************************************************************
        //�÷��̾� ����
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

        //ä�� ���� ���(����)
        ItemData.Info iteminfo = CSVManager.Instance.GetItemInfo(
            UIManager.Instance.equipPanel.m_HandSlot.ItemInfo.itemID);
        if (iteminfo != null)
            SoundManager.Instance.PlaySound(E_SoundType.Effect, iteminfo.Sound);

        //ä�� �ð��� ���� ä��
        ObjectData.Info info = CSVManager.Instance.GetObjectInfo(m_ObjectPrefab.ID);
        yield return new WaitForSeconds(info.Gathering_Time);
        Debug.Log($"������,{info.Name_Eng},{isComplete}");

        //�÷��̾� ����
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
        Player.Instance.Control.isAllowInteraction = true;  //�ٸ� ��ȣ�ۿ� ����
        //--------------------------------------------------------

        //������ ����� Ȯ��
        if (isComplete)
        {
            //ä�� �Ƿε� �Ҹ�
            if (info.Fatigue_Reduction > 0)
                Player.Instance._Status.status.fatigue.statusValue -= info.Fatigue_Reduction;
            UIManager.Instance.equipPanel.ReductionHand();  //����������
            GetItems();     //�����۾��
            CompleteProcess();

            //ä�� �Ϸ� �� 
            SoundManager.Instance.PlaySound(E_SoundType.Effect, info.Sound);
        }

        Player.Instance._Animation.hiting = false;
    }

    //ä���Ϸ�
    protected virtual void CompleteProcess()
    {
        this.gameObject.SetActive(false);       //ä���� ������Ʈ ��Ȱ��ȭ
        gatheringInfo.isCom = true;
    }

}
