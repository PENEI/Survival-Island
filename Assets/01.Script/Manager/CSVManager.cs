using System.Collections.Generic;
using UnityEngine;

public class CSVManager : Singleton<CSVManager>
{
    [Header("오브젝트 생성 부모")]
    public Transform parent;                                //오브젝트 생성 시 부모
    [Header("아이템 CSV")]
    public TextAsset ItemCSV;                               //아이템 CSV
    [Header("ObjCSV")]
    public TextAsset ObejctCSV;                        //ObjCSV

    public ItemData ItemCSVData = new ItemData();       //아이템 데이터
    public ObjectData ObjData = new ObjectData();     //Obj 데이터

    Dictionary<int, ItemData.Info> ItemCSVDic;
    Dictionary<int, ObjectData.Info> ObjectCSVDic;

    void Awake()
    {
    }

    protected override void SingletonInit()
    {
        ItemCSVDic = new Dictionary<int, ItemData.Info>();
        ObjectCSVDic = new Dictionary<int, ObjectData.Info>();

        //CSV읽기
        ItemCSVData.Load(ItemCSV);
        ObjData.Load(ObejctCSV);

        //아이템
        List<ItemData.Info> iteminfo_list = ItemCSVData.GetRowList();       //아이템 정보 리스트
        foreach (ItemData.Info item in iteminfo_list)
        {
            ItemCSVDic.Add(item.ID, item);
        }

        //오브젝트
        List<ObjectData.Info> objinfo_list = ObjData.GetRowList();             //N_Obj 정보 리스트
        foreach (ObjectData.Info item in objinfo_list)
        {
            ObjectCSVDic.Add(item.ID, item);
        }
    }

    public ObjectData.Info GetObjectInfo(int _id)
    {
        if(ObjectCSVDic.TryGetValue(_id, out ObjectData.Info info))
        {
            return info;
        }
        return null;
    }

    public ItemData.Info GetItemInfo(int _id)
    {
        if (ItemCSVDic.TryGetValue(_id, out ItemData.Info info))
        {
            return info;
        }
        return null;
    }

    //Obj 생성
    [ContextMenu("ObjectCSVCreate")]
    void ObjectCSVCreate()
    {
        //CSV파일 읽기
        ObjData.Load(ObejctCSV);

        //카피할 게임 오브젝트 생성
        GameObject copy = new GameObject();

        //오브젝트 생성
        foreach (ObjectData.Info row in ObjData.GetRowList())
        {
            GameObject tempobj = GameObject.Instantiate(copy, parent);
            tempobj.name = row.Name_Eng;                //오브젝트 이름
            //컴포넌트 추가
            tempobj.AddComponent<MeshRenderer>();   
            tempobj.AddComponent<MeshFilter>();
            tempobj.AddComponent<CapsuleCollider>();
            ObjectPrefab Objprefab = tempobj.AddComponent<ObjectPrefab>();
            //Objprefab.itemInfo = row;      //데이터 넣기

            switch (row.Object_Type)
            {
                //자연 오브젝트 
                case E_Object_Type.N_Obj:
                    tempobj.AddComponent<GatheringObj>();   //채집 스크립트 추가
                    break;
            }

            tempobj.tag = "ActionObj";            //태그
        }

        //카피한 오브젝트 삭제
        GameObject.DestroyImmediate(copy);
    }

    //Item 생성
    [ContextMenu("ItemCSVCreate")]
    void ItemCSVCreate()
    {
        //아이템 CSV읽기
        ItemCSVData.Load(ItemCSV);

        //카피할 오브젝트
        GameObject copy = new GameObject();

        //생성
        foreach (ItemData.Info row in ItemCSVData.GetRowList())
        {
            GameObject tempobj = GameObject.Instantiate(copy, parent);
            tempobj.name = row.Name_Eng;                //이름
            //컴포넌트 추가
            ItemPrefab tempdata = tempobj.AddComponent<ItemPrefab>();
            //tempdata.itemInfo = row;    //아이템 데이터
        }

        //카피한 오브젝트 삭제
        GameObject.DestroyImmediate(copy);
    }
}
