using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    List<ItemModel> ToolModelList;                  //아이템 도구 모델 리스트
    Dictionary<int, ItemModel> ToolModelDic;     //아이템 도구 모델 딕셔너리 (아이템 아이디, 모델)

    protected override void SingletonInit()
    {
        ToolModelList = new List<ItemModel>();
        ToolModelDic = new Dictionary<int, ItemModel>();
        //도구 찾기
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
                ItemModel model = GameObject.Instantiate<ItemModel>(item.model, this.transform);    //도구 생성
                ToolModelList.Add(model);                       //리스트에 추가
                ToolModelDic.Add(item.ID, model);  //딕셔너리에 추가
                model.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 아이템 모델 가져오기
    /// </summary>
    /// <param name="_id">아이디</param>
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
