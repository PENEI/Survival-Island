using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPanel : MonoBehaviour
{
    Du3Project.UIItemControlLimite uIItemControlLimite;     //������ ��ũ�� ����
    [SerializeField]
    Text m_BuildInfoText;       //�������� �Է� �ؽ�Ʈ
    [SerializeField]
    Button OK_Button;
    [SerializeField]
    Image m_BuildPanel;

    Dictionary<int, ObjectPrefab> m_BuildObjectList;    //���������Ʈ ��ųʸ� (���̵�, ������Ʈ ������)
    Recipe SelectRecipe;        //���õ� ������
    BuildUIObj m_buildObj;

    bool isinteraction;         //��ȣ�ۿ� ������
    bool isComplete;            //��ȣ�ۿ� ��Ȯ��

    IEnumerator enumerator;
    private void Awake()
    {
        m_BuildObjectList = MakingManager.Instance.BuildObjectList;
        //�ؽ�Ʈ ����
        SetBuildInfoText(MakingManager.Instance.BuildList[0]);

        uIItemControlLimite = GetComponentInChildren<Du3Project.UIItemControlLimite>();
        uIItemControlLimite.ItemCount = MakingManager.Instance.BuildList.Count;     //���� ����Ʈ ��ŭ ���� ����
    }

    private void Update()
    {
        if(isinteraction)
        {
            //*****
            //�÷��̾�, ī�޶� �����̸� ä���Ϸ� �Ұ���
            if (Player.Instance.Control.isMove
                || Player.Instance._Animation.hiting)
            {
                StopCoroutine(enumerator);
                Player.Instance._Animation.hiting = false;
                isinteraction = false;
                isComplete = false;
                Player.Instance.Control.isAllowInteraction = true;  //��ȣ�ۿ� �����ϰ� �ٲ�
            }
        }
    }

    private void OnEnable()
    {
        m_BuildPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="_buildObj">��ȣ�ۿ�� ���� ������Ʈ</param>
    public void SetBuild(BuildUIObj _buildObj)
    {
        m_buildObj = _buildObj;
    }


    /// <summary>
    /// �����̾����� ��������Ʈ
    /// </summary>
    /// <param name="_id">������Ʈ ���̵�</param>
    /// <returns>������ ��������Ʈ, ������: null</returns>
    public Sprite GetBuildObjSprite(int _id)
    {
        //Ű�� ã��
        if(m_BuildObjectList.TryGetValue(_id, out ObjectPrefab tempvaluse))
        {
            return tempvaluse.sprite;
        }
        return null;
    }

    /// <summary>
    /// ���� ���� �ؽ�Ʈ ����
    /// </summary>
    /// <param name="_recipe">���õ� ������</param>
    public void SetBuildInfoText(Recipe _recipe)
    {
        bool isbuild = true;
        ObjectPrefab prefab;
        ObjectData.Info info = CSVManager.Instance.GetObjectInfo(_recipe.ResultItemID);
        if (m_BuildObjectList.TryGetValue(_recipe.ResultItemID, out prefab))
        {
            string tempstr = info.Name_Kor + "\n";

            for (int i = 0; i < info.SourceArr.Length; i++)
            {
                if (info.SourceArr[i].ItemID > 0)
                {
                    ItemData.Info iteminfo = CSVManager.Instance.GetItemInfo(info.SourceArr[i].ItemID);
                    tempstr += "\n" + iteminfo.Name_Kor + " x " + info.SourceArr[i].count;
                }

            }

            m_BuildInfoText.text = tempstr; //�ؽ�Ʈ ����
            SelectRecipe = _recipe; //���õ� ������
        }


        //���� Ȯ��
        if (m_BuildObjectList.TryGetValue(SelectRecipe.ResultItemID, out prefab))
        {
            info = CSVManager.Instance.GetObjectInfo(prefab.ID);
            if (UIManager.Instance.equipPanel.PlayerTool != info.Use_Tool)
            {
                Debug.Log("�������: ���� �ٸ�");
                isbuild = false;
            }
        }

        if (isbuild)
        {
            //���Ȯ��
            foreach (SourceInfo item in SelectRecipe.SourceArr)
            {
                if (!UIManager.Instance.invenPanel.GetHaveItem(item))
                {
                    isbuild = false;
                    break;
                }
            }
        }
        OK_Button.interactable = isbuild;


    }

    /// <summary>
    /// ���� Ȯ�� ��ư
    /// </summary>
    public void _On_Build_OK()
    {
        StartAction();
    }

    void StartAction()
    {
        //��ȣ�ۿ����̸� X
        enumerator = BuildAction();
        StartCoroutine(enumerator);
    }

    IEnumerator BuildAction()
    {
        Debug.Log("a");
        ObjectPrefab prefab;
        ObjectData.Info info = null;
        //���� Ȯ��
        if (m_BuildObjectList.TryGetValue(SelectRecipe.ResultItemID, out prefab))
        {
            info = CSVManager.Instance.GetObjectInfo(prefab.ID);
        }
        isComplete = true;
        isinteraction = true;
        Player.Instance.Control.SetAllowAll(false); //�ٸ� ��ȣ�ۿ� �Ұ���\

        //*****
        Player.Instance._Animation.state = E_Player_State.Work;
        if (!Player.Instance._Animation.ani.GetBool("IdleExit"))
        {
            Player.Instance._Animation.ani.SetBool("IdleExit", true);
        }
        CancelInvoke("IdleAnimation");
        Player.Instance._Animation.ani.SetTrigger("IsWork");
        Player.Instance._Animation.ani.SetInteger("IsSham", 1);
        m_BuildPanel.gameObject.SetActive(false);

        //**************************************************************����

        //���� ���
        ItemData.Info iteminfo = CSVManager.Instance.GetItemInfo(
             UIManager.Instance.equipPanel.m_HandSlot.ItemInfo.itemID);
        if (iteminfo != null)
            SoundManager.Instance.PlaySound(E_SoundType.Effect, 50007);

        //����ð� ���
        yield return new WaitForSeconds(info.Gathering_Time);       //���


        //**************************************************************��

        //*****
        Player.Instance._Animation.ani.SetInteger("IsSham", 2);

        if (isComplete)
        {
            foreach (SourceInfo item in SelectRecipe.SourceArr)
            {
                ItemData.Info tempinfo = CSVManager.Instance.GetItemInfo(item.ItemID);
                if (tempinfo != null)
                {
                    ItemInfo tempitem = new ItemInfo(item.ItemID, item.count, tempinfo.Dur);
                    UIManager.Instance.invenPanel.SubItem(tempitem);
                }
            }
            Debug.Log($"���༺��, {SelectRecipe.ResultItemID}");

            //������Ʈ ����
            m_buildObj.SetBuild(prefab);
            m_buildObj.Action();
            //*****
            Player.Instance._Status.status.fatigue.statusValue -= info.Craft_Fatigue;       //���� �Ƿ� ����
            m_buildObj = null;
            isinteraction = false;
        }

        Player.Instance.Control.SetAllowAll(true); //�ٸ� ��ȣ�ۿ� �Ұ���\ //�ٸ� ��ȣ�ۿ� ����
    }
}
