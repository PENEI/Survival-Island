using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MakingPanel : MonoBehaviour
{
    public bool isAllowMaking;              //���۰��� Ȯ��

    [Header("�������")]
    public E_MakingType MakingType;     //���� ���
    public int MakingID;

    [Header("���� ��� Ȯ��")]
    public bool isFuel;         //���� ��� Ȯ��
    [Header("���� ������ ���̵�/�̸�")]
    public int FuelItemID;      //���� ���̵�
    public string FuelItemName; //���� �̸�

    public bool isMakingItem;       //������ ��������� Ȯ��

    List<DefaultSlot> m_SourceList;     //��� ���� ����Ʈ
    MakingResultSlot m_ResultSlot;             //����� ����
    FuelSlot m_FuelSlot;               //���� ����

    ItemInfo result_item;   //��� ������ ����
    Recipe result_recipe;   //��� ������

    Recipe itemRecipe;      //���� ������ ������ ����

    void Awake()
    {
        itemRecipe = new Recipe();
        m_SourceList = new List<DefaultSlot>();

        //���� Ÿ�Կ� ���� �����Ҵ�
        m_SourceList.AddRange(this.gameObject.GetComponentsInChildren<DefaultSlot>());
        m_ResultSlot = this.gameObject.GetComponentInChildren<MakingResultSlot>();
        m_FuelSlot = this.gameObject.GetComponentInChildren<FuelSlot>();
    }


    void Update()
    {
        if(isAllowMaking)
            MakingProcess();
    }

    private void OnDisable()
    {
        if (isAllowMaking)
            GetSourceItem();
    }

    /// <summary>
    /// ����� ���� �� ������ ����
    /// </summary>
    public void GetResultItem()
    {
        //��� ������ ���� ����
        for (int i = 0; i < result_recipe.SourceArr.Length; i++)
        {
            if(result_recipe.SourceArr[i].ItemID > 0)
            {
                Slot slot = m_SourceList.Find(x => result_recipe.SourceArr[i].ItemID == x.ItemInfo.itemID);//������ ���� ã��
                slot.SetCount(slot.ItemInfo.count - (result_recipe.SourceArr[i].count * result_item.count));   //����
                if (isFuel)
                {
                    if (m_FuelSlot.isItem)
                    {
                        if (m_FuelSlot.ItemInfo.itemID == FuelItemID ||
                            CSVManager.Instance.GetItemInfo(m_FuelSlot.ItemInfo.itemID).Name_Eng == FuelItemName)
                            m_FuelSlot.SetCount(m_FuelSlot.ItemInfo.count - result_item.count);
                    }

                }
            }
        }
    }

    public void SetResultCount(int _count)
    {
        result_item.count = _count;
    }

    /// <summary>
    /// ���� ������ ����
    /// </summary>
    /// <param name="_recipe"></param>
    public void SetItem(Recipe _recipe)
    {
        //��� ����Ʈ �ʱ�ȭ
        foreach (Slot slot in m_SourceList)
        {
            slot.OffSlot();
        }

        result_recipe = _recipe;        //��������
        ItemData.Info tempitem;
        //��� ����
        int inx = 0;
        for (int i = 0; i < result_recipe.SourceArr.Length; i++)
        {
            if(result_recipe.SourceArr[i].ItemID > 0)
            {
                tempitem = CSVManager.Instance.GetItemInfo(result_recipe.SourceArr[i].ItemID);
                m_SourceList[inx++].SetSlot(new ItemInfo(result_recipe.SourceArr[i].ItemID, result_recipe.SourceArr[i].count, tempitem.Dur));

            }
        }

        //����
        if(isFuel)
        {
            tempitem = CSVManager.Instance.GetItemInfo(FuelItemID);
            m_FuelSlot.SetSlot(new ItemInfo(FuelItemID, 1, tempitem.Dur));
        }

        //�����
        tempitem = CSVManager.Instance.GetItemInfo(_recipe.ResultItemID);
        result_item = new ItemInfo(_recipe.ResultItemID, 1, tempitem.Dur);
        m_ResultSlot.SetSlot(result_item);

    }

    /// <summary>
    /// �������� �κ��丮��
    /// </summary>
    void GetSourceItem()
    {
        foreach (Slot slot in m_SourceList)
        {
            if (slot.isItem)
            {
                if (UIManager.Instance.invenPanel != null)
                {
                    if (UIManager.Instance.invenPanel.AddItem(slot.ItemInfo))
                    {
                        slot.OffSlot();
                    }
                }
            }
        }
    }

    //�����
    void MakingProcess()
    {
        MakingInfo info = MakingManager.Instance.GetMakingInfo(MakingType); //�������� ���
        if(info != null)
        {
            //���� üũ
            if(ItemMakeCheck(info, out result_item))
            {
                isMakingItem = true;
                m_ResultSlot.SetSlot(result_item);       //��� ������ ����
            }
            else
            {
                isMakingItem = false;
                m_ResultSlot.OffSlot();
            }
        }
    }

    //���� üũ
    bool ItemMakeCheck(MakingInfo _info, out ItemInfo _resultitem)
    {
        _resultitem = new ItemInfo();             //���۵� ������

        //���� Ȯ��
        if (isFuel)
        {
            if (!m_FuelSlot.isItem ||
                m_FuelSlot.ItemInfo.itemID != FuelItemID &&
                 CSVManager.Instance.GetItemInfo(m_FuelSlot.ItemInfo.itemID).Name_Eng != FuelItemName)
                return false;
        }

        itemRecipe.ClearSource();

        int itemcount = 0;      //��ώ�Կ� ������
        for (int i = 0; i < m_SourceList.Count; i++)
        {
            if (m_SourceList[i].ItemInfo.itemID > 0)
            {
                //��� ���Կ� �������� ���� ��� �����۵����� �߰�
                itemRecipe.SourceArr[i].ItemID = m_SourceList[i].ItemInfo.itemID;   
                itemRecipe.SourceArr[i].count = m_SourceList[i].ItemInfo.count;
                itemcount++;
            }
        }
        //��ώ�Կ� �������� ������ false
        if (itemcount <= 0)
            return false;

        itemRecipe.SourceSort();    //����

        result_recipe = _info.GetResult(itemRecipe);    //��� ������ ���

        if (result_recipe != null)
        {
            _resultitem.itemID = result_recipe.ResultItemID;     //������� ������ ���̵�
            int makecount = MinCount(itemRecipe, result_recipe);    //������� ���� ���

            ItemData.Info info = CSVManager.Instance.GetItemInfo(result_recipe.ResultItemID);   //������ ����
            _resultitem.count = makecount;      //��������� ����
            _resultitem.durability = info.Dur;  //������
            return true;
        }
        return false;
    }

    //�ּ� ���� ���
    int MinCount(Recipe _recipe, Recipe _reulstrecipe)
    {
        //���� ���
        int count = -1;
        for (int i = 0; i < _recipe.SourceArr.Length; i++)
        {
            if(_recipe.SourceArr[i].ItemID != 0)
            {
                if(count == -1)
                {
                    count = _recipe.SourceArr[i].count / _reulstrecipe.SourceArr[i].count;  
                }
                else
                {
                    int temp = _recipe.SourceArr[i].count / _reulstrecipe.SourceArr[i].count;
                    count = temp > count ? count : temp;        //�ּҼ��� Ȯ��
                }

            }
        }

        if (isFuel)
        {
            if (m_FuelSlot.isItem)
            {
                if (m_FuelSlot.ItemInfo.itemID == FuelItemID ||
                 CSVManager.Instance.GetItemInfo(m_FuelSlot.ItemInfo.itemID).Name_Eng == FuelItemName)
                {
                    if (count > m_FuelSlot.ItemInfo.count)
                        count = m_FuelSlot.ItemInfo.count;
                }
            }

        }

        // �ִ� ���� Ȯ��
        ItemData.Info info = CSVManager.Instance.GetItemInfo(result_recipe.ResultItemID);   //������ ����
        if (info.Stack < count)
            count = info.Stack;

        return count;
    }
}
