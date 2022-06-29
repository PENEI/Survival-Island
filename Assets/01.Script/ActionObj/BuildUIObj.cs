using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BuildUIObj : MonoBehaviour, ActionObj
{
    [Header("��ȣ�ۿ� ���")]
    public bool isAllowAction;      //���� ������ ���

    [Header("���� ��ġ")]
    public Transform BuildPosObj;   //������ġ

    bool isMaking;                  //������Ȱ��ȭ Ȯ��

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
        //�����̰� Ȱ��ȭ ��
        if (isMaking)
        {
            //ĳ���� �����̸� ������ ��Ȱ��ȭ
            if (Player.Instance.Control.isMove)
                UIActive();
        }
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    /// <param name="_list"></param>
    public void SetData(List<BuildInfo> _list)
    {
        if (isData)
            return;
        isData = true;

        BuildInfo temp = _list.Find(x => x.IncID == IncID); //���� ���̵� ã��
        //������ ���� ����
        if (temp == null)
        {
            temp = new BuildInfo();
            _list.Add(temp);
            temp.IncID = IncID;     //���̵� ����
        }

        buildInfo = temp;
        ObjectPrefab prefab;
        //������ �������� Ȯ��
        if (MakingManager.Instance.BuildObjectList.TryGetValue(buildInfo.BuildObjID, out prefab))
            SetBuild(prefab);       //����
    }

    public void SetData()
    {
        if (isData)
            return;
        isData = true;

        BuildInfo temp = new BuildInfo();
        temp.IncID = IncID;     //���̵� ����

        buildInfo = temp;
        ObjectPrefab prefab;
        //������ �������� Ȯ��
        if (MakingManager.Instance.BuildObjectList.TryGetValue(buildInfo.BuildObjID, out prefab))
            SetBuild(prefab);       //����
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
        //��ȣ�ۿ� ��� Ȯ��, �÷��̾ ���۰������� Ȯ��
        if (isAllowAction && Player.Instance.Control.isAllowMaking)
            return true;

        return false;
    }

    /// <summary>
    /// �Ǽ�
    /// </summary>
    /// <param name="_prefab">�Ǽ��� ������</param>
    public void SetBuild(ObjectPrefab _prefab)
    {
        isAllowAction = false;  //��ȣ�ۿ�X
        GameObject.Instantiate(_prefab.gameObject, BuildPosObj.position, _prefab.transform.rotation, this.transform);  //����

        buildInfo.isComBuild = true;  //�Ǽ���
        buildInfo.BuildObjID = _prefab.ID;   //���̵�
    }

    //���������� Ȱ��ȭ/��Ȱ��ȭ
    void UIActive()
    {
        UIManager.Instance.ActiveMakingNInven(UIManager.Instance.BuildPanel.gameObject);
        isMaking = UIManager.Instance.BuildPanel.gameObject.activeSelf;
        if(isMaking)
            UIManager.Instance.BuildPanel.SetBuild(this);
    }
}
