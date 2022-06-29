using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModel : MonoBehaviour
{
    MeshRenderer mesh;
    MeshFilter filter;
    BoxCollider collider;

    bool isget;

    //아이템 모델 얻기
    public void GetItemModel(out MeshRenderer _mesh, out MeshFilter _filter, out BoxCollider _collider)
    {
        if(!isget)
        {
            mesh = GetComponent<MeshRenderer>();
            filter = GetComponent<MeshFilter>();
            collider = GetComponent<BoxCollider>();
            isget = true;
        }

        _mesh = mesh;
        _filter = filter;
        _collider = collider;
    }
}
