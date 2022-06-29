using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ObjManager : Singleton<ObjManager>
{
    [Header("Obj ������ ����Ʈ")]
    public List<ObjectPrefab> ObjList;
    [Header("Item ������ ����Ʈ")]
    public List<ItemPrefab> ItemList;

    public string ObjectPath;
    public string ItemPath;

    [Header("Ȯ�ο�")]
    public List<GatheringObj> InsGatheringObjList;      //������ ������Ʈ ����Ʈ
    public List<DelayObj> InsDelayObjList;      //������ ������Ʈ ����Ʈ
    public List<BuildUIObj> InsBuildObjList;       //���� ������Ʈ ����Ʈ

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

        //���̵� ���� ���� ����
        ObjList = ObjList.OrderBy(x => x.ID).ToList();
        ItemList = ItemList.OrderBy(x => x.ID).ToList();

        //������ ������ ��ųʸ�
        ItemPrefabDic = new Dictionary<int, ItemPrefab>();
        foreach (ItemPrefab item in ItemList)
        {
            ItemPrefabDic.Add(item.ID, item);
        }

        //������ ����
        //ä����
        foreach (GatheringObj item in InsGatheringObjList)
        {
            if (item != null)
                item.SetData(XmlManager.Instance.dataInfo.gatheringInfoList);
        }
        //������ ä����
        foreach (DelayObj item in InsDelayObjList)
        {
            if(item != null)
               item.SetData(XmlManager.Instance.dataInfo.delayInfoList);
        }
        //���� ������Ʈ
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
