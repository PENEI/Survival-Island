using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakingPanelList : MonoBehaviour
{
    public List<MakingPanel> PanelList;     //�����гθ���Ʈ

    void Awake()
    {
    }
    
    /// <summary>
    /// ��� ���� ����
    /// </summary>
    public void GetResultItem()
    {
        //������� �����г� ã��
        foreach (MakingPanel panel in PanelList)
        {
            if (panel.isMakingItem)
                panel.GetResultItem();
        }
    }

    //���� ���� ����
    public void SetResultCount(int _count)
    {
        //������� �����г� ã��
        foreach (MakingPanel panel in PanelList)
        {
            if (panel.isMakingItem)
                panel.SetResultCount(_count);
        }
    }

}
