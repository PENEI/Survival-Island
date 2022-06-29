using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeGuide : MonoBehaviour
{
    [SerializeField]
    List<ScrollRect> ScrollViewList;            //스크롤뷰 리스트
    List<Du3Project.UIItemControlLimite> uIItemControlLimite;     //아이템 스크롤 제한
    [SerializeField]
    MakingPanelList m_MakingPanelList;      //제작 창 리스트
    [SerializeField]
    Text m_PlaceText;           //제작 장소 텍스트

    public List<Recipe> CurRecipeList { get; private set; }     //현재 제작 레시피

    void Awake()
    {
        uIItemControlLimite = new List<Du3Project.UIItemControlLimite>(GetComponentsInChildren<Du3Project.UIItemControlLimite>());

        CurRecipeList = MakingManager.Instance.AllMakingList;       //현재 제작 레시피
        uIItemControlLimite[0].ItemCount = CurRecipeList.Count;     //제작 리스트 만큼 갯수 결정

        //스크롤뷰 갯수 세팅
        for(int i = (int)E_MaterialType.Material; i < (int)E_MaterialType.Max - 1; i++)
        {
            MakingManager.Instance.MaterialRecipeDic.TryGetValue((E_MaterialType)i, out List<Recipe> temprecipe);
            uIItemControlLimite[i].ItemCount = temprecipe.Count;
            ScrollViewList[i].gameObject.SetActive(false);
        }

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        m_PlaceText.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        this.gameObject.SetActive(false);
    }

    //스크롤뷰 필터
    public void _On_FilterList(int _type)
    {
        //현재 레시피리스트 세팅
        switch ((E_MaterialType)_type)
        {
            case E_MaterialType.None:
                CurRecipeList = MakingManager.Instance.AllMakingList;
                break;
            case E_MaterialType.Material:
            case E_MaterialType.Tool:
            case E_MaterialType.Armor:
            case E_MaterialType.Use_Item:
                MakingManager.Instance.MaterialRecipeDic.TryGetValue((E_MaterialType)_type, out List<Recipe> temprecipe);
                CurRecipeList = temprecipe;
                break;
        }

        //스크롤뷰 활성화
        for (int i = 0; i < ScrollViewList.Count; i++)
        {
            ScrollViewList[i].gameObject.SetActive(false);
            if(i == _type)
            {
                ScrollViewList[i].gameObject.SetActive(true);
            }
        }

    }

    /// <summary>
    /// 제작 가이드 세팅
    /// </summary>
    /// <param name="_id">제작하는 아이디</param>
    public void SetGuide(int _id)
    {
        ItemData.Info tempitem = CSVManager.Instance.GetItemInfo(_id);  //아이템 정보

        foreach (MakingPanel making in m_MakingPanelList.PanelList)
        {
            //해당되는 제작 리스트 찾기
            if(making.MakingID == tempitem.Craft_Place)
            {
                making.gameObject.SetActive(true);
                making.SetItem(new Recipe(tempitem));

                //제작 장소 텍스트 보여주기
                m_PlaceText.gameObject.SetActive(true);
                switch (making.MakingType)
                {
                    case E_MakingType.Cook:
                        m_PlaceText.text = "모닥불";
                        break;
                    case E_MakingType.Workbench:
                        m_PlaceText.text = "작업대";
                        break;
                    case E_MakingType.Inven:
                        m_PlaceText.text = "가방";
                        break;
                    case E_MakingType.Brazier:
                        m_PlaceText.text = "화로";
                        break;
                }

            }
            else
            {
                making.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 아이콘 스프라이트 얻기
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public Sprite GetItemSprite(int _id)
    {
        return ObjManager.Instance.GetItemPrefab(_id).sprite;
    }
}
