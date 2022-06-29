using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class MakingInfo
{
    [SerializeField, Header("제작타입")]
    E_MakingType makingType;                //제작타입
    [SerializeField, Header("제작 장소 아이디")]
    int PlaceID;        //제작 장소 아이디
    [SerializeField]
    List<Recipe> RecipeList;            //제작 레시피
    Dictionary<string, List<Recipe>> RecipeDic; //제작 레시피 딕셔너리(재료아이템정보, 제작 레시피 리스트)

    //초기화
    public void Init(List<ItemData.Info> dataList)
    {
        RecipeList = new List<Recipe>();
        RecipeDic = new Dictionary<string, List<Recipe>>();

        //리스트에 추가
        foreach (ItemData.Info item in dataList)
        {
            RecipeList.Add(new Recipe(item));
        }

        RecipeList = RecipeList.OrderBy(x => x.ResultItemID).ToList();  //정렬

        //딕셔너리에 추가
        foreach (Recipe item in RecipeList)
        {
            //키값이 있는지 확인
            if(RecipeDic.TryGetValue(item.GetSourceString(), out List<Recipe> recipe))
            {
                //있으면 레시피리스트에 추가
                recipe.Add(item);
            }
            else
            {
                //없으면 생성
                recipe = new List<Recipe>();
                recipe.Add(item);
                RecipeDic.Add(item.GetSourceString(), recipe);

            }
        }


        //제작 딕셔너리 리스트 정렬
        foreach (List<Recipe> item in RecipeDic.Values)
        {
            if(item.Count >= 2)
            {
                item.Sort((a, b) => a.MaxSourceCount() > b.MaxSourceCount() ? 1 : -1);      //제작 아이템 수에따라 정렬
                item.Reverse();
            }
        }
    }

    //제작 장소
    public E_MakingType GetMakingType()
    {
        return makingType;
    }

    //제작 장소아이디
    public int GetPlaceID()
    {
        return PlaceID;
    }

    //결과 레시피 받기
    public Recipe GetResult(Recipe _recipe)
    {
        RecipeDic.TryGetValue(_recipe.GetSourceString(), out List<Recipe> resultRecipe);    //키값 확인
        if(resultRecipe != null)
        {
            //아이템 갯수 확인
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
    List<MakingInfo> makingList;                         //제작 정보 리스트
    
    public List<Recipe> AllMakingList { get; private set; }                             //모든 제작 정보
    public Dictionary<E_MaterialType, List<Recipe>> MaterialRecipeDic { get; private set; } //마테리얼 별 제작 딕셔너리
    Dictionary<E_MakingType, MakingInfo> MakingDic;      //제작 정보 딕녀서리, (제작타입, 제작정보)

    public List<Recipe> BuildList;                  //건축 리스트
    public Dictionary<int, ObjectPrefab> BuildObjectList;   //건축 딕셔너리(아이디, 프리팹 )

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


    //제작리스트 초기화
    void MakeListInit()
    {
        MakingDic = new Dictionary<E_MakingType, MakingInfo>();     //제작 장소 딕셔너리(제작장소, 제작정보)

        List<ItemData.Info> MakeItemList = CSVManager.Instance.ItemCSVData.FindAll_Craft(true); //제작가능 아이템 리스트
        List<ItemData.Info> tempList;

        //제작 정보 초기화
        foreach (MakingInfo makeinfo in makingList)
        {
            tempList = MakeItemList.FindAll(x => x.Craft_Place == makeinfo.GetPlaceID());
            makeinfo.Init(tempList);
            MakingDic.Add(makeinfo.GetMakingType(), makeinfo);
            tempList.Clear();
        }

        //모든 제작 정보
        AllMakingList = new List<Recipe>();
        foreach (MakingInfo makeinfo in makingList)
        {
            foreach (Recipe recipe in makeinfo.GetRecipeList())
            {
                AllMakingList.Add(recipe);
            }
        }
        AllMakingList = AllMakingList.OrderBy(x => x.ResultItemID).ToList();        //정렬

        //마테리얼 별 제작 정보
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

    //건축정보
    void BuildInit()
    {
        List<ObjectPrefab> MakeBuildList = new List<ObjectPrefab>();        //인공오브젝트 리스트
        //인공오브젝트 찾기
        foreach (ObjectPrefab item in ObjManager.Instance.ObjList)
        {
            ObjectData.Info info = CSVManager.Instance.GetObjectInfo(item.ID);
            if (info !=null && info.Object_Type == E_Object_Type.A_Obj)
                MakeBuildList.Add(item);
        }

        //레시피 등록
        foreach (ObjectPrefab item in MakeBuildList)
        {
            ObjectData.Info info = CSVManager.Instance.GetObjectInfo(item.ID);
            BuildList.Add(new Recipe(info));
        }

        //딕셔너리 초기화, (아이템아이디, 오브젝트 프리팹)
        BuildObjectList = new Dictionary<int, ObjectPrefab>();
        foreach (ObjectPrefab item in MakeBuildList)
        {
            BuildObjectList.Add(item.ID, item);
        }

    }
}
