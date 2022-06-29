using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeGuide : MonoBehaviour
{
    [SerializeField]
    List<ScrollRect> ScrollViewList;            //��ũ�Ѻ� ����Ʈ
    List<Du3Project.UIItemControlLimite> uIItemControlLimite;     //������ ��ũ�� ����
    [SerializeField]
    MakingPanelList m_MakingPanelList;      //���� â ����Ʈ
    [SerializeField]
    Text m_PlaceText;           //���� ��� �ؽ�Ʈ

    public List<Recipe> CurRecipeList { get; private set; }     //���� ���� ������

    void Awake()
    {
        uIItemControlLimite = new List<Du3Project.UIItemControlLimite>(GetComponentsInChildren<Du3Project.UIItemControlLimite>());

        CurRecipeList = MakingManager.Instance.AllMakingList;       //���� ���� ������
        uIItemControlLimite[0].ItemCount = CurRecipeList.Count;     //���� ����Ʈ ��ŭ ���� ����

        //��ũ�Ѻ� ���� ����
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

    //��ũ�Ѻ� ����
    public void _On_FilterList(int _type)
    {
        //���� �����Ǹ���Ʈ ����
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

        //��ũ�Ѻ� Ȱ��ȭ
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
    /// ���� ���̵� ����
    /// </summary>
    /// <param name="_id">�����ϴ� ���̵�</param>
    public void SetGuide(int _id)
    {
        ItemData.Info tempitem = CSVManager.Instance.GetItemInfo(_id);  //������ ����

        foreach (MakingPanel making in m_MakingPanelList.PanelList)
        {
            //�ش�Ǵ� ���� ����Ʈ ã��
            if(making.MakingID == tempitem.Craft_Place)
            {
                making.gameObject.SetActive(true);
                making.SetItem(new Recipe(tempitem));

                //���� ��� �ؽ�Ʈ �����ֱ�
                m_PlaceText.gameObject.SetActive(true);
                switch (making.MakingType)
                {
                    case E_MakingType.Cook:
                        m_PlaceText.text = "��ں�";
                        break;
                    case E_MakingType.Workbench:
                        m_PlaceText.text = "�۾���";
                        break;
                    case E_MakingType.Inven:
                        m_PlaceText.text = "����";
                        break;
                    case E_MakingType.Brazier:
                        m_PlaceText.text = "ȭ��";
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
    /// ������ ��������Ʈ ���
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public Sprite GetItemSprite(int _id)
    {
        return ObjManager.Instance.GetItemPrefab(_id).sprite;
    }
}
