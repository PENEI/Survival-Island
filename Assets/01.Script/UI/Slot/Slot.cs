using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Text CountText;   //아이템 숫자 텍스트
    [SerializeField]
    Image Img;          //아이템 이미지

    public bool isItem;         //현재 아이템이 있는지
    public bool isDrag;         //현재 드래그할수있는지

    //아이템 정보
    public ItemInfo ItemInfo;      

    public E_SlotType SlotType { get; set; }

    protected Vector2 mousepos;

    RectTransform m_ParentRect; //부모
    RectTransform m_rect;

    void Awake()
    {
        m_ParentRect = transform.parent.parent.GetComponent<RectTransform>();
        m_rect = GetComponent<RectTransform>();
        _On_Init();
        SetSlot(this.ItemInfo);
    }

    void Start()
    {
        //아이템 숫자가 1이하면 표시X
        if (!isItem || ItemInfo.count <= 1)
        {
            CountText.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if(UIManager.Instance != null
            && UIManager.Instance.itemTooltip != null)
            UIManager.Instance.itemTooltip.gameObject.SetActive(false);
    }
    public virtual void _On_Drag_End_Drop() { }
    protected virtual void _On_EndDrag(Vector2 _mouse_pos)
    {
        if (isItem
            && !UIManager.Instance.CountSelect.gameObject.activeSelf)
        {
            //아이템이 인벤토리창 밖으로 나갔을 경우 버리기 실행
            Rect temprect = m_ParentRect.rect;
            Vector2 tempvec = m_ParentRect.position;

            Rect tempinven_rect = UIManager.Instance.InfoPanel.rect;
            Vector2 tempinven_vec = UIManager.Instance.InfoPanel.position;

            if (UIManager.Instance.dragSlotObj.transform.position.x < temprect.xMin + tempvec.x
               || UIManager.Instance.dragSlotObj.transform.position.x > temprect.xMax + tempvec.x
               || UIManager.Instance.dragSlotObj.transform.position.y < temprect.yMin + tempvec.y
               || UIManager.Instance.dragSlotObj.transform.position.y > temprect.yMax + tempvec.y)
            {
                if (UIManager.Instance.dragSlotObj.transform.position.x < tempinven_rect.xMin + tempinven_vec.x
               || UIManager.Instance.dragSlotObj.transform.position.x > tempinven_rect.xMax + tempinven_vec.x
               || UIManager.Instance.dragSlotObj.transform.position.y < tempinven_rect.yMin + tempinven_vec.y
               || UIManager.Instance.dragSlotObj.transform.position.y > tempinven_rect.yMax + tempinven_vec.y)
                    UIManager.Instance.trashPanel.SetTrashPanle(this, _mouse_pos);
            }
        }
    }
    protected virtual void _On_SetSlot(ItemInfo _iteminfo) { }
    protected virtual void _On_OffSlot() { }
    protected virtual void _On_RightClick() { }
    abstract protected void _On_DropSlot();
    abstract protected void _On_Init();

    public void SetAlpha(float _a)
    {
        if(Img != null)
            Img.color = new Color(Img.color.r, Img.color.g, Img.color.b, _a);
    }

    //내구도에 따른 색 변경
    public void SetDurColor()
    {
        Img.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);      //색없음

        if (isItem)
        {
            ItemData.Info info = CSVManager.Instance.GetItemInfo(ItemInfo.itemID);
            bool istool = false;
            switch (info.Material_Type)
            {
                case E_MaterialType.Tool:
                    // 병이 아닐 경우 색 변경
                    if(info.Use_Tool != E_UseTool.Bottle)
                        istool = true;
                    break;
                case E_MaterialType.Armor:
                    istool = true;
                    break;
            }

            //도구 확인
            if(istool)
            {
                //내구도 확인
                if (ItemInfo.durability <= UIManager.Instance.DurabilityCount)
                {
                    Img.color = UIManager.Instance.DurabilityColor;
                }
                else
                {
                    Img.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);  //도구일 경우 검은색
                }
            }
            else
            {
                Img.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);      //아니면 흰색
            }
        }
    }

    //아이템 슬롯에 할당
    public void SetSlot(ItemInfo _iteminfo)
    {
        if (_iteminfo.itemID <= 0)
        {
            OffSlot();
            return;
        }

        isItem = true;                          
        ItemInfo.itemID = _iteminfo.itemID;
        ItemInfo.durability = _iteminfo.durability;

        ItemPrefab item = ObjManager.Instance.GetItemPrefab(_iteminfo.itemID);
        Img.sprite = item.sprite;
        SetCount(_iteminfo.count);
        SetDurColor();

        _On_SetSlot(_iteminfo);
    }


    //슬롯에서 아이템 제거
    public void OffSlot()
    {
        isItem = false;
        ItemInfo.itemID = -1;
        ItemInfo.count = 0;
        ItemInfo.durability = 0;
        CountText.gameObject.SetActive(false);
        Img.sprite = null;
        SetDurColor();

        _On_OffSlot();
    }

    //아이템 숫자 설정
    public void SetCount(int _count)
    {
        ItemInfo.count  = _count;
        CountText.text = _count.ToString();

        //2이상이면 갯수 텍스트 활성화
        if (ItemInfo.count > 1)
        {
            CountText.gameObject.SetActive(true);
        }
        else if(ItemInfo.count <= 0)
        {
            //0일 경우 이 슬롯 아이템 제거
            this.OffSlot();
        }
        else
        {
            CountText.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ItemData.Info info = CSVManager.Instance.GetItemInfo(ItemInfo.itemID);
            bool isUse = false; //아이템 사용 여부
            //아이템 확인
            if (isItem)
            {
                //약품 타입 확인
                if(info.Material_Type == E_MaterialType.Medicene)
                {
                    isUse = true;
                }

                //아이템 타입 확인, 아이템 사용 가능한지 확인
                if(info.Material_Type == E_MaterialType.Use_Item
                    && Player.Instance.Control.isAllowItem)
                {
                    isUse = true;
                }

            }
            if(isUse)
                _On_RightClick();
        }
    }

    //드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        //왼쪽클릭
        if (!isDrag
            || eventData.button != PointerEventData.InputButton.Left)
            return;

        if (isItem)
        {
            UIManager.Instance.dragSlotObj.StartDrag(Img, this);
            UIManager.Instance.dragSlotObj.transform.position = eventData.position;
        }
    }

    //드래그중
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDrag
            || eventData.button != PointerEventData.InputButton.Left)
            return;

        if (isItem)
            UIManager.Instance.dragSlotObj.transform.position = eventData.position;
    }

    //드래그 종료
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDrag
            || eventData.button != PointerEventData.InputButton.Left)
            return;

        _On_EndDrag(eventData.position);

        UIManager.Instance.dragSlotObj.EndDrag();
    }

    //드랍
    public void OnDrop(PointerEventData eventData)
    {
        if (!isDrag
            || eventData.button != PointerEventData.InputButton.Left)
            return;

        if (UIManager.Instance.dragSlotObj.dragSlot == null
            || !UIManager.Instance.dragSlotObj.isDraging
            || !UIManager.Instance.dragSlotObj.dragSlot.isItem
            || UIManager.Instance.dragSlotObj.dragSlot == this)
        {
            return;
        }

        mousepos = eventData.position;

        //아이템이 없을 경우, 아이템이 같을 경우
        if(!isItem
            || UIManager.Instance.dragSlotObj.dragSlot.ItemInfo.itemID == this.ItemInfo.itemID)
            UIManager.Instance.dragSlotObj.dragSlot._On_Drag_End_Drop();

        _On_DropSlot();
    }

    //슬롯 아이템 바꾸기
    protected void ChangeSlot(Slot _slot)
    {
        ItemInfo tempitemInfo = new ItemInfo(ItemInfo);
        SetSlot(_slot.ItemInfo);
        _slot.SetSlot(tempitemInfo);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!UIManager.Instance.dragSlotObj.isDraging
       && isItem)
        {
            ItemData.Info info = CSVManager.Instance.GetItemInfo(ItemInfo.itemID);
            UIManager.Instance.itemTooltip.SetItemTooltip(info.Name_Kor, m_rect);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.itemTooltip.gameObject.SetActive(false);
    }
}
