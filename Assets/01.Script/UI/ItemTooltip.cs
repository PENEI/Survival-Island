using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    [Header("����")]
    public float margine = 10.0f;

    Text m_Text;                        //�ؽ�Ʈ
    RectTransform m_ImgRect;        //�̹��� ��ƮƮ������

    void Awake()
    {
        m_Text = GetComponentInChildren<Text>();
        m_ImgRect = GetComponent<RectTransform>();
    }

    //������ ����
    public void SetItemTooltip(string _itemname, RectTransform _rect)
    {
        gameObject.SetActive(true);
        m_Text.text = _itemname;        //������ �̸� �ؽ�Ʈ ����
        m_ImgRect.sizeDelta = new Vector2(m_Text.preferredWidth + margine,
            m_Text.preferredHeight + margine);      //�̹��� ������

        //���� �̹��� ������
        Vector2 rect = new Vector2(m_ImgRect.sizeDelta.x, m_ImgRect.sizeDelta.y);
        rect *= 0.5f;

        //���Ի�����
        Vector2 slot = new Vector2(_rect.sizeDelta.x, _rect.sizeDelta.y);
        slot *= 0.5f;

        //��ġ
        Vector2 temppos = new Vector2(_rect.gameObject.transform.position.x,
            _rect.gameObject.transform.position.y);

        Vector2 tempsize = new Vector2(rect.x + slot.x, -rect.y + slot.y);

        //ȭ�� �Ѿ���� Ȯ��
        if (Screen.width < temppos.x + tempsize.x + rect.x)
        {
            tempsize.x *= -1;
        }

        this.transform.position = temppos;
        m_ImgRect.anchoredPosition = m_ImgRect.anchoredPosition + tempsize;
    }

    /// <summary>
    /// ������ ���� ����
    /// </summary>
    /// <param name="_itemname">������ �̸�</param>
    /// <param name="_rect">���� Ʈ������</param>
    /// <param name="_pos">�߰� ����</param>
    public void SetItemTooltip(string _itemname, RectTransform _rect, Vector2 _pos)
    {
        gameObject.SetActive(true);
        m_Text.text = _itemname;        //������ �̸� �ؽ�Ʈ ����
        m_ImgRect.sizeDelta = new Vector2(m_Text.preferredWidth + margine,
            m_Text.preferredHeight + margine);      //�̹��� ������

        //���� �̹��� ������
        Vector2 rect = new Vector2(m_ImgRect.sizeDelta.x, m_ImgRect.sizeDelta.y);
        rect *= 0.5f;

        //���Ի�����
        Vector2 slot = new Vector2(_rect.sizeDelta.x, _rect.sizeDelta.y);
        slot *= 0.5f;

        //��ġ
        Vector2 temppos = new Vector2(_rect.gameObject.transform.position.x,
            _rect.gameObject.transform.position.y);

        Vector2 tempsize = new Vector2(rect.x + slot.x, -rect.y + slot.y);

        //ȭ�� �Ѿ���� Ȯ��
        if (Screen.width < temppos.x + tempsize.x + rect.x)
        {
            tempsize.x *= -1;
        }

        this.transform.position = temppos;
        m_ImgRect.anchoredPosition = m_ImgRect.anchoredPosition + tempsize + _pos;
    }
}
