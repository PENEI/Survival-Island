using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Du3Project
{
	public class RecipeItemElement : UIItemElement, IPointerEnterHandler, IPointerExitHandler
    {
        RectTransform m_Rect;

        private void Awake()
        {
            m_Rect = GetComponent<RectTransform>();
        }

        public override void UpdateItem(int p_index)
        {
            ItemIndex = p_index;

            int id = UIManager.Instance.recipeGuide.CurRecipeList[ItemIndex].ResultItemID;
            ItemIcon.sprite = UIManager.Instance.recipeGuide.GetItemSprite(id); //이미지 세팅

            ItemData.Info info = CSVManager.Instance.GetItemInfo(id);
            switch (info.Material_Type)
            {
                case E_MaterialType.Tool:
                case E_MaterialType.Armor:
                    ItemIcon.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);  //도구일 경우 검은색
                    break;
                case E_MaterialType.Material:
                case E_MaterialType.Use_Item:
                    ItemIcon.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);      //아니면 흰색
                    break;
            }
        }

        public override void _On_ImgButton()
        {
            int id = UIManager.Instance.recipeGuide.CurRecipeList[ItemIndex].ResultItemID;
            UIManager.Instance.recipeGuide.SetGuide(id);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            int id = UIManager.Instance.recipeGuide.CurRecipeList[ItemIndex].ResultItemID;
            ItemData.Info info = CSVManager.Instance.GetItemInfo(id);
            UIManager.Instance.itemTooltip.SetItemTooltip(info.Name_Kor, m_Rect, new Vector2(0, -50));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIManager.Instance.itemTooltip.gameObject.SetActive(false);
        }
    }
}