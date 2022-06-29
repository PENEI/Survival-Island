using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPanel : MonoBehaviour
{
    Du3Project.UIItemControlLimite uIItemControlLimite;     //아이템 스크롤 제한
    [SerializeField]
    Text m_BuildInfoText;       //건축정보 입력 텍스트
    [SerializeField]
    Button OK_Button;
    [SerializeField]
    Image m_BuildPanel;

    Dictionary<int, ObjectPrefab> m_BuildObjectList;    //건축오브젝트 딕셔너리 (아이디, 오브젝트 프리팹)
    Recipe SelectRecipe;        //선택된 레시피
    BuildUIObj m_buildObj;

    bool isinteraction;         //상호작용 중인지
    bool isComplete;            //상호작용 끝확인

    IEnumerator enumerator;
    private void Awake()
    {
        m_BuildObjectList = MakingManager.Instance.BuildObjectList;
        //텍스트 세팅
        SetBuildInfoText(MakingManager.Instance.BuildList[0]);

        uIItemControlLimite = GetComponentInChildren<Du3Project.UIItemControlLimite>();
        uIItemControlLimite.ItemCount = MakingManager.Instance.BuildList.Count;     //건축 리스트 만큼 갯수 결정
    }

    private void Update()
    {
        if(isinteraction)
        {
            //*****
            //플레이어, 카메라가 움직이면 채집완료 불가능
            if (Player.Instance.Control.isMove
                || Player.Instance._Animation.hiting)
            {
                StopCoroutine(enumerator);
                Player.Instance._Animation.hiting = false;
                isinteraction = false;
                isComplete = false;
                Player.Instance.Control.isAllowInteraction = true;  //상호작용 가능하게 바꿈
            }
        }
    }

    private void OnEnable()
    {
        m_BuildPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// 건축 세팅
    /// </summary>
    /// <param name="_buildObj">상호작용된 건축 오브젝트</param>
    public void SetBuild(BuildUIObj _buildObj)
    {
        m_buildObj = _buildObj;
    }


    /// <summary>
    /// 유아이아이콘 스프라이트
    /// </summary>
    /// <param name="_id">오브젝트 아이디</param>
    /// <returns>아이콘 스프라이트, 없으면: null</returns>
    public Sprite GetBuildObjSprite(int _id)
    {
        //키값 찾기
        if(m_BuildObjectList.TryGetValue(_id, out ObjectPrefab tempvaluse))
        {
            return tempvaluse.sprite;
        }
        return null;
    }

    /// <summary>
    /// 건축 정보 텍스트 세팅
    /// </summary>
    /// <param name="_recipe">선택된 레시피</param>
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

            m_BuildInfoText.text = tempstr; //텍스트 세팅
            SelectRecipe = _recipe; //선택된 레시피
        }


        //도구 확인
        if (m_BuildObjectList.TryGetValue(SelectRecipe.ResultItemID, out prefab))
        {
            info = CSVManager.Instance.GetObjectInfo(prefab.ID);
            if (UIManager.Instance.equipPanel.PlayerTool != info.Use_Tool)
            {
                Debug.Log("건축실패: 도구 다름");
                isbuild = false;
            }
        }

        if (isbuild)
        {
            //재료확인
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
    /// 건축 확인 버튼
    /// </summary>
    public void _On_Build_OK()
    {
        StartAction();
    }

    void StartAction()
    {
        //상호작용중이면 X
        enumerator = BuildAction();
        StartCoroutine(enumerator);
    }

    IEnumerator BuildAction()
    {
        Debug.Log("a");
        ObjectPrefab prefab;
        ObjectData.Info info = null;
        //도구 확인
        if (m_BuildObjectList.TryGetValue(SelectRecipe.ResultItemID, out prefab))
        {
            info = CSVManager.Instance.GetObjectInfo(prefab.ID);
        }
        isComplete = true;
        isinteraction = true;
        Player.Instance.Control.SetAllowAll(false); //다른 상호작용 불가능\

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

        //**************************************************************시작

        //사운드 재생
        ItemData.Info iteminfo = CSVManager.Instance.GetItemInfo(
             UIManager.Instance.equipPanel.m_HandSlot.ItemInfo.itemID);
        if (iteminfo != null)
            SoundManager.Instance.PlaySound(E_SoundType.Effect, 50007);

        //건축시간 대기
        yield return new WaitForSeconds(info.Gathering_Time);       //대기


        //**************************************************************끝

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
            Debug.Log($"건축성공, {SelectRecipe.ResultItemID}");

            //오브젝트 생성
            m_buildObj.SetBuild(prefab);
            m_buildObj.Action();
            //*****
            Player.Instance._Status.status.fatigue.statusValue -= info.Craft_Fatigue;       //건축 피로 차감
            m_buildObj = null;
            isinteraction = false;
        }

        Player.Instance.Control.SetAllowAll(true); //다른 상호작용 불가능\ //다른 상호작용 가능
    }
}
