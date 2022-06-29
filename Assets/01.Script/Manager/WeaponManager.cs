using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    List<ItemModel> ToolModelList;                  //������ ���� �� ����Ʈ
    Dictionary<int, ItemModel> ToolModelDic;     //������ ���� �� ��ųʸ� (������ ���̵�, ��)

    protected override void SingletonInit()
    {
        ToolModelList = new List<ItemModel>();
        ToolModelDic = new Dictionary<int, ItemModel>();
        //���� ã��
        List<ItemPrefab> ModelList = new List<ItemPrefab>();
        foreach (ItemPrefab item in ObjManager.Instance.ItemList)
        {
            ItemData.Info info = CSVManager.Instance.GetItemInfo(item.ID);
            if (info.Material_Type == E_MaterialType.Tool)
                ModelList.Add(item);
        }

        foreach (ItemPrefab item in ModelList)
        {
            if(item.model != null)
            {
                ItemModel model = GameObject.Instantiate<ItemModel>(item.model, this.transform);    //���� ����
                ToolModelList.Add(model);                       //����Ʈ�� �߰�
                ToolModelDic.Add(item.ID, model);  //��ųʸ��� �߰�
                model.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ������ �� ��������
    /// </summary>
    /// <param name="_id">���̵�</param>
    /// <returns></returns>
    public ItemModel GetToolModel(int _id)
    {
        if(ToolModelDic.TryGetValue(_id, out ItemModel model))
        {
            return model;
        }
        return null;
    }
}
