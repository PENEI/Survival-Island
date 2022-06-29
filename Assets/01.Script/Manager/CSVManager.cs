using System.Collections.Generic;
using UnityEngine;

public class CSVManager : Singleton<CSVManager>
{
    [Header("������Ʈ ���� �θ�")]
    public Transform parent;                                //������Ʈ ���� �� �θ�
    [Header("������ CSV")]
    public TextAsset ItemCSV;                               //������ CSV
    [Header("ObjCSV")]
    public TextAsset ObejctCSV;                        //ObjCSV

    public ItemData ItemCSVData = new ItemData();       //������ ������
    public ObjectData ObjData = new ObjectData();     //Obj ������

    Dictionary<int, ItemData.Info> ItemCSVDic;
    Dictionary<int, ObjectData.Info> ObjectCSVDic;

    void Awake()
    {
    }

    protected override void SingletonInit()
    {
        ItemCSVDic = new Dictionary<int, ItemData.Info>();
        ObjectCSVDic = new Dictionary<int, ObjectData.Info>();

        //CSV�б�
        ItemCSVData.Load(ItemCSV);
        ObjData.Load(ObejctCSV);

        //������
        List<ItemData.Info> iteminfo_list = ItemCSVData.GetRowList();       //������ ���� ����Ʈ
        foreach (ItemData.Info item in iteminfo_list)
        {
            ItemCSVDic.Add(item.ID, item);
        }

        //������Ʈ
        List<ObjectData.Info> objinfo_list = ObjData.GetRowList();             //N_Obj ���� ����Ʈ
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

    //Obj ����
    [ContextMenu("ObjectCSVCreate")]
    void ObjectCSVCreate()
    {
        //CSV���� �б�
        ObjData.Load(ObejctCSV);

        //ī���� ���� ������Ʈ ����
        GameObject copy = new GameObject();

        //������Ʈ ����
        foreach (ObjectData.Info row in ObjData.GetRowList())
        {
            GameObject tempobj = GameObject.Instantiate(copy, parent);
            tempobj.name = row.Name_Eng;                //������Ʈ �̸�
            //������Ʈ �߰�
            tempobj.AddComponent<MeshRenderer>();   
            tempobj.AddComponent<MeshFilter>();
            tempobj.AddComponent<CapsuleCollider>();
            ObjectPrefab Objprefab = tempobj.AddComponent<ObjectPrefab>();
            //Objprefab.itemInfo = row;      //������ �ֱ�

            switch (row.Object_Type)
            {
                //�ڿ� ������Ʈ 
                case E_Object_Type.N_Obj:
                    tempobj.AddComponent<GatheringObj>();   //ä�� ��ũ��Ʈ �߰�
                    break;
            }

            tempobj.tag = "ActionObj";            //�±�
        }

        //ī���� ������Ʈ ����
        GameObject.DestroyImmediate(copy);
    }

    //Item ����
    [ContextMenu("ItemCSVCreate")]
    void ItemCSVCreate()
    {
        //������ CSV�б�
        ItemCSVData.Load(ItemCSV);

        //ī���� ������Ʈ
        GameObject copy = new GameObject();

        //����
        foreach (ItemData.Info row in ItemCSVData.GetRowList())
        {
            GameObject tempobj = GameObject.Instantiate(copy, parent);
            tempobj.name = row.Name_Eng;                //�̸�
            //������Ʈ �߰�
            ItemPrefab tempdata = tempobj.AddComponent<ItemPrefab>();
            //tempdata.itemInfo = row;    //������ ������
        }

        //ī���� ������Ʈ ����
        GameObject.DestroyImmediate(copy);
    }
}
