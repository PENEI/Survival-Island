using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandTool : MonoBehaviour
{
    MeshRenderer m_MeshRenderer;        
    MeshFilter m_meshFilter;
    BoxCollider m_Collider;
    GameObject m_ModelObj;

    Vector3 default_collider_size;          //�Ǽ� �ݶ��̴� ������
    Vector3 default_collider_center;       //�Ǽ� �ݶ��̴� ��ġ

    bool isinit;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        if (isinit)
            return;

        //������Ʈ ��������
        m_MeshRenderer = GetComponentInChildren<MeshRenderer>();
        m_meshFilter = GetComponentInChildren<MeshFilter>();
        m_ModelObj = m_meshFilter.gameObject;
        m_Collider = GetComponent<BoxCollider>();

        default_collider_size = m_Collider.size;
        default_collider_center = m_Collider.center;
        //�Ǽ� ����
        DefaultHand();
        isinit = true;
    }

    /// <summary>
    /// �𵨸� ����
    /// </summary>
    /// <param name="_ItemMesh">������ �޽�</param>
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

        m_ModelObj.gameObject.SetActive(true);  //�𵨿�����Ʈ Ȱ��ȭ
        m_MeshRenderer.sharedMaterials = mesh.materials;
        m_meshFilter.sharedMesh = filter.sharedMesh;
        m_Collider.size = collider.size;
        m_Collider.center = collider.center;
        transform.localPosition = _Itemmodel.transform.position;     //��ġ
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
        //�⺻ �ݶ��̴������κ���
        m_Collider.size = default_collider_size; 
        m_Collider.center = default_collider_center;

        m_ModelObj.gameObject.SetActive(false);         //�𵨸� ������Ʈ ��Ȱ��ȭ
    }
}
