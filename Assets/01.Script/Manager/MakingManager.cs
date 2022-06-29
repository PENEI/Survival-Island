using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class MakingInfo
{
    [SerializeField, Header("����Ÿ��")]
    E_MakingType makingType;                //����Ÿ��
    [SerializeField, Header("���� ��� ���̵�")]
    int PlaceID;        //���� ��� ���̵�
    [SerializeField]
    List<Recipe> RecipeList;            //���� ������
    Dictionary<string, List<Recipe>> RecipeDic; //���� ������ ��ųʸ�(������������, ���� ������ ����Ʈ)

    //�ʱ�ȭ
    public void Init(List<ItemData.Info> dataList)
    {
        RecipeList = new List<Recipe>();
        RecipeDic = new Dictionary<string, List<Recipe>>();

        //����Ʈ�� �߰�
        foreach (ItemData.Info item in dataList)
        {
            RecipeList.Add(new Recipe(item));
        }

        RecipeList = RecipeList.OrderBy(x => x.ResultItemID).ToList();  //����

        //��ųʸ��� �߰�
        foreach (Recipe item in RecipeList)
        {
            //Ű���� �ִ��� Ȯ��
            if(RecipeDic.TryGetValue(item.GetSourceString(), out List<Recipe> recipe))
            {
                //������ �����Ǹ���Ʈ�� �߰�
                recipe.Add(item);
            }
            else
            {
                //������ ����
                recipe = new List<Recipe>();
                recipe.Add(item);
                RecipeDic.Add(item.GetSourceString(), recipe);

            }
        }


        //���� ��ųʸ� ����Ʈ ����
        foreach (List<Recipe> item in RecipeDic.Values)
        {
            if(item.Count >= 2)
            {
                item.Sort((a, b) => a.MaxSourceCount() > b.MaxSourceCount() ? 1 : -1);      //���� ������ �������� ����
                item.Reverse();
            }
        }
    }

    //���� ���
    public E_MakingType GetMakingType()
    {
        return makingType;
    }

    //���� ��Ҿ��̵�
    public int GetPlaceID()
    {
        return PlaceID;
    }

    //��� ������ �ޱ�
    public Recipe GetResult(Recipe _recipe)
    {
        RecipeDic.TryGetValue(_recipe.GetSourceString(), out List<Recipe> resultRecipe);    //Ű�� Ȯ��
        if(resultRecipe != null)
        {
            //������ ���� Ȯ��
            foreach (Recipe item in resultRecipe)
            {
                if(item.CheckCount(_recipe))
                {
                    return item;
                }
            }
        }
        return null;
    }

    public List<Recipe> GetRecipeList() { return RecipeList; }
}

public class MakingManager : Singleton<MakingManager>
{
    [SerializeField]
    List<MakingInfo> makingList;                         //���� ���� ����Ʈ
    
    public List<Recipe> AllMakingList { get; private set; }                             //��� ���� ����
    public Dictionary<E_MaterialType, List<Recipe>> MaterialRecipeDic { get; private set; } //���׸��� �� ���� ��ųʸ�
    Dictionary<E_MakingType, MakingInfo> MakingDic;      //���� ���� ��༭��, (����Ÿ��, ��������)

    public List<Recipe> BuildList;                  //���� ����Ʈ
    public Dictionary<int, ObjectPrefab> BuildObjectList;   //���� ��ųʸ�(���̵�, ������ )

    protected override void SingletonInit()
    {
        MakeListInit();
        BuildInit();
    }

    public MakingInfo GetMakingInfo(E_MakingType _type)
    {
        MakingDic.TryGetValue(_type, out MakingInfo info);
        return info;
    }


    //���۸���Ʈ �ʱ�ȭ
    void MakeListInit()
    {
        MakingDic = new Dictionary<E_MakingType, MakingInfo>();     //���� ��� ��ųʸ�(�������, ��������)

        List<ItemData.Info> MakeItemList = CSVManager.Instance.ItemCSVData.FindAll_Craft(true); //���۰��� ������ ����Ʈ
        List<ItemData.Info> tempList;

        //���� ���� �ʱ�ȭ
        foreach (MakingInfo makeinfo in makingList)
        {
            tempList = MakeItemList.FindAll(x => x.Craft_Place == makeinfo.GetPlaceID());
            makeinfo.Init(tempList);
            MakingDic.Add(makeinfo.GetMakingType(), makeinfo);
            tempList.Clear();
        }

        //��� ���� ����
        AllMakingList = new List<Recipe>();
        foreach (MakingInfo makeinfo in makingList)
        {
            foreach (Recipe recipe in makeinfo.GetRecipeList())
            {
                AllMakingList.Add(recipe);
            }
        }
        AllMakingList = AllMakingList.OrderBy(x => x.ResultItemID).ToList();        //����

        //���׸��� �� ���� ����
        MaterialRecipeDic = new Dictionary<E_MaterialType, List<Recipe>>();
        for (int i = 0; i < (int)E_MaterialType.Use_Item; i++)
        {
            List<Recipe> temprecipe = AllMakingList.FindAll(x => 
            CSVManager.Instance.GetItemInfo(x.ResultItemID).Material_Type == (E_MaterialType)i + 1);
            if((E_MaterialType)i + 1 == E_MaterialType.Use_Item)
            {
                temprecipe.AddRange(AllMakingList.FindAll(x =>
            CSVManager.Instance.GetItemInfo(x.ResultItemID).Material_Type == E_MaterialType.Medicene));
            }
            MaterialRecipeDic.Add((E_MaterialType)i + 1, temprecipe);
        }

    }

    //��������
    void BuildInit()
    {
        List<ObjectPrefab> MakeBuildList = new List<ObjectPrefab>();        //�ΰ�������Ʈ ����Ʈ
        //�ΰ�������Ʈ ã��
        foreach (ObjectPrefab item in ObjManager.Instance.ObjList)
        {
            ObjectData.Info info = CSVManager.Instance.GetObjectInfo(item.ID);
            if (info !=null && info.Object_Type == E_Object_Type.A_Obj)
                MakeBuildList.Add(item);
        }

        //������ ���
        foreach (ObjectPrefab item in MakeBuildList)
        {
            ObjectData.Info info = CSVManager.Instance.GetObjectInfo(item.ID);
            BuildList.Add(new Recipe(info));
        }

        //��ųʸ� �ʱ�ȭ, (�����۾��̵�, ������Ʈ ������)
        BuildObjectList = new Dictionary<int, ObjectPrefab>();
        foreach (ObjectPrefab item in MakeBuildList)
        {
            BuildObjectList.Add(item.ID, item);
        }

    }
}
