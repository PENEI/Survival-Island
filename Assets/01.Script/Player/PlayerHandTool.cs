using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandTool : MonoBehaviour
{
    MeshRenderer m_MeshRenderer;        
    MeshFilter m_meshFilter;
    BoxCollider m_Collider;
    GameObject m_ModelObj;

    Vector3 default_collider_size;          //맨손 콜라이더 사이즈
    Vector3 default_collider_center;       //맨손 콜라이더 위치

    bool isinit;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        if (isinit)
            return;

        //컴포넌트 가져오기
        m_MeshRenderer = GetComponentInChildren<MeshRenderer>();
        m_meshFilter = GetComponentInChildren<MeshFilter>();
        m_ModelObj = m_meshFilter.gameObject;
        m_Collider = GetComponent<BoxCollider>();

        default_collider_size = m_Collider.size;
        default_collider_center = m_Collider.center;
        //맨손 적용
        DefaultHand();
        isinit = true;
    }

    /// <summary>
    /// 모델링 세팅
    /// </summary>
    /// <param name="_ItemMesh">아이템 메쉬</param>
    public void SetHandTool(ItemModel _Itemmodel)
    {
        Init();
        if (_Itemmodel == null)
        {
            DefaultHand();
            return;
        }

        _Itemmodel.GetItemModel(out MeshRenderer mesh,
            out MeshFilter filter, out BoxCollider collider);

        m_ModelObj.gameObject.SetActive(true);  //모델오브젝트 활성화
        m_MeshRenderer.sharedMaterials = mesh.materials;
        m_meshFilter.sharedMesh = filter.sharedMesh;
        m_Collider.size = collider.size;
        m_Collider.center = collider.center;
        transform.localPosition = _Itemmodel.transform.position;     //위치
        transform.localScale = _Itemmodel.transform.localScale;
        transform.localRotation = _Itemmodel.transform.rotation;
    }

    public void OffHandTool()
    {
        Init();
        DefaultHand();
    }

    void DefaultHand()
    {
        //기본 콜라이더값으로변경
        m_Collider.size = default_collider_size; 
        m_Collider.center = default_collider_center;

        m_ModelObj.gameObject.SetActive(false);         //모델링 오브젝트 비활성화
    }
}
