using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

//���� ������ ����
[System.Serializable]
public class ItemInfo
{
    public int itemID;           //������ ���̵�
    public int count;               //����
    public int durability;      //������

    public ItemInfo()
    {
    }

    public ItemInfo(ItemInfo itemInfo) : this()
    {
        itemID = itemInfo.itemID;
        count = itemInfo.count;
        durability = itemInfo.durability;
    }

    public ItemInfo(int _itemID, int _count, int _dur) : this()
    {
        this.itemID = _itemID;
        this.count = _count;
        this.durability = _dur;
    }

    public void Set(ItemInfo itemInfo)
    {
        this.itemID = itemInfo.itemID;
        this.count = itemInfo.count;
        this.durability = itemInfo.durability;
    }
}

public struct ItemDropInfo
{
    public int itemID;
    public int count;
    public int Percent;
}

[System.Serializable]
public struct SourceInfo
{
    public int ItemID;
    public int count;

    public void Init()
    {
        ItemID = 0;
        count = 0;
    }
}

[System.Serializable]
public class Recipe
{
    public int ResultItemID { get; }        //���
    public SourceInfo[] SourceArr; //��� ������

    public Recipe()
    {
        SourceArr = new SourceInfo[7];
    }

    public Recipe(ItemData.Info _item)
    {
        SourceArr = new SourceInfo[7];

        for (int i = 0; i < _item.SourceArr.Length; i++)
        {
            if(_item.SourceArr[i].ItemID >0)
            {
                SourceArr[i] = _item.SourceArr[i];
            }

        }
        SourceSort();
        ResultItemID = _item.ID;
    }

    public Recipe(ObjectData.Info _item)
    {
        SourceArr = new SourceInfo[7];

        for (int i = 0; i < _item.SourceArr.Length; i++)
        {
            if (_item.SourceArr[i].ItemID > 0)
            {
                SourceArr[i] = _item.SourceArr[i];
            }
        }
        SourceSort();
        ResultItemID = _item.ID;
    }

    //���̵� ���� ���� ����
    public void SourceSort()
    {
        SourceArr = SourceArr.OrderBy(x => x.ItemID).ToArray();
    }

    //���迭 �ʱ�ȭ
    public void ClearSource()
    {
        for (int i = 0; i < SourceArr.Length; i++)
        {
            SourceArr[i].Init();
        }
    }

    //��� ������ ���̵����� ���ڿ� ����
    public string GetSourceString()
    {
        string str = "";

        for (int i = 0; i < SourceArr.Length; i++)
        {
            str += SourceArr[i].ItemID.ToString();
            str += ",";
        }

        return str;
    }

    //���� Ȯ��
    public bool CheckCount(Recipe _sourcerecipe)
    {
        for (int i = 0; i < SourceArr.Length; i++)
        {
            if (_sourcerecipe.SourceArr[i].count < SourceArr[i].count)
            {
                return false;
            }
        }
        return true;
    }

    //��� �� ����
    public int MaxSourceCount()
    {
        int count = 0;
        for (int i = 0; i < SourceArr.Length; i++)
        {
            count += SourceArr[i].count;
        }
        return count;
    }
}