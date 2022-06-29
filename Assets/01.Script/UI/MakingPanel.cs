using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MakingPanel : MonoBehaviour
{
    public bool isAllowMaking;              //제작가능 확인

    [Header("제작장소")]
    public E_MakingType MakingType;     //제작 장소
    public int MakingID;

    [Header("연료 사용 확인")]
    public bool isFuel;         //연료 사용 확인
    [Header("연료 아이템 아이디/이름")]
    public int FuelItemID;      //연료 아이디
    public string FuelItemName; //연료 이름

    public bool isMakingItem;       //아이템 만들어진지 확인

    List<DefaultSlot> m_SourceList;     //재료 슬롯 리스트
    MakingResultSlot m_ResultSlot;             //결과물 슬롯
    FuelSlot m_FuelSlot;               //연료 슬롯

    ItemInfo result_item;   //결과 아이템 정보
    Recipe result_recipe;   //결과 레시피

    Recipe itemRecipe;      //유저 아이템 레시피 정보

    void Awake()
    {
        itemRecipe = new Recipe();
        m_SourceList = new List<DefaultSlot>();

        //슬롯 타입에 따라 슬롯할당
        m_SourceList.AddRange(this.gameObject.GetComponentsInChildren<DefaultSlot>());
        m_ResultSlot = this.gameObject.GetComponentInChildren<MakingResultSlot>();
        m_FuelSlot = this.gameObject.GetComponentInChildren<FuelSlot>();
    }


    void Update()
    {
        if(isAllowMaking)
            MakingProcess();
    }

    private void OnDisable()
    {
        if (isAllowMaking)
            GetSourceItem();
    }

    /// <summary>
    /// 결과물 얻을 시 아이템 감소
    /// </summary>
    public void GetResultItem()
    {
        //재료 아이템 갯수 감소
        for (int i = 0; i < result_recipe.SourceArr.Length; i++)
        {
            if(result_recipe.SourceArr[i].ItemID > 0)
            {
                Slot slot = m_SourceList.Find(x => result_recipe.SourceArr[i].ItemID == x.ItemInfo.itemID);//아이템 슬롯 찾기
                slot.SetCount(slot.ItemInfo.count - (result_recipe.SourceArr[i].count * result_item.count));   //감소
                if (isFuel)
                {
                    if (m_FuelSlot.isItem)
                    {
                        if (m_FuelSlot.ItemInfo.itemID == FuelItemID ||
                            CSVManager.Instance.GetItemInfo(m_FuelSlot.ItemInfo.itemID).Name_Eng == FuelItemName)
                            m_FuelSlot.SetCount(m_FuelSlot.ItemInfo.count - result_item.count);
                    }

                }
            }
        }
    }

    public void SetResultCount(int _count)
    {
        result_item.count = _count;
    }

    /// <summary>
    /// 제작 아이템 세팅
    /// </summary>
    /// <param name="_recipe"></param>
    public void SetItem(Recipe _recipe)
    {
        //재료 리스트 초기화
        foreach (Slot slot in m_SourceList)
        {
            slot.OffSlot();
        }

        result_recipe = _recipe;        //제작정보
        ItemData.Info tempitem;
        //재료 세팅
        int inx = 0;
        for (int i = 0; i < result_recipe.SourceArr.Length; i++)
        {
            if(result_recipe.SourceArr[i].ItemID > 0)
            {
                tempitem = CSVManager.Instance.GetItemInfo(result_recipe.SourceArr[i].ItemID);
                m_SourceList[inx++].SetSlot(new ItemInfo(result_recipe.SourceArr[i].ItemID, result_recipe.SourceArr[i].count, tempitem.Dur));

            }
        }

        //연료
        if(isFuel)
        {
            tempitem = CSVManager.Instance.GetItemInfo(FuelItemID);
            m_FuelSlot.SetSlot(new ItemInfo(FuelItemID, 1, tempitem.Dur));
        }

        //결과물
        tempitem = CSVManager.Instance.GetItemInfo(_recipe.ResultItemID);
        result_item = new ItemInfo(_recipe.ResultItemID, 1, tempitem.Dur);
        m_ResultSlot.SetSlot(result_item);

    }

    /// <summary>
    /// 재료아이템 인벤토리로
    /// </summary>
    void GetSourceItem()
    {
        foreach (Slot slot in m_SourceList)
        {
            if (slot.isItem)
            {
                if (UIManager.Instance.invenPanel != null)
                {
                    if (UIManager.Instance.invenPanel.AddItem(slot.ItemInfo))
                    {
                        slot.OffSlot();
                    }
                }
            }
        }
    }

    //만들기
    void MakingProcess()
    {
        MakingInfo info = MakingManager.Instance.GetMakingInfo(MakingType); //제작정보 얻기
        if(info != null)
        {
            //제작 체크
            if(ItemMakeCheck(info, out result_item))
            {
                isMakingItem = true;
                m_ResultSlot.SetSlot(result_item);       //결과 아이템 세팅
            }
            else
            {
                isMakingItem = false;
                m_ResultSlot.OffSlot();
            }
        }
    }

    //제작 체크
    bool ItemMakeCheck(MakingInfo _info, out ItemInfo _resultitem)
    {
        _resultitem = new ItemInfo();             //제작된 아이템

        //연료 확인
        if (isFuel)
        {
            if (!m_FuelSlot.isItem ||
                m_FuelSlot.ItemInfo.itemID != FuelItemID &&
                 CSVManager.Instance.GetItemInfo(m_FuelSlot.ItemInfo.itemID).Name_Eng != FuelItemName)
                return false;
        }

        itemRecipe.ClearSource();

        int itemcount = 0;      //재료슬롯에 아이템
        for (int i = 0; i < m_SourceList.Count; i++)
        {
            if (m_SourceList[i].ItemInfo.itemID > 0)
            {
                //재료 슬롯에 아이템이 있을 경우 아이템데이터 추가
                itemRecipe.SourceArr[i].ItemID = m_SourceList[i].ItemInfo.itemID;   
                itemRecipe.SourceArr[i].count = m_SourceList[i].ItemInfo.count;
                itemcount++;
            }
        }
        //재료슬롯에 아이템이 없으면 false
        if (itemcount <= 0)
            return false;

        itemRecipe.SourceSort();    //정렬

        result_recipe = _info.GetResult(itemRecipe);    //결과 아이템 얻기

        if (result_recipe != null)
        {
            _resultitem.itemID = result_recipe.ResultItemID;     //만들어진 아이템 아이디
            int makecount = MinCount(itemRecipe, result_recipe);    //만들어질 갯수 얻기

            ItemData.Info info = CSVManager.Instance.GetItemInfo(result_recipe.ResultItemID);   //아이템 정보
            _resultitem.count = makecount;      //만들어지는 수량
            _resultitem.durability = info.Dur;  //내구도
            return true;
        }
        return false;
    }

    //최소 갯수 계산
    int MinCount(Recipe _recipe, Recipe _reulstrecipe)
    {
        //갯수 계산
        int count = -1;
        for (int i = 0; i < _recipe.SourceArr.Length; i++)
        {
            if(_recipe.SourceArr[i].ItemID != 0)
            {
                if(count == -1)
                {
                    count = _recipe.SourceArr[i].count / _reulstrecipe.SourceArr[i].count;  
                }
                else
                {
                    int temp = _recipe.SourceArr[i].count / _reulstrecipe.SourceArr[i].count;
                    count = temp > count ? count : temp;        //최소수량 확인
                }

            }
        }

        if (isFuel)
        {
            if (m_FuelSlot.isItem)
            {
                if (m_FuelSlot.ItemInfo.itemID == FuelItemID ||
                 CSVManager.Instance.GetItemInfo(m_FuelSlot.ItemInfo.itemID).Name_Eng == FuelItemName)
                {
                    if (count > m_FuelSlot.ItemInfo.count)
                        count = m_FuelSlot.ItemInfo.count;
                }
            }

        }

        // 최대 스택 확인
        ItemData.Info info = CSVManager.Instance.GetItemInfo(result_recipe.ResultItemID);   //아이템 정보
        if (info.Stack < count)
            count = info.Stack;

        return count;
    }
}
