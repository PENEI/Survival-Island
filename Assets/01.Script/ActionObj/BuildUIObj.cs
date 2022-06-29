using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BuildUIObj : MonoBehaviour, ActionObj
{
    [Header("상호작용 허용")]
    public bool isAllowAction;      //제작 유아이 허용

    [Header("건축 위치")]
    public Transform BuildPosObj;   //건축위치

    bool isMaking;                  //유아이활성화 확인

    [SerializeField]
    public int IncID;
    public BuildInfo buildInfo;


    bool isData;

    void Awake()
    {
    }

    private void Start()
    {
        SetData();
    }


    private void Update()
    {
        //유아이가 활성화 시
        if (isMaking)
        {
            //캐릭터 움직이면 유아이 비활성화
            if (Player.Instance.Control.isMove)
                UIActive();
        }
    }

    /// <summary>
    /// 데이터 세팅
    /// </summary>
    /// <param name="_list"></param>
    public void SetData(List<BuildInfo> _list)
    {
        if (isData)
            return;
        isData = true;

        BuildInfo temp = _list.Find(x => x.IncID == IncID); //같은 아이디 찾기
        //없으면 새로 만듬
        if (temp == null)
        {
            temp = new BuildInfo();
            _list.Add(temp);
            temp.IncID = IncID;     //아이디 세팅
        }

        buildInfo = temp;
        ObjectPrefab prefab;
        //아이템 지어진지 확인
        if (MakingManager.Instance.BuildObjectList.TryGetValue(buildInfo.BuildObjID, out prefab))
            SetBuild(prefab);       //건축
    }

    public void SetData()
    {
        if (isData)
            return;
        isData = true;

        BuildInfo temp = new BuildInfo();
        temp.IncID = IncID;     //아이디 세팅

        buildInfo = temp;
        ObjectPrefab prefab;
        //아이템 지어진지 확인
        if (MakingManager.Instance.BuildObjectList.TryGetValue(buildInfo.BuildObjID, out prefab))
            SetBuild(prefab);       //건축
    }

    public void Action()
    {
        UIActive(); 
    }

    public E_InteractionType GetInteractionType()
    {
        return E_InteractionType.Build;
    }

    public bool IsAction()
    {
        //상호작용 허용 확인, 플레이어가 제작가능인지 확인
        if (isAllowAction && Player.Instance.Control.isAllowMaking)
            return true;

        return false;
    }

    /// <summary>
    /// 건설
    /// </summary>
    /// <param name="_prefab">건설할 프리팹</param>
    public void SetBuild(ObjectPrefab _prefab)
    {
        isAllowAction = false;  //상호작용X
        GameObject.Instantiate(_prefab.gameObject, BuildPosObj.position, _prefab.transform.rotation, this.transform);  //생성

        buildInfo.isComBuild = true;  //건설완
        buildInfo.BuildObjID = _prefab.ID;   //아이디
    }

    //건축유아이 활성화/비활성화
    void UIActive()
    {
        UIManager.Instance.ActiveMakingNInven(UIManager.Instance.BuildPanel.gameObject);
        isMaking = UIManager.Instance.BuildPanel.gameObject.activeSelf;
        if(isMaking)
            UIManager.Instance.BuildPanel.SetBuild(this);
    }
}
