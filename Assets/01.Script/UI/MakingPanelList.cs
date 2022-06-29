using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakingPanelList : MonoBehaviour
{
    public List<MakingPanel> PanelList;     //제작패널리스트

    void Awake()
    {
    }
    
    /// <summary>
    /// 재료 갯수 감소
    /// </summary>
    public void GetResultItem()
    {
        //만들어진 제작패널 찾기
        foreach (MakingPanel panel in PanelList)
        {
            if (panel.isMakingItem)
                panel.GetResultItem();
        }
    }

    //제작 갯수 세팅
    public void SetResultCount(int _count)
    {
        //만들어진 제작패널 찾기
        foreach (MakingPanel panel in PanelList)
        {
            if (panel.isMakingItem)
                panel.SetResultCount(_count);
        }
    }

}
