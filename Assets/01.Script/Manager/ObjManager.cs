using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ObjManager : Singleton<ObjManager>
{
    [Header("Obj 프리팹 리스트")]
    public List<ObjectPrefab> ObjList;
    [Header("Item 프리팹 리스트")]
    public List<ItemPrefab> ItemList;

    public string ObjectPath;
    public string ItemPath;

    [Header("확인용")]
    public List<GatheringObj> InsGatheringObjList;      //생성된 오브젝트 리스트
    public List<DelayObj> InsDelayObjList;      //생성된 오브젝트 리스트
    public List<BuildUIObj> InsBuildObjList;       //건축 오브젝트 리스트

    [SerializeField]
    int GatheringId;
    [SerializeField]
    int DelayId;
    [SerializeField]
    int BuildId;

    Dictionary<int, ItemPrefab> ItemPrefabDic;

    protected override void SingletonInit()
    {
        ObjList.AddRange(Resources.LoadAll<ObjectPrefab>(ObjectPath));
        ItemList.AddRange(Resources.LoadAll<ItemPrefab>(ItemPath));

        //아이디 값에 따른 정렬
        ObjList = ObjList.OrderBy(x => x.ID).ToList();
        ItemList = ItemList.OrderBy(x => x.ID).ToList();

        //아이템 프리팹 딕셔너리
        ItemPrefabDic = new Dictionary<int, ItemPrefab>();
        foreach (ItemPrefab item in ItemList)
        {
            ItemPrefabDic.Add(item.ID, item);
        }

        //데이터 세팅
        //채집물
        foreach (GatheringObj item in InsGatheringObjList)
        {
            if (item != null)
                item.SetData(XmlManager.Instance.dataInfo.gatheringInfoList);
        }
        //딜레이 채집물
        foreach (DelayObj item in InsDelayObjList)
        {
            if(item != null)
               item.SetData(XmlManager.Instance.dataInfo.delayInfoList);
        }
        //건축 오브젝트
        foreach (BuildUIObj item in InsBuildObjList)
        {
            if(item != null)
            {
                item.SetData(XmlManager.Instance.dataInfo.buildInfoList);
            }
        }

    }

    public ItemPrefab GetItemPrefab(int _id)
    {
        if(ItemPrefabDic.TryGetValue(_id, out ItemPrefab item))
        {
            return item;
        }
        return null;
    }

#if UNITY_EDITOR
    [ContextMenu("SetIncInit")]
    void SetIncInit()
    {
        InsBuildObjList.RemoveAll(x => x == null);
        InsDelayObjList.RemoveAll(x => x == null);
        InsGatheringObjList.RemoveAll(x => x == null);

        List<GatheringObj> temp_gathering = new List<GatheringObj>();
        List<DelayObj> temp_delay = new List<DelayObj>();
        List<BuildUIObj> temp_build = new List<BuildUIObj>();

        temp_gathering.AddRange(GameObject.FindObjectsOfType<GatheringObj>());
        temp_delay.AddRange(GameObject.FindObjectsOfType<DelayObj>());
        temp_build.AddRange(GameObject.FindObjectsOfType<BuildUIObj>());

        foreach (DelayObj item in temp_delay)
        {
            temp_gathering.Remove(item);
        }

        foreach (GatheringObj item in temp_gathering)
        {
            if(item.tag == "ActionObj")
            {
                if (!InsGatheringObjList.Contains(item))
                {
                    item.IncID = ++GatheringId;
                    InsGatheringObjList.Add(item);
                    PrefabUtility.RecordPrefabInstancePropertyModifications(item);
                }
            }

        }

        foreach (DelayObj item in temp_delay)
        {
            if (!InsDelayObjList.Contains(item))
            {
                item.IncID = ++DelayId;
                InsDelayObjList.Add(item);
                PrefabUtility.RecordPrefabInstancePropertyModifications(item);
            }
        }

        foreach (BuildUIObj item in temp_build)
        {
            if (!InsBuildObjList.Contains(item))
            {
                item.IncID = ++BuildId;
                InsBuildObjList.Add(item);
                PrefabUtility.RecordPrefabInstancePropertyModifications(item);
            }
        } 
    }
#endif
}
