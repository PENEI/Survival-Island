using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TrashPanel : MonoBehaviour
{
    [SerializeField]
    Text m_TrashText;         //������ ������ �ؽ�Ʈ

    RectTransform m_Rect;

    Slot m_TrashSlot;
    string TrashStringText;
    bool isActive;

    public float minY_size;
    public float margine;

    private void Awake()
    {
        isActive = false;
        TrashStringText = "���� �����ðڽ��ϱ�?";
        m_Rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isActive && UIManager.Instance.dragSlotObj.isDraging)
            PanelOff();
    }

    /// <summary>
    /// ������ Ȯ�� �г� ����
    /// </summary>
    /// <param name="_slot">���� ����</param>
    /// <param name="_pos">��ġ</param>
    public void SetTrashPanle(Slot _slot, Vector2 _pos)
    {
        isActive = true;
        this.gameObject.transform.position = _pos;
        UIManager.Instance.SetActiveUI(gameObject, true);
        m_TrashSlot = _slot;
        ItemData.Info info = CSVManager.Instance.GetItemInfo(m_TrashSlot.ItemInfo.itemID);
        m_TrashText.text = "<" + info.Name_Kor + ">" + m_TrashSlot.ItemInfo.count.ToString() + TrashStringText;
        m_Rect.sizeDelta = new Vector2(m_Rect.sizeDelta.x, minY_size + margine + m_TrashText.preferredHeight);


    }

    public void _On_BT_Ok()
    {
        m_TrashSlot.OffSlot();
        PanelOff();
    }

    public void _On_BT_Cancle()
    {
        PanelOff();
    }

    void PanelOff()
    {
        isActive = false;
        m_TrashSlot = null;
        UIManager.Instance.SetActiveUI(gameObject, false);
    }
}
